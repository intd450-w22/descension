using Environment;
using Managers;
using UnityEngine;

namespace Items
{
    public class ItemSpawner : MonoBehaviour
    {
        public GameObject PickPickupPrefab;
        public GameObject BowPickupPrefab;
        public GameObject QuiverPickupPrefab;

        private static ItemSpawner _instance;
        public static ItemSpawner Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<ItemSpawner>();
                return _instance;
            }
        }
        
        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        public void DropItem(GameObject prefab, int quantity)
        {
            Vector3 playerPosition = GameManager.PlayerController.transform.position;
            
            // spawn arrow
            SoundManager.Instance.ItemFound();
            GameObject pickup = Instantiate(prefab, playerPosition + new Vector3(0,10,0), Quaternion.identity);
            pickup.GetComponent<Pickup>().quantity = quantity;
        }
    }
}