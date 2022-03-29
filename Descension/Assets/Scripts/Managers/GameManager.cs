using Actor.Player;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        private static GameManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<GameManager>();
                return _instance;
            }
        }

        private static PlayerController _playerController;
        public static PlayerController PlayerController
        {
            get
            {
                if (_playerController == null) _playerController = FindObjectOfType<PlayerController>();
                return _playerController;
            }
        }

        protected void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                _instance = this;
            }
        }

        private bool _isPaused;
        public static bool IsPaused => Instance._isPaused;

        private bool _isFrozen;
        public static bool IsFrozen => Instance._isPaused || Instance._isFrozen;

        public static void Freeze() => Instance.OnFreeze();

        private void OnFreeze()
        {
            _isFrozen = true;
        }

        public static void UnFreeze() => Instance.OnUnFreeze();

        private void OnUnFreeze()
        {
            _isFrozen = false;
        }

        public static void Pause() => Instance.OnPause();
        private void OnPause()
        {
            _isPaused = true;

            Debug.Log("OnPause");
            SoundManager.PauseBackgroundAudio();
        }

        public static void Resume() => Instance.OnResume();
        private void OnResume()
        {
            _isPaused = false;

            Debug.Log("OnResume");
            SoundManager.ResumeBackgroundAudio();
        }

    }
}
