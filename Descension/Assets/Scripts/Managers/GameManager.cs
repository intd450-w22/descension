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

        public static bool IsPaused
        {
            get => Instance.isPaused;
            set => Instance.isPaused = value;
        }
        public bool isPaused;

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
        

    }
}
