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
        public float range = 8;
        public float collisionWidth = 15;
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
        private float _range;
        private float _collisionWidth;
        private float _spriteOffset;
        private float _spriteRotationOffset;
        
        // state
        private bool _swinging;
        private int _swingAngle;
        private int _swingHitAngle;
        private float _aimAngle;
        private Vector2 _collisionBox;
        private Vector3 _aimDirection;
        private Transform _playerTransform;
        private Camera _camera;

        public Sword(SwordItem swordItem, int slotIndex, int quantity) : base(swordItem, slotIndex, quantity) 
            => Init(swordItem.damage, swordItem.knockBack, swordItem.range, swordItem.collisionWidth, swordItem.spriteOffset, swordItem.spriteRotationOffset);

        public Sword(Sword sword) : base(sword) 
            => Init(sword._damage, sword._knockBack, sword._range + _collisionBox.x/2f, sword._collisionWidth, sword._spriteOffset, sword._spriteRotationOffset);

        private void Init(float damage, float knockBack, float range, float collisionWidth, float spriteOffset, float spriteRotationOffset)
        {
            name = SwordItem.Name;
            _damage = damage;
            _knockBack = knockBack;
            _collisionWidth = collisionWidth;
            _collisionBox = new Vector2(1, collisionWidth);
            _range = range - _collisionBox.x/2f;
            _spriteOffset = spriteOffset;
            _spriteRotationOffset = spriteRotationOffset;
            _playerTransform = PlayerController.Instance.transform;
            _camera = PlayerController.Camera;
        }
        
        public override Equippable DeepCopy() => new Sword(this);

        public override string GetName() => name;

        public override void SpawnDrop() => SpawnManager.SpawnItem(SpawnManager.SwordPrefab, PlayerPosition, Quantity);

        public override void OnEquip()
        {
            if (Reticle.localPosition.magnitude < 0.5f)
            {
                Reticle.position = PlayerPosition + new Vector3(_range,0,3f);
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
            var screenPoint = _camera.WorldToScreenPoint(_playerTransform.localPosition);
            _aimDirection = (Input.mousePosition - screenPoint).normalized;
            var position = _playerTransform.position;
            
            _aimAngle = _aimDirection.ToDegrees();
            
            var reticlePos = position + (_aimDirection * _range);;
            Reticle.position = new Vector3(reticlePos.x, reticlePos.y, 3f);
            SpriteTransform.SetPositionAndRotation(position + _aimDirection * _spriteOffset, new Quaternion { eulerAngles = new Vector3(0, 0, _aimAngle - _spriteRotationOffset + _swingAngle) });

            if (_swinging && _swingAngle == _swingHitAngle) CheckHit();
            
            var absAngle = Math.Abs(_aimAngle);
            if (absAngle >= 90 && _swingAngle > -45) _swingAngle -= 30;
            else if (absAngle < 90 && _swingAngle < 45) _swingAngle += 30;
        }

        public override void Execute()
        {
            base.Execute();

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
            
            GameDebug.DrawBoxCast2D(PlayerPosition, _collisionBox, _aimAngle, _aimDirection, _range, 0.5f, Color.blue);
            
            RaycastHit2D[] hitEnemies;
            foreach (var hit in hitEnemies = Physics2D.BoxCastAll(PlayerPosition, _collisionBox, _aimAngle, _aimDirection, _range, (int) UnityLayer.Enemy))
            {
                hit.collider.gameObject
                    .GetComponent<IDamageable>(true)
                    .InflictDamage(_damage, _aimDirection, _knockBack);
            }

            if (hitEnemies.Length >= 1) --Quantity;
        }
        
    }
}
