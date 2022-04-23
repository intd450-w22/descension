using UnityEngine;

namespace Managers
{
    public class Manager : MonoBehaviour
    {
        private static Manager _instance;
        private static Manager Instance => _instance ??= FindObjectOfType<Manager>();

        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
}
