using Actor.Interface;
using Managers;
using UnityEngine;
using Util.EditorHelpers;
using Util.Helpers;

namespace Actor.Items.Pickups
{
    public class Pickup : AInteractable, IUnique
    {
        [SerializeField, ReadOnly] private int uniqueId;
        public int GetUniqueId() => uniqueId;
        public void SetUniqueId(int id) => uniqueId = id;
        
        public EquippableItem item;
        public int quantity = 1;
        public string[] pickupMessage;

        [HideInInspector] public GameObject prefab;
        [HideInInspector] public Vector3 location;
        private bool _spawned;
        
        protected void Awake()
        {
            if (GameManager.IsUniqueDestroyed(this)) Destroy(gameObject);
        }
        
        protected void OnEnable() 
        {
            if (GetUniqueId() == 0)
            {
                _spawned = true;
                SpawnManager.AddCachingDelegate(OnSceneChange);
            }
        }

        private void OnDestroy() => SpawnManager.RemoveCachingDelegate(OnSceneChange);

        private void OnSceneChange() => SpawnManager.CachePickup(new PickupCacheInfo(this));

        private void TryPickup()
        {
            if (!InventoryManager.PickupItem(item, ref quantity))
            {
                SoundManager.Error(); //TODO fail to pick up sound
                UIManager.GetHudController().ShowDialogue("Inventory full");
                return;
            }
            
            SoundManager.ItemFound();

            if (quantity == 0)
            {
                if (!_spawned) GameManager.DestroyUnique(this);
                Destroy(gameObject);
            }
                
            // only show pickup dialogue once
            if (!FactManager.IsFactTrue(item.Fact))
            {
                DialogueManager.StartDialogue(item.GetName(), pickupMessage);
                FactManager.SetFact(item.Fact, true);
            }
        }

        private void OnValidate() => gameObject.GetChildObject("ItemSprite").GetComponent<SpriteRenderer>().sprite = item.inventorySprite;

        public override void Interact() => TryPickup();
        public override Vector2 Location() => gameObject.transform.position;
        public override string GetPrompt() => "Press F to pick up " + item.GetName();
    }

    public readonly struct PickupCacheInfo
    {
        public PickupCacheInfo(Pickup pickup)
        {
            Prefab = pickup.prefab;
            Location = pickup.location;
            Quantity = pickup.quantity;
        }
        
        public readonly GameObject Prefab;
        public readonly Vector3 Location;
        public readonly int Quantity;
    }
    
}
