using System.Collections.Generic;
using Actor.Interface;
using Actor.Player;
using Environment;
using Items;
using Items.Pickups;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Util.Enums;
using Util.Helpers;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        private static GameManager Instance => _instance ??= FindObjectOfType<GameManager>();

        protected void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        void Start()
        {
            _globalPostProcessing = FindObjectOfType<postProcessingScript>();
        }

        private postProcessingScript _globalPostProcessing;
        public static postProcessingScript GlobalPostProcessing => Instance._globalPostProcessing;

        [SerializeField] private bool _isPaused;
        public static bool IsPaused => Instance._isPaused;

        [SerializeField] private bool _isFrozen;
        public static bool IsFrozen => Instance._isPaused || Instance._isFrozen;

        public static void Freeze() => Instance.OnFreeze();

        private void OnFreeze()
        {
            GameDebug.Log("OnFreeze");
            _isFrozen = true;
        }

        public static void UnFreeze() => Instance.OnUnFreeze();

        private void OnUnFreeze()
        {
            GameDebug.Log("OnUnFreeze");
            _isFrozen = false;
            InventoryManager.SetCooldown();
        }

        public static void Pause() => Instance.OnPause();
        private void OnPause()
        {
            _isPaused = true;

            GameDebug.Log("OnPause");
            SoundManager.PauseBackgroundAudio();
        }

        public static void Resume() => Instance.OnResume();
        private void OnResume()
        {
            _isPaused = false;

            GameDebug.Log("OnResume");
            SoundManager.ResumeBackgroundAudio();
            InventoryManager.SetCooldown();
        }
        
        
        #region scene management

        public delegate void PreSceneChangeDelegate();
        private static PreSceneChangeDelegate _onSceneChangeDelegate;
        public static void AddPreSceneChangeDelegate(PreSceneChangeDelegate callback) => _onSceneChangeDelegate += callback;
        public static void RemovePreSceneChangeDelegate(PreSceneChangeDelegate callback) => _onSceneChangeDelegate -= callback;

        public static Scene GetCurrentScene() => SceneManager.GetActiveScene();
        public static void SwitchScene(Scene scene, UIType uiType = UIType.None, int startPosition = -1) => Instance._SwitchScene(scene.name, uiType, startPosition);
        public static void SwitchScene(string scene, UIType uiType = UIType.None, int startPosition = -1) => Instance._SwitchScene(scene, uiType, startPosition);
        private void _SwitchScene(string scene, UIType uiType = UIType.None, int startPosition = -1)
        {
            GameDebug.Log("SwitchScene(" + scene + ")");
            
            if (startPosition != -1) PlayerController.SetStartPosition(startPosition);

            SpawnManager.CacheSpawnedPickups();
            
            _onSceneChangeDelegate?.Invoke();
            _onSceneChangeDelegate = null;

            AsyncOperation load = SceneManager.LoadSceneAsync(scene);

            if (uiType != UIType.None)
                this.InvokeWhen(
                    () => UIManager.SwitchUi(uiType),
                    () => load.isDone,
                    0.5f);
        }
        
        
        

        # endregion
        
        #region static caching interface
        
        // cache object destroyed in level. Will only be permanent if the player completes the level.
        // Use with IsUniqueDestroyed(IUnique).
        public static void DestroyUnique(IUnique unique)
        {
            DestroyedUnique.Add(unique.GetUniqueId());
        }
        
        // cache object destroyed in level. Will only be permanent if the player completes the level.
        // Use with IsUniqueDestroyed(IUnique, Vector3).
        public static void DestroyUnique(IUnique unique, Vector3 location)
        {
            DestroyedUniqueWithLocation.Add(unique.GetUniqueId(), location);
        }
        
        // permanently destroy object. Use with IsUniqueDestroyed(IUnique).
        public static void DestroyUniquePermanent(IUnique unique)
        {
            PermanentDestroyedUnique.Add(unique.GetUniqueId());
        }

        // permanently destroy object. Use with IsUniqueDestroyed(IUnique, Vector3).
        public static void DestroyUniquePermanent(IUnique unique, Vector3 location)
        {
            PermanentDestroyedUniqueWithLocation.Add(unique.GetUniqueId(), location);
        }
        
        // returns true if object is permanently destroyed.
        // Use with DestroyUnique(IUnique) or DestroyUniquePermanent(IUnique).
        public static bool IsUniqueDestroyed(IUnique unique)
        {
            return PermanentDestroyedUnique.Contains(unique.GetUniqueId());
        }

        // returns true if object is permanently destroyed and outputs location set when DestroyUnique was called.
        // Use with DestroyUnique(IUnique,Vector2) or DestroyUniquePermanent(IUnique,Vector2).
        public static bool IsUniqueDestroyed(IUnique unique, out Vector3 location)
        {
            if (PermanentDestroyedUniqueWithLocation.ContainsKey(unique.GetUniqueId()))
            {
                location = PermanentDestroyedUniqueWithLocation[unique.GetUniqueId()];
                return true;
            }

            location = Vector3.zero;
            return false;
        }

        private static readonly Dictionary<int, Vector2> DestroyedUniqueWithLocation = new Dictionary<int, Vector2>();
        private static readonly Dictionary<int, Vector2> PermanentDestroyedUniqueWithLocation = new Dictionary<int, Vector2>();
        private static readonly HashSet<int> DestroyedUnique = new HashSet<int>();
        private static readonly HashSet<int> PermanentDestroyedUnique = new HashSet<int>();

        public static void ClearDestroyedCache()
        {
            DestroyedUniqueWithLocation.Clear();
            PermanentDestroyedUniqueWithLocation.Clear();

            DestroyedUnique.Clear();
            PermanentDestroyedUnique.Clear();
        }

        public static void OnSceneComplete()
        {
            foreach (var destroyed in DestroyedUniqueWithLocation)
                PermanentDestroyedUniqueWithLocation[destroyed.Key] = destroyed.Value;
            
            DestroyedUniqueWithLocation.Clear();
            
            foreach (var destroyed in DestroyedUnique)
                PermanentDestroyedUnique.Add(destroyed);
            
            DestroyedUnique.Clear();
        }
        public static void OnReloadScene()
        {
            DestroyedUniqueWithLocation.Clear();
            DestroyedUnique.Clear();
        }
        
        #endregion
        
        
    }
}
