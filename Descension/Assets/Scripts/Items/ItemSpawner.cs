using System;
using System.Collections.Generic;
using Items.Pickups;
using JetBrains.Annotations;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

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
        public static GameObject TorchPrefab => Instance.torchPickupPrefab;
        public GameObject torchPickupPrefab;
        
        private static ItemSpawner _instance;
        private static ItemSpawner Instance => _instance ??= FindObjectOfType<ItemSpawner>();

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

        
        public static void SpawnRandom(Vector3 position, DropStruct[] prefabs) => Instance._SpawnRandom(position, prefabs);
        private void _SpawnRandom(Vector3 position, DropStruct[] prefabs)
        {
            if (prefabs == null) return;

            float roll = Random.Range(0f, 100f);
            float i = 0;
            foreach (DropStruct prefab in prefabs)
            {
                i += prefab.dropChance;
                if (i >= roll)
                {
                    if (prefab.quantity != 0) _SpawnItem(prefab.item, position, prefab.quantity);
                    else _SpawnItem(prefab.item, position);
                    return;
                }
            }
        }
        
        [Serializable]
        public struct DropStruct
        {
            public DropStruct(GameObject item, int dropChance, int quantity = 0)
            {
                this.item = item;
                this.dropChance = dropChance;
                this.quantity = quantity;
            }
            
            [Header("Item must be a pickup prefab")]
            public GameObject item; // item pickup prefab
            public float dropChance;  // percent drop chance
            [Header("Leave quantity at 0 for prefab default quantity")]
            public int quantity;    // leave at 0 for default quantity
        }
    }
    
    

    
}