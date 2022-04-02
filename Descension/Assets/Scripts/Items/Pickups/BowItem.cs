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
        public float spriteOffset = 2;
        public float spriteRotationOffset = 45;
        public Vector2 spritePositionOffset;
        public float bowReticleDistance = 2f;
         
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) 
            => new Bow(arrowPrefab, damage, bowReticleDistance, spriteOffset, spriteRotationOffset, spritePositionOffset, slotIndex, quantity, maxQuantity, inventorySprite);
    }
    
    
    
    // logic for bow
    [Serializable]
    class Bow : Equippable
    {
        private Arrows _arrows;
        private GameObject _arrowPrefab;
        private float _damage;
        private String _currentControlScheme = ControlScheme.Desktop.ToString();
        private float _bowReticleDistance;
        private float _spriteOffset;
        private float _spriteRotationOffset;
        private Vector3 _positionOffset;
        private bool _execute;
        private PlayerControls _playerControls;

        // gets the arrow quiver if we have one
        private Arrows Arrows
        {
            get => _arrows ??= (Arrows) InventoryManager.Slots.Find(slot => slot.name == "Arrows");
            set => _arrows = value;
        }

        
        
        public Bow(GameObject arrowPrefab, float damage, float bowReticleDistance, float spriteOffset, float spriteRotationOffset, Vector2 positionOffset, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = BowItem.Name;
            _arrowPrefab = arrowPrefab;
            _damage = damage;
            _bowReticleDistance = bowReticleDistance;
            _spriteOffset = spriteOffset;
            _spriteRotationOffset = spriteRotationOffset;
            _positionOffset = positionOffset;
            _playerControls = new PlayerControls();
            _playerControls.Enable();
            
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite)
            => new Bow(_arrowPrefab, _damage, _bowReticleDistance, _spriteOffset, _spriteRotationOffset, _positionOffset, slotIndex, quantity, maxQuantity, sprite);

        public override String GetName() => name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.BowPrefab, PlayerPosition, Quantity);

        public override void OnEquip()
        {
            Reticle.gameObject.SetActive(true);
            Sprite = inventorySprite;
            SpriteTransform.gameObject.SetActive(true);
        }

        public override void OnUnEquip()
        {
            Reticle.gameObject.SetActive(false);
            SpriteTransform.gameObject.SetActive(false);
        }

        public override void Update() => _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();

        public override void FixedUpdate()
        {
            var screenPoint = PlayerController.Camera.WorldToScreenPoint(PlayerController.Instance.transform.localPosition);
            var direction = (Input.mousePosition - screenPoint).normalized;
            
            // Set the position of the reticle on the screen according to input type
            if (_currentControlScheme == ControlScheme.Desktop.ToString())
            {
                // Place the reticle on the cursor 
                // TODO: Hide the cursor ? 
                Reticle.position = (Vector2) PlayerController.Camera.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (_currentControlScheme == ControlScheme.Gamepad.ToString())
            {
                // Place the reticle in a ring around the player 
                // TODO: Add aiming with the right stick ala Enter the Gungeon 
                Reticle.position = PlayerPosition + (direction * _bowReticleDistance);
            }
            
            float bowAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + _spriteRotationOffset;
            Vector3 bowPosition = PlayerPosition + _positionOffset + direction * _spriteOffset;
            SpriteTransform.SetPositionAndRotation(bowPosition, new Quaternion { eulerAngles = new Vector3(0, 0, bowAngle) });

            
            
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
            Projectile.Instantiate(_arrowPrefab, bowPosition - _positionOffset, direction, _damage, Tag.Enemy);
            
            // reduce arrows quantity
            if (--Arrows.Quantity <= 0) Arrows = null;
        }
        
    }
}
