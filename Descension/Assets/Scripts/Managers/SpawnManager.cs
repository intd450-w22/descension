using System;
using System.Collections.Generic;
using Actor.Items.Pickups;
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
        
        #region Caching
        
        private static readonly Dictionary<string, HashSet<PickupCacheInfo>> DroppedPickups = new Dictionary<string, HashSet<PickupCacheInfo>>();

        public delegate void PreSceneChangeDelegate();
        private static PreSceneChangeDelegate _onCachingDelegate;
        public static void AddCachingDelegate(PreSceneChangeDelegate callback) => _onCachingDelegate += callback;
        public static void RemoveCachingDelegate(PreSceneChangeDelegate callback) => _onCachingDelegate -= callback;
        private static void ClearSceneCachingDelegates() => _onCachingDelegate = null;
        private static void InvokeCachingDelegates()
        {
            _onCachingDelegate?.Invoke();
            ClearSceneCachingDelegates();
        }

        // clears cache and calls all CachingDelegates
        private static void CacheSpawnedPickups()
        {
            ClearDroppedPickupsCache();
            InvokeCachingDelegates();
        }
        
        // spawns all cached pickups
        public static void SpawnCachedPickups()
        {
            var scene = SceneManager.GetActiveScene().name;

            if (!DroppedPickups.ContainsKey(scene)) return;
            
            foreach (var pickup in DroppedPickups[scene])
                SpawnItem(pickup.Prefab, pickup.Location, pickup.Quantity, true);
        }
        
        public static void CachePickup(PickupCacheInfo pickupCacheInfo)
        {
            var scene = SceneManager.GetActiveScene();
            if (!DroppedPickups.ContainsKey(scene.name)) DroppedPickups.Add(scene.name, new HashSet<PickupCacheInfo>());
            DroppedPickups[scene.name].Add(pickupCacheInfo);
        }
        
        private static void ClearDroppedPickupsCache()
        {
            var scene = SceneManager.GetActiveScene().name;
            if (DroppedPickups.ContainsKey(scene)) DroppedPickups[scene].Clear();
        }
        
        #endregion

        #region Spawning
        
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
            pickup.location = position;
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
        
        #endregion

        public static void OnReloadScene() => ClearSceneCachingDelegates();
        public static void OnSceneComplete() => CacheSpawnedPickups();
        public static void ClearLevelCache() => ClearSceneCachingDelegates();
        public static void ClearGameCache() => DroppedPickups.Clear();
    }
    
    

    
}