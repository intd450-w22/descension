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
        public float reticleDistance = 2;

        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) 
            => new Pick(lootChance, reticleDistance, slotIndex, quantity, maxQuantity, inventorySprite);
    }
    
    
    
    // logic for pick item
    [Serializable]
    class Pick : Equippable
    {
        private float _lootChance;
        private float _reticleDistance;
        private bool _execute;
        private PlayerControls _playerControls;

        private Transform _reticle;
        private Transform Reticle => _reticle ? _reticle : _reticle = PlayerController.Reticle;

        public Pick(float lootChance, float reticleDistance, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = PickItem.Name;
            _lootChance = lootChance;
            _reticleDistance = reticleDistance;
            
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite)
            => new Pick(_lootChance, _reticleDistance, slotIndex, quantity, maxQuantity, sprite);

        public override String GetName() => name;
        
        public override void OnEquip() => Reticle.gameObject.SetActive(true);

        public override void OnUnEquip() => Reticle.gameObject.SetActive(false);

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.PickPrefab, GameManager.PlayerController.transform.position, Quantity);

        public override void Update() => _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();

        public override void FixedUpdate()
        {
            PlayerController controller = GameManager.PlayerController;
            Vector3 screenPoint = controller.playerCamera.WorldToScreenPoint(controller.transform.localPosition);
            Vector3 direction = (Input.mousePosition - screenPoint).normalized;
            Vector3 position = controller.transform.position;
            
            Reticle.position = position + (direction * _reticleDistance);
            
            Debug.DrawLine(position, Reticle.position);
            
            
            
            if (!_execute) return;
            _execute = false;
            
            if (Quantity <= 0)
            {
                UIManager.GetHudController().ShowText("No picks!");
                return;
            }

            Vector3 playerPosition = controller.transform.position;
            
            Debug.DrawLine(playerPosition, playerPosition + direction * 3);
            
            RaycastHit2D rayCast = Physics2D.Raycast(playerPosition, direction, 3, (int) UnityLayer.Boulder);
            if (rayCast)
            {
                SoundManager.RemoveRock();
                
                if (Random.Range(0f, 100f) < _lootChance)
                {
                    int gold = Random.Range(1, 21);
                    
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
