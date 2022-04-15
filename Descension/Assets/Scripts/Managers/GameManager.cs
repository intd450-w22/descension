using System.Collections.Generic;
using System.Linq;
using Actor.Interface;
using Actor.Player;
using Environment;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Util;
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
        
        public static Scene GetCurrentScene() => SceneManager.GetActiveScene();
        public static void SwitchScene(Scene scene, UIType uiType = UIType.None, int startPosition = -1) => Instance._SwitchScene(scene.name, uiType, startPosition);
        public static void SwitchScene(string scene, UIType uiType = UIType.None, int startPosition = -1) => Instance._SwitchScene(scene, uiType, startPosition);
        private void _SwitchScene(string scene, UIType uiType = UIType.None, int startPosition = -1)
        {
            GameDebug.Log("SwitchScene(" + scene + ")");
            
            if (startPosition != -1) PlayerController.SetStartPosition(startPosition);
            
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
        public static void DestroyUnique(IUnique unique)
        {
            // #if UNITY_EDITOR
            // Assert.IsFalse(unique.CacheOnDestroyed(), this + _assertMessage);
            // #endif
            
            _destroyedUnique.Add(unique.GetUniqueId());
        }
        
        // cache object destroyed in level. Will only be permanent if the player completes the level.
        public static void DestroyUnique(IUnique unique, Vector3 location)
        {
            // #if UNITY_EDITOR
            // Assert.IsTrue(unique.CacheOnDestroyed(), this + _assertMessage);
            // #endif
            
            _destroyedUniqueWithLocation.Add(unique.GetUniqueId(), location);
        }
        
        // permanently destroy object
        public static void DestroyUniquePermanent(IUnique unique)
        {
           
            // #if UNITY_EDITOR
            // Assert.IsFalse(unique.CacheOnDestroyed(), unique + _assertMessage);
            // #endif
            
            _permanentDestroyedUnique.Add(unique.GetUniqueId());
        }

        // permanently destroy object
        public static void DestroyUniquePermanent(IUnique unique, Vector3 location)
        {
            // #if UNITY_EDITOR
            // Assert.IsTrue(unique.CacheOnDestroyed(), unique + _assertMessage);
            // #endif
            
            _permanentDestroyedUniqueWithLocation.Add(unique.GetUniqueId(), location);
        }
        
        // returns true if object is permanently destroyed
        public static bool IsUniqueDestroyed(IUnique unique)
        {
            // #if UNITY_EDITOR
            // unique.SeCacheOnDestroyed(false);
            // #endif
            
            return _permanentDestroyedUnique.Contains(unique.GetUniqueId());
        }

        // returns true if object is permanently destroyed and outputs location set when DestroyUnique was called
        public static bool IsUniqueDestroyed(IUnique unique, out Vector3 location)
        {
            // #if UNITY_EDITOR
            // unique.SeCacheOnDestroyed(true);
            // #endif
            
            if (_permanentDestroyedUniqueWithLocation.ContainsKey(unique.GetUniqueId()))
            {
                location = _permanentDestroyedUniqueWithLocation[unique.GetUniqueId()];
                return true;
            }

            location = Vector3.zero;
            return false;
        }

        private static Dictionary<int, Vector2> _destroyedUniqueWithLocation = new Dictionary<int, Vector2>();
        private static Dictionary<int, Vector2> _permanentDestroyedUniqueWithLocation = new Dictionary<int, Vector2>();
        private static HashSet<int> _destroyedUnique = new HashSet<int>();
        private static HashSet<int> _permanentDestroyedUnique = new HashSet<int>();
        private static HashSet<int> _uniqueIds = new HashSet<int>();

        public static void ClearUniqueIds() => _uniqueIds.Clear();

        public static int GenerateNewUniqueId(IUnique unique)
        {
            var id = unique.GetInstanceID();
            while (_uniqueIds.Contains(id)) ++id;
            _uniqueIds.Add(id);
            unique.SetUniqueId(id);
            return id;
        }
        
        public static void ClearDestroyedCache()
        {
            _destroyedUniqueWithLocation.Clear();
            _permanentDestroyedUniqueWithLocation.Clear();

            _destroyedUnique.Clear();
            _permanentDestroyedUnique.Clear();
        }

        public static void OnSceneComplete()
        {
            foreach (var destroyed in _destroyedUniqueWithLocation)
                _permanentDestroyedUniqueWithLocation[destroyed.Key] = destroyed.Value;
            
            _destroyedUniqueWithLocation.Clear();
            
            foreach (var destroyed in _destroyedUnique)
                _permanentDestroyedUnique.Add(destroyed);
            
            _destroyedUnique.Clear();
        }
        public static void OnReloadScene()
        {
            _destroyedUniqueWithLocation.Clear();
            _destroyedUnique.Clear();
        }
        
        #endregion
        
        
    }
}
