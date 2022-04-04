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
            => new Sword(damage, knockBack, reticleDistance, spriteOffset, spriteRotationOffset, slotIndex, quantity, maxQuantity, inventorySprite);
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
        private Vector3 _direction;
        private int _swing;
        private bool _swinging;
        private int _swingHit;
        private Transform _playerTransform;
        private Camera _camera;
        private PlayerControls _playerControls;

        public Sword(float damage, float knockBack, float reticleDistance, float spriteOffset, float spriteRotationOffset, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = SwordItem.Name;

            _damage = damage;
            _knockBack = knockBack;
            _reticleDistance = reticleDistance;
            _spriteOffset = spriteOffset;
            _spriteRotationOffset = spriteRotationOffset;
            _playerTransform = PlayerController.Instance.transform;
            _camera = PlayerController.Camera;
            
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite) 
            => new Sword(_damage, _knockBack, _reticleDistance, _spriteOffset, _spriteRotationOffset, slotIndex, quantity, maxQuantity, sprite);

        public override String GetName() => name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.SwordPrefab, PlayerPosition, Quantity);

        public override void OnEquip()
        {
            if (Reticle.localPosition.magnitude < 0.5f)
            {
                Reticle.position = PlayerPosition + new Vector3(_reticleDistance,0,0);
                SpriteTransform.SetPositionAndRotation(PlayerPosition + new Vector3(_spriteOffset,0,0), new Quaternion { eulerAngles = new Vector3(0, 0, 0) });
            }

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
            Vector3 screenPoint = _camera.WorldToScreenPoint(_playerTransform.localPosition);
            _direction = (Input.mousePosition - screenPoint).normalized;
            Vector3 position = _playerTransform.position;
            
            _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            
            Reticle.position = position + (_direction * _reticleDistance);
            SpriteTransform.SetPositionAndRotation(position + _direction * _spriteOffset, new Quaternion { eulerAngles = new Vector3(0, 0, _angle - _spriteRotationOffset + _swing) });

            if (_swinging && _swing == _swingHit) CheckHit();
            
            float absAngle = Math.Abs(_angle);
            if (absAngle >= 90 && _swing > -45) _swing -= 30;
            else if (absAngle < 90 && _swing < 45) _swing += 30;
            
            //******** Try to Execute if key pressed *******//
            if (!_execute) return;
            _execute = false;
            _swinging = true;
            
            if (absAngle >= 90)
            {
                _swing = 45;
                _swingHit = 15;
            }
            else
            {
                _swing = -45;
                _swingHit = -15;
            }

            SoundManager.Swing();
        }

        void CheckHit()
        {
            _swinging = false;
            
            DebugHelper.DrawBoxCast2D(PlayerPosition, new Vector2(1, 15), _angle, _direction, _reticleDistance, 0.5f, Color.blue);
            
            RaycastHit2D[] hitEnemies;
            foreach (RaycastHit2D hit in hitEnemies = Physics2D.BoxCastAll(PlayerPosition, new Vector2(1, 15), _angle, _direction, _reticleDistance, (int) UnityLayer.Enemy))
            {
                IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable == null) damageable = hit.collider.gameObject.GetComponentInParent<IDamageable>();
                
                damageable.InflictDamage(PlayerController.Instance.gameObject, _damage, _knockBack);
            }

            if (hitEnemies.Length >= 1) --Quantity;
        }
        
    }
}
