using Actor.Player;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<GameManager>();

                return _instance;
            }
            set => _instance = value;
        }

        private static PlayerController _playerController;
        
        public static PlayerController PlayerController
        {
            get
            {
                if (_playerController == null)
                    _playerController = FindObjectOfType<PlayerController>();

                return _playerController;
            }
            set => _playerController = value;
        }
        
        

        public bool IsPaused;

        protected void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                _instance = this;
            }
        }
        

    }
}
