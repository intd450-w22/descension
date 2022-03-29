using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;
    private static Manager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Manager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
