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
        public float knockBack = 100;
        public float spriteOffset = 2;
        public float spriteRotationOffset = 45;
        public Vector2 spritePositionOffset;
        public float bowReticleDistance = 2f;
        
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) => new Bow(this, slotIndex, quantity);
    }
    
    
    
    // logic for bow
    [Serializable]
    internal class Bow : Equippable
    {
        // attributes
        private GameObject _arrowPrefab;
        private float _damage;
        private float _knockBack;
        private float _reticleDistance;
        private float _spriteDistanceOffset;
        private float _spriteRotationOffset;
        private Vector3 _spritePositionOffset;
        private String _currentControlScheme = ControlScheme.Desktop.ToString();

        // state
        private Vector3 _bowPosition;
        private Vector3 _direction;
        
        // gets the arrow quiver if we have one
        private Arrows _arrows;
        private Arrows Arrows
        {
            get => _arrows ??= (Arrows) InventoryManager.Slots.Find(slot => slot.name == "Arrows");
            set => _arrows = value;
        }
        
        public Bow(BowItem bowItem, int slotIndex, int quantity) : base(bowItem, slotIndex, quantity) 
            => Init(bowItem.arrowPrefab, bowItem.damage, bowItem.knockBack, bowItem.bowReticleDistance, bowItem.spriteOffset, bowItem.spriteRotationOffset, bowItem.spritePositionOffset);

        public Bow(Bow bow) : base(bow) 
            => Init(bow._arrowPrefab, bow._damage, bow._knockBack, bow._reticleDistance, bow._spriteDistanceOffset, bow._spriteRotationOffset, bow._spritePositionOffset);

        public void Init(GameObject arrowPrefab, float damage, float knockBack, float bowReticleDistance, float spriteOffset, float spriteRotationOffset, Vector2 positionOffset)
        {
            name = BowItem.Name;
            _arrowPrefab = arrowPrefab;
            _damage = damage;
            _knockBack = knockBack;
            _reticleDistance = bowReticleDistance;
            _spriteDistanceOffset = spriteOffset;
            _spriteRotationOffset = spriteRotationOffset;
            _spritePositionOffset = positionOffset;
        }
        
        public override Equippable DeepCopy() => new Bow(this);

        public override String GetName() => name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.BowPrefab, PlayerPosition, Quantity);

        public override void OnEquip()
        {
            Reticle.position = (Vector2) PlayerController.Camera.ScreenToWorldPoint(Input.mousePosition);
            var screenPoint = PlayerController.Camera.WorldToScreenPoint(PlayerController.Instance.transform.localPosition);
            _direction = (Input.mousePosition - screenPoint).normalized;
            
            float bowAngle = _direction.ToDegrees() + _spriteRotationOffset;
            _bowPosition = PlayerPosition + _spritePositionOffset + _direction * _spriteDistanceOffset;
            SpriteTransform.SetPositionAndRotation(_bowPosition, new Quaternion { eulerAngles = new Vector3(0, 0, bowAngle) });
            
            Reticle.gameObject.SetActive(true);
            Sprite = inventorySprite;
            SpriteTransform.gameObject.SetActive(true);
        }

        public override void OnUnEquip()
        {
            Reticle.gameObject.SetActive(false);
            SpriteTransform.gameObject.SetActive(false);
        }


        protected override void FixedUpdate()
        {
            var screenPoint = PlayerController.Camera.WorldToScreenPoint(PlayerController.Instance.transform.localPosition);
            _direction = (Input.mousePosition - screenPoint).normalized;
            
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
                Reticle.position = PlayerPosition + (_direction * _reticleDistance);
            }
            
            float bowAngle = _direction.ToDegrees() + _spriteRotationOffset;
            _bowPosition = PlayerPosition + _spritePositionOffset + _direction * _spriteDistanceOffset;
            SpriteTransform.SetPositionAndRotation(_bowPosition, new Quaternion { eulerAngles = new Vector3(0, 0, bowAngle) });
        }

        protected override void Execute()
        {
            if (Arrows == null)
            {
                UIManager.GetHudController().ShowText("No arrows to shoot!");
                return;
            }

            Vector3 playerPosition = PlayerPosition;
            
            Debug.DrawLine(playerPosition, playerPosition + _direction * 3);
            
            // spawn arrow
            Projectile.Instantiate(_arrowPrefab, _bowPosition - _spritePositionOffset, _direction, _damage, _knockBack, Tag.Enemy);
            
            // reduce arrows quantity
            if (--Arrows.Quantity <= 0) Arrows = null;
        }
    }
}
