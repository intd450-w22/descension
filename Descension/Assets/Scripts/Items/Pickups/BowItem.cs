using System;
using Actor.Objects;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;
using Util.Helpers;


namespace Items.Pickups
{
    public class BowItem : EquippableItem
    {
        public static String Name = "Bow";
        
        [Header("Bow")]
        public GameObject arrowPrefab;
        public float damage = 10;
        public float bowReticleDistance = 2f;

        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) 
            => new Bow(arrowPrefab, damage, bowReticleDistance, slotIndex, quantity, maxQuantity, inventorySprite);
    }
    
    
    
    // logic for bow
    [Serializable]
    class Bow : Equippable
    {
        private Arrows _arrows;
        private Transform _reticle;
        private GameObject _arrowPrefab;
        private float _damage;
        private String _currentControlScheme = ControlScheme.Desktop.ToString();
        private float _bowReticleDistance;
        private bool _execute;
        private PlayerControls _playerControls;

        // gets the arrow quiver if we have one
        private Arrows Arrows
        {
            get
            {
                if (_arrows == null)
                {
                    _arrows = (Arrows) InventoryManager.Slots.Find(slot => slot.name == "Arrows");
                }
                return _arrows;
            }
            set => _arrows = value;
        }

        // gets the reticle object
        private Transform Reticle
        {
            get
            {
                if (_reticle == null)
                {
                    _reticle = PlayerController.Instance.gameObject.GetChildTransformWithName("Reticle");
                }
                return _reticle;
            }
        }
        
        public Bow(GameObject arrowPrefab, float damage, float bowReticleDistance, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = BowItem.Name;
            _arrowPrefab = arrowPrefab;
            _damage = damage;
            _bowReticleDistance = bowReticleDistance;
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite)
            => new Bow(_arrowPrefab, _damage, _bowReticleDistance, slotIndex, quantity, maxQuantity, sprite);

        public override String GetName() => name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.BowPrefab, PlayerPosition, Quantity);

        public override void OnEquip() => Reticle.gameObject.SetActive(true);

        public override void OnUnEquip() => Reticle.gameObject.SetActive(false);

        public override void Update() => _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();

        public override void FixedUpdate()
        {
            var screenPoint = PlayerController.Instance.playerCamera.WorldToScreenPoint(PlayerController.Instance.transform.localPosition);
            var direction = (Input.mousePosition - screenPoint).normalized;
            
            // Set the position of the reticle on the screen according to input type
            if (_currentControlScheme == ControlScheme.Desktop.ToString())
            {
                // Place the reticle on the cursor 
                // TODO: Hide the cursor ? 
                Reticle.position = (Vector2) PlayerController.Instance.playerCamera.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (_currentControlScheme == ControlScheme.Gamepad.ToString())
            {
                // Place the reticle in a ring around the player 
                // TODO: Add aiming with the right stick ala Enter the Gungeon 
                Reticle.position = PlayerPosition + (direction * _bowReticleDistance);
            }
            
            
            //******** Try to Execute if key pressed and have arrows *******//
            if (!_execute) return;
            _execute = false;
            
            if (Arrows == null)
            {
                UIManager.GetHudController().ShowText("No arrows to shoot!");
                return;
            }

            Vector3 playerPosition = PlayerPosition;
            
            Debug.DrawLine(playerPosition, playerPosition + direction * 3);
            
            // spawn arrow
            Projectile.Instantiate(_arrowPrefab, playerPosition + direction, direction, _damage, Tag.Enemy);
            
            // reduce arrows quantity
            if (--Arrows.Quantity <= 0) Arrows = null;
        }
        
    }
}
