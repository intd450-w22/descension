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
        public override Equippable CreateInstance(int slotIndex, int quantity) => new Sword(this, slotIndex, quantity);
    }
    
    
    // logic for sword
    [Serializable]
    internal class Sword : Equippable
    {
        // attributes
        private float _damage;
        private float _knockBack;
        private float _reticleDistance;
        private float _spriteOffset;
        private float _spriteRotationOffset;
        
        // state
        private bool _swinging;
        private int _swingAngle;
        private int _swingHitAngle;
        private float _aimAngle;
        private Vector3 _aimDirection;
        private Transform _playerTransform;
        private Camera _camera;

        public Sword(SwordItem swordItem, int slotIndex, int quantity) : base(swordItem, slotIndex, quantity) 
            => Init(swordItem.damage, swordItem.knockBack, swordItem.reticleDistance, swordItem.spriteOffset, swordItem.spriteRotationOffset);

        public Sword(Sword sword) : base(sword) 
            => Init(sword._damage, sword._knockBack, sword._reticleDistance, sword._spriteOffset, sword._spriteRotationOffset);

        private void Init(float damage, float knockBack, float reticleDistance, float spriteOffset, float spriteRotationOffset)
        {
            name = SwordItem.Name;
            _damage = damage;
            _knockBack = knockBack;
            _reticleDistance = reticleDistance;
            _spriteOffset = spriteOffset;
            _spriteRotationOffset = spriteRotationOffset;
            _playerTransform = PlayerController.Instance.transform;
            _camera = PlayerController.Camera;
        }
        
        public override Equippable DeepCopy() => new Sword(this);

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
        
        protected override void FixedUpdate()
        {
            Vector3 screenPoint = _camera.WorldToScreenPoint(_playerTransform.localPosition);
            _aimDirection = (Input.mousePosition - screenPoint).normalized;
            Vector3 position = _playerTransform.position;
            
            _aimAngle = _aimDirection.ToDegrees();
            
            Reticle.position = position + (_aimDirection * _reticleDistance);
            SpriteTransform.SetPositionAndRotation(position + _aimDirection * _spriteOffset, new Quaternion { eulerAngles = new Vector3(0, 0, _aimAngle - _spriteRotationOffset + _swingAngle) });

            if (_swinging && _swingAngle == _swingHitAngle) CheckHit();
            
            float absAngle = Math.Abs(_aimAngle);
            if (absAngle >= 90 && _swingAngle > -45) _swingAngle -= 30;
            else if (absAngle < 90 && _swingAngle < 45) _swingAngle += 30;
        }

        protected override void Execute()
        {
            _swinging = true;
            
            if (Math.Abs(_aimAngle) >= 90)
            {
                _swingAngle = 45;
                _swingHitAngle = 15;
            }
            else
            {
                _swingAngle = -45;
                _swingHitAngle = -15;
            }

            SoundManager.Swing();
        }

        void CheckHit()
        {
            _swinging = false;
            
            DebugHelper.DrawBoxCast2D(PlayerPosition, new Vector2(1, 15), _aimAngle, _aimDirection, _reticleDistance, 0.5f, Color.blue);
            
            RaycastHit2D[] hitEnemies;
            foreach (RaycastHit2D hit in hitEnemies = Physics2D.BoxCastAll(PlayerPosition, new Vector2(1, 15), _aimAngle, _aimDirection, _reticleDistance, (int) UnityLayer.Enemy))
            {
                IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable == null) damageable = hit.collider.gameObject.GetComponentInParent<IDamageable>();
                
                damageable.InflictDamage(_damage, _aimDirection, _knockBack);
            }

            if (hitEnemies.Length >= 1) --Quantity;
        }
        
    }
}
