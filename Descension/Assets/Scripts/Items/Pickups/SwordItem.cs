using System;
using Actor.Interface;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;
using Util.Helpers;


namespace Items.Pickups
{
    public class SwordItem : EquippableItem
    {
        public static string Name = "Sword";

        [Header("Sword")]
        public float damage = 10;
        public float spriteOffset = 2;
        public float spriteRotationOffset = 45;
        public float reticleDistance = 4;
        public float knockBack = 0;

        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity)
        {
            return new Sword(damage, knockBack, reticleDistance, spriteOffset, spriteRotationOffset, slotIndex, quantity, maxQuantity, inventorySprite);
        }
    }
    
    
    
    // logic for sword
    [Serializable]
    class Sword : Equippable
    {
        private Transform _reticle;
        private Transform _itemObject;
        private float _damage;
        private float _knockBack;
        private float _reticleDistance;
        private float _spriteOffset;
        private float _spriteRotationOffset;
        private bool _execute;
        private int _swing;
        private PlayerControls _playerControls;
        
        // gets the reticle object
        private Transform Reticle => _reticle ? _reticle : _reticle = PlayerController.Reticle;
        private Transform ItemObject => _itemObject ? _itemObject : _itemObject = PlayerController.ItemObject;
        

        public Sword(float damage, float knockBack, float reticleDistance, float spriteOffset, float spriteRotationOffset, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = SwordItem.Name;

            _damage = damage;
            _knockBack = knockBack;
            _reticleDistance = reticleDistance;
            _spriteOffset = spriteOffset;
            _spriteRotationOffset = spriteRotationOffset;
            
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite) 
            => new Sword(_damage, _knockBack, _reticleDistance, _spriteOffset, _spriteRotationOffset, slotIndex, quantity, maxQuantity, sprite);

        public override String GetName() => name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.SwordPrefab, GameManager.PlayerController.transform.position, Quantity);

        public override void OnEquip()
        {
            Reticle.gameObject.SetActive(true);
            PlayerController.ItemSprite = inventorySprite;
            ItemObject.gameObject.SetActive(true);
        }

        public override void OnUnEquip()
        {
            Reticle.gameObject.SetActive(false);
            ItemObject.gameObject.SetActive(false);
        }

        public override void Update()
        {
            _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();
            
        }

        public override void FixedUpdate()
        {
            PlayerController controller = GameManager.PlayerController;
            Vector3 screenPoint = controller.playerCamera.WorldToScreenPoint(controller.transform.localPosition);
            Vector3 direction = (Input.mousePosition - screenPoint).normalized;
            Vector3 position = controller.transform.position;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float absAngle = Math.Abs(angle);
            
            if (absAngle >= 90 && _swing > -45) _swing -= 15; 
            else if (absAngle < 90 && _swing < 45) _swing += 15; 
            
            
            float swordAngle = angle - _spriteRotationOffset + _swing;
            Debug.Log(angle);
            // Vector3 swordDirection = new Vector3(0, 1, 0).GetRotated(swordAngle);
            
            Reticle.position = position + (direction * _reticleDistance);
            ItemObject.SetPositionAndRotation(position + direction * _spriteOffset, new Quaternion { eulerAngles = new Vector3(0, 0, swordAngle) });
            Debug.DrawLine(position, Reticle.position);

            //******** Try to Execute if key pressed *******//
            if (!_execute) return;
            {
                _execute = false;
                _swing = absAngle >= 90 ? 45 : -45;
            }
            
            
            if (Quantity <= 0)
            {
                UIManager.GetHudController().ShowText("Sword has no durability!");
                return;
            }
            
            
            
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(Reticle.position, new Vector2(2, 2), angle, (int) UnityLayer.Enemy);
            foreach (Collider2D hit in hitEnemies)
            {
                IDamageable damageable = hit.gameObject.GetComponent<IDamageable>();
                if (damageable == null) damageable = hit.gameObject.GetComponentInParent<IDamageable>();
                
                damageable.InflictDamage(GameManager.PlayerController.gameObject, _damage, _knockBack);
            }

            if (hitEnemies.Length >= 1) --Quantity;
        }
        
    }
}
