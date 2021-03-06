using System;
using Actor.Environment;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;
using Util.Helpers;

namespace Actor.Items.Pickups
{
    public class PickItem : EquippableItem
    {
        public static string Name = "Pick";

        [Header("Pick")]
        public float lootChance = 40;
        public float reticleDistance = 2;
        public float spriteOffset = 2;
        public float spriteRotationOffset = 45;
        
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) => new Pick(this, slotIndex, quantity);
    }
    
    
    
    // logic for pick item
    [Serializable]
    internal class Pick : Equippable
    {
        // attributes
        private float _lootChance;
        private float _reticleDistance;
        private float _spriteOffset;
        private float _spriteRotationOffset;
        
        // state
        private bool _swinging;
        private int _swingAngle;
        private int _swingHitAngle;
        private float _aimAngle;
        private Vector3 _aimDirection;
        private Vector3 _playerPosition;

        public Pick(PickItem pickItem, int slotIndex, int quantity) : base(pickItem, slotIndex, quantity) 
            => Init(pickItem.lootChance, pickItem.reticleDistance, pickItem.spriteOffset, pickItem.spriteRotationOffset);
        
        public Pick(Pick pick) : base(pick) 
            => Init(pick._lootChance, pick._reticleDistance, pick._spriteOffset, pick._spriteRotationOffset);
        
        public void Init(float lootChance, float reticleDistance, float spriteOffset, float spriteRotationOffset)
        {
            name = PickItem.Name;
            _lootChance = lootChance;
            _reticleDistance = reticleDistance;
            _spriteOffset = spriteOffset;
            _spriteRotationOffset = spriteRotationOffset;
        }
        
        public override Equippable DeepCopy() => new Pick(this);

        public override string GetName() => name;
        
        public override void OnEquip()
        {
            if (Reticle.localPosition.magnitude < 0.5)
            {
                Reticle.position = PlayerPosition + new Vector3(_reticleDistance,0,3f);
                SpriteTransform.SetPositionAndRotation(PlayerPosition + new Vector3(_spriteOffset,0,0), new Quaternion { eulerAngles = new Vector3(0, 0, 0) });
            }
            Reticle.gameObject.SetActive(true);
            PlayerController.ItemSprite = inventorySprite;
            SpriteTransform.gameObject.SetActive(true);
        }

        public override void OnUnEquip()
        {
            Reticle.gameObject.SetActive(false);
            SpriteTransform.gameObject.SetActive(false);
        }

        public override void SpawnDrop() => SpawnManager.SpawnItem(SpawnManager.PickPrefab, PlayerPosition, Quantity);

        protected override void FixedUpdate()
        {
            var screenPoint = PlayerController.Camera.WorldToScreenPoint(PlayerController.Instance.transform.localPosition);
            _aimDirection = (Input.mousePosition - screenPoint).normalized;
            _playerPosition = PlayerPosition;
            
            _aimAngle = _aimDirection.ToDegrees();
            var reticlePos = _playerPosition + (_aimDirection * _reticleDistance);
            Reticle.position = new Vector3(reticlePos.x, reticlePos.y, 3f);
            SpriteTransform.SetPositionAndRotation(_playerPosition + _aimDirection * _spriteOffset, new Quaternion { eulerAngles = new Vector3(0, 0, _aimAngle - _spriteRotationOffset + _swingAngle) });
            
            GameDebug.DrawLine(_playerPosition, Reticle.position);

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
            
            var rayCast = Physics2D.Raycast(_playerPosition, _aimDirection, _reticleDistance, (int) UnityLayer.Boulder);
            if (rayCast)
            {
                SoundManager.RemoveRock();
                
                rayCast.collider.GetComponent<RemovableRock>().OnDestroyed();
                
                --Quantity;
            }
        }
        
        
    }
}
