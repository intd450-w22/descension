using System;
using Environment;
using Items.Pickups;
using Managers;
using UnityEngine;

namespace Items
{
    public class ItemSpawner : MonoBehaviour
    {
        // public struct Item
        // {
        //     public GameObject item;
        //     public String pickupMessage;
        // };
        
        
        public GameObject pickPickupPrefab;
        public GameObject bowPickupPrefab;
        public GameObject arrowsPickupPrefab;

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
            
            // spawn pickup
            SoundManager.Instance.ItemFound();
            GameObject pickupObject = Instantiate(prefab, playerPosition, Quaternion.identity);
            Pickup pickup = pickupObject.GetComponent<Pickup>();
            pickup.quantity = quantity;
            UIManager.Instance.GetHudController().ShowText(pickup.item.GetName() + " Dropped");
        }
    }
}