using System;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace Items.Pickups
{
    public class PickItem : EquippableItem
    {
        public static String Name = "Pick";

        [Header("Pick")]
        public float lootChance = 40;

        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity)
        {
            return new Pick(lootChance, slotIndex, quantity, maxQuantity, inventorySprite);
        }
    }
    
    
    
    // logic for pick item
    [Serializable]
    class Pick : Equippable
    {
        private float _lootChance;
        private bool _execute;
        private PlayerControls _playerControls;
        
        
        public Pick(float lootChance, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            this.name = GetName();
            _lootChance = lootChance;
            
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }

        public override String GetName()
        {
            return PickItem.Name;
        }

        public override void SpawnDrop()
        {
            ItemSpawner.Instance.DropItem(ItemSpawner.Instance.pickPickupPrefab, Quantity);
        }

        public override void Update()
        {
            _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();
        }

        public override void FixedUpdate()
        {
            if (!_execute) return;
            _execute = false;
            
            if (Quantity <= 0)
            {
                UIManager.GetHudController().ShowText("No picks!");
                return;
            }
            
            PlayerController controller = GameManager.PlayerController;
            
            var screenPoint = controller.playerCamera.WorldToScreenPoint(controller.transform.localPosition);
            var direction = (Input.mousePosition - screenPoint).normalized;
            
            Vector3 playerPosition = controller.transform.position;
            
            Debug.DrawLine(playerPosition, playerPosition + direction * 3);
            
            RaycastHit2D rayCast = Physics2D.Raycast(playerPosition, direction, 3, (int) UnityLayer.Boulder);
            if (rayCast)
            {
                SoundManager.RemoveRock();
                
                if (Random.Range(0f, 100f) < _lootChance)
                {
                    int gold = (int) Mathf.Floor(Random.Range(0f, 20f));
                    
                    SoundManager.GoldFound();
                    
                    InventoryManager.Gold += gold;
                    
                    UIManager.GetHudController()
                        .ShowFloatingText(rayCast.transform.position, "Gold +" + gold, Color.yellow);
                }
                
                Object.Destroy(rayCast.transform.gameObject);
                --Quantity;
            }
            else
            {
                Debug.Log("Raycast Miss");
            }
        }
        
    }
}
