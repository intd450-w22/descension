using System.Collections.Generic;
using System.Linq;
using Actor.Interface;
using Actor.Player;
using Environment;
using UnityEditor;
using UnityEngine;
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
        
        
        
        public static void ClearDestroyedCache() => UniqueMonoBehaviour.ClearDestroyedCache();
        public static void OnSceneComplete() => UniqueMonoBehaviour.OnSceneComplete();
        public static void OnReloadScene() => UniqueMonoBehaviour.OnReloadScene();

        # endregion
        
        
        
        
    }
}
