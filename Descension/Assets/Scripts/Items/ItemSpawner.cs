using System.Collections.Generic;
using Items.Pickups;
using Managers;
using UnityEngine;

namespace Items
{
    public class ItemSpawner : MonoBehaviour
    {
        
        public static GameObject PickPrefab => Instance.pickPickupPrefab;
        public GameObject pickPickupPrefab;
        public static GameObject BowPrefab => Instance.bowPickupPrefab;
        public GameObject bowPickupPrefab;
        public static GameObject SwordPrefab => Instance.swordPickupPrefab;
        public GameObject swordPickupPrefab;
        public static GameObject ArrowsPrefab => Instance.arrowsPickupPrefab;
        public GameObject arrowsPickupPrefab;
        public static GameObject HealthPrefab => Instance.healthPickupPrefab;
        public GameObject healthPickupPrefab;
        public static GameObject ExplosivesPrefab => Instance.explosivesPickupPrefab;
        public GameObject explosivesPickupPrefab;
        public static GameObject TimerPrefab => Instance.timerPickupPrefab;
        public GameObject timerPickupPrefab;
        public static GameObject TriggerPrefab => Instance.triggerPickupPrefab;
        public GameObject triggerPickupPrefab;

        private static ItemSpawner _instance;
        private static ItemSpawner Instance
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
        }

        public static Pickup SpawnItem(GameObject prefab, Vector3 position) => Instance._SpawnItem(prefab, position);
        private Pickup _SpawnItem(GameObject prefab, Vector3 position)
        {
            if (prefab == null)
            {
                Debug.LogError("ItemSpawner DropItem called with null prefab.");
                return null;
            }
            
            // spawn pickup
            SoundManager.ItemFound(); // TODO maybe replace with unique item drop sound
            GameObject pickupObject = Instantiate(prefab, position, Quaternion.identity);
            return pickupObject.GetComponent<Pickup>();
        }

        public static Pickup SpawnItem(GameObject prefab, Vector3 position, int quantity) => Instance._SpawnItem(prefab, position, quantity);
        private Pickup _SpawnItem(GameObject prefab, Vector3 position, int quantity)
        {
            if (prefab == null)
            {
                Debug.LogError("ItemSpawner DropItem called with null prefab.");
                return null;
            }
            
            // do not spawn if no quantity
            if (quantity <= 0) return null;
            
            // spawn pickup
            Pickup pickup = SpawnItem(prefab, position);
            pickup.quantity = quantity;
            UIManager.GetHudController().ShowText(pickup.item.GetName() + " Dropped");
            return pickup;
        }

        public static void SpawnRandom(Vector3 position, GameObject[] prefabs = null) => Instance._SpawnRandom(position, prefabs);
        private void _SpawnRandom( Vector3 position, GameObject[] prefabs = null)
        {
            if (prefabs == null)
            {
                prefabs = new[]
                {
                    pickPickupPrefab, 
                    bowPickupPrefab, 
                    swordPickupPrefab, 
                    arrowsPickupPrefab, 
                    healthPickupPrefab
                };
            }
                
            int index = Random.Range(0, prefabs.Length);
            _SpawnItem(prefabs[index], position);
        }
    }
}