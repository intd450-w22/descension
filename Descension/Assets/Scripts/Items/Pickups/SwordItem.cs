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
        public int updateInterval = 3;

        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) 
            => new Sword(damage, knockBack, reticleDistance, spriteOffset, spriteRotationOffset, updateInterval, slotIndex, quantity, maxQuantity, inventorySprite);
    }
    
    
    
    // logic for sword
    [Serializable]
    class Sword : Equippable
    {
        private float _damage;
        private float _knockBack;
        private float _reticleDistance;
        private float _spriteOffset;
        private float _spriteRotationOffset;
        private bool _execute;
        private float _angle;
        private int _swing;
        private bool _swinging;
        private int _updateCount;
        private int _updateInterval;
        private Transform _playerTransform;
        private Camera _camera;
        private PlayerControls _playerControls;

        public Sword(float damage, float knockBack, float reticleDistance, float spriteOffset, float spriteRotationOffset, int updateInterval, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = SwordItem.Name;

            _damage = damage;
            _knockBack = knockBack;
            _reticleDistance = reticleDistance;
            _spriteOffset = spriteOffset;
            _spriteRotationOffset = spriteRotationOffset;
            _updateInterval = updateInterval;
            
            _playerTransform = PlayerController.Instance.transform;
            _camera = PlayerController.Camera;
            
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite) 
            => new Sword(_damage, _knockBack, _reticleDistance, _spriteOffset, _spriteRotationOffset, _updateInterval, slotIndex, quantity, maxQuantity, sprite);

        public override String GetName() => name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.SwordPrefab, PlayerPosition, Quantity);

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
            if (_updateCount++ % _updateInterval != 0) return;

            Vector3 screenPoint = _camera.WorldToScreenPoint(_playerTransform.localPosition);
            Vector3 direction = (Input.mousePosition - screenPoint).normalized;
            Vector3 position = _playerTransform.position;
            
            _angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            float absAngle = Math.Abs(_angle);
            if (absAngle >= 90 && _swing > -45) _swing -= 15; 
            else if (absAngle < 90 && _swing < 45) _swing += 15;

            Reticle.position = position + (direction * _reticleDistance);
            SpriteTransform.SetPositionAndRotation(position + direction * _spriteOffset, new Quaternion { eulerAngles = new Vector3(0, 0, _angle - _spriteRotationOffset + _swing) });
            Debug.DrawLine(position, Reticle.position);

            if (_swinging && _swing == 0) CheckHit();
            
            //******** Try to Execute if key pressed *******//
            if (!_execute) return;
            _execute = false;
            _swing = Math.Abs(_angle) >= 90 ? 45 : -45;
            _swinging = true;
            
            SoundManager.Swing();
        }

        void CheckHit()
        {
            _swinging = false;
            
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(Reticle.position, new Vector2(2, 2), _angle, (int) UnityLayer.Enemy);
            foreach (Collider2D hit in hitEnemies)
            {
                IDamageable damageable = hit.gameObject.GetComponent<IDamageable>();
                if (damageable == null) damageable = hit.gameObject.GetComponentInParent<IDamageable>();
                
                damageable.InflictDamage(PlayerController.Instance.gameObject, _damage, _knockBack);
            }

            if (hitEnemies.Length >= 1) --Quantity;
        }
        
    }
}
