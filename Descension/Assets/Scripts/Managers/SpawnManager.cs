using System;
using System.Collections.Generic;
using Items.Pickups;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Managers
{
    public class SpawnManager : MonoBehaviour
    {
        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }
        
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
        
        private static SpawnManager _instance;
        private static SpawnManager Instance => _instance ??= FindObjectOfType<SpawnManager>();
        
        private static readonly Dictionary<string, HashSet<Pickup>> DroppedPickups = new Dictionary<string, HashSet<Pickup>>();

        public static void SpawnDroppedPickups()
        {
            var scene = SceneManager.GetActiveScene().name;

            if (!DroppedPickups.ContainsKey(scene)) return;
            
            foreach (var pickup in DroppedPickups[scene])
                SpawnItem(pickup.prefab, pickup.position, pickup.quantity, true);

            DroppedPickups[scene].Clear();
        }
        
        public static void CachePickup(Pickup pickup)
        {
            var scene = SceneManager.GetActiveScene();
            if (!DroppedPickups.ContainsKey(scene.name)) DroppedPickups.Add(scene.name, new HashSet<Pickup>());
            DroppedPickups[scene.name].Add(pickup);
        }
        
        public static Pickup SpawnItem(GameObject prefab, Vector3 position, bool silent = false) => Instance._SpawnItem(prefab, position, silent);
        private Pickup _SpawnItem(GameObject prefab, Vector3 position, bool silent)
        {
            if (prefab == null)
            {
                Debug.LogError("ItemSpawner DropItem called with null prefab.");
                return null;
            }

            // spawn pickup
            if (!silent) SoundManager.ItemFound(); // TODO maybe replace with unique item drop sound
            GameObject pickupObject = Instantiate(prefab, position, Quaternion.identity);
            Pickup pickup = pickupObject.GetComponent<Pickup>();
            pickup.prefab = prefab;
            pickup.position = position;
            return pickup;
        }

        public static Pickup SpawnItem(GameObject prefab, Vector3 position, int quantity, bool silent = false) => Instance._SpawnItem(prefab, position, quantity, silent);
        private Pickup _SpawnItem(GameObject prefab, Vector3 position, int quantity, bool silent)
        {
            if (prefab == null)
            {
                Debug.LogError("ItemSpawner DropItem called with null prefab.");
                return null;
            }
            
            // do not spawn if no quantity
            if (quantity <= 0) return null;
            
            // spawn pickup
            Pickup pickup = SpawnItem(prefab, position, silent);
            pickup.quantity = quantity;
            if (!silent) DialogueManager.ShowPrompt(pickup.item.GetName() + " Dropped");
            return pickup;
        }

        
        public static void SpawnRandom(Vector3 position, DropStruct[] prefabs, bool silent = false) => Instance._SpawnRandom(position, prefabs, silent);
        private void _SpawnRandom(Vector3 position, DropStruct[] prefabs, bool silent)
        {
            if (prefabs == null) return;

            float roll = Random.Range(0f, 100f);
            float i = 0;
            foreach (DropStruct prefab in prefabs)
            {
                i += prefab.dropChance;
                if (i >= roll)
                {
                    if (prefab.quantity != 0) _SpawnItem(prefab.item, position, prefab.quantity, silent);
                    else _SpawnItem(prefab.item, position, silent);
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