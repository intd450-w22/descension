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
        public float spriteOffset = 2;
        public float spriteRotationOffset = 45;

        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) 
            => new Pick(lootChance, reticleDistance, spriteOffset, spriteRotationOffset, slotIndex, quantity, maxQuantity, inventorySprite);
    }
    
    
    
    // logic for pick item
    [Serializable]
    class Pick : Equippable
    {
        private float _lootChance;
        private float _reticleDistance;
        private float _spriteOffset;
        private float _spriteRotationOffset;
        private bool _execute;
        private int _swing = 45;
        private bool _swinging;
        private Vector3 _position;
        private Vector3 _direction;
        private PlayerControls _playerControls;
        
        public Pick(float lootChance, float reticleDistance, float spriteOffset, float spriteRotationOffset, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = PickItem.Name;
            _lootChance = lootChance;
            _reticleDistance = reticleDistance;
            _spriteOffset = spriteOffset;
            _spriteRotationOffset = spriteRotationOffset;
            
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite)
            => new Pick(_lootChance, _reticleDistance, _spriteOffset, _spriteRotationOffset, slotIndex, quantity, maxQuantity, sprite);

        public override String GetName() => name;
        
        public override void OnEquip()
        {
            Reticle.gameObject.SetActive(true);
            PlayerController.ItemSprite = inventorySprite;
            SpriteTransform.gameObject.SetActive(true);
        }

        public override void OnUnEquip()
        {
            Reticle.gameObject.SetActive(false);
            SpriteTransform.gameObject.SetActive(false);
        }

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.PickPrefab, PlayerPosition, Quantity);

        public override void Update() => _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();

        public override void FixedUpdate()
        {
            Vector3 screenPoint = PlayerController.Camera.WorldToScreenPoint(PlayerController.Instance.transform.localPosition);
            _direction = (Input.mousePosition - screenPoint).normalized;
            _position = PlayerPosition;
            
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            float absAngle = Math.Abs(angle); 
            
            if (absAngle >= 90 && _swing > -45) _swing -= 15; 
            else if (absAngle < 90 && _swing < 45) _swing += 15; 
            
            float pickAngle = angle - _spriteRotationOffset + _swing;
            
            Reticle.position = _position + (_direction * _reticleDistance);
            SpriteTransform.SetPositionAndRotation(_position + _direction * _spriteOffset, new Quaternion { eulerAngles = new Vector3(0, 0, pickAngle) });
            
            Debug.DrawLine(_position, Reticle.position);

            if (_swinging && _swing == 0) CheckHit();

            if (!_execute) return;
            _execute = false;
            _swing = absAngle >= 90 ? 45 : -45;
            _swinging = true;

            SoundManager.Swing();

            // Debug.DrawLine(_position, _position + _direction * 3);
            //
            // RaycastHit2D rayCast = Physics2D.Raycast(_position, _direction, 3, (int) UnityLayer.Boulder);
            // if (rayCast)
            // {
            //     SoundManager.RemoveRock();
            //     
            //     if (Random.Range(0f, 100f) < _lootChance)
            //     {
            //         int gold = Random.Range(1, 21);
            //         
            //         SoundManager.GoldFound();
            //         
            //         InventoryManager.Gold += gold;
            //         
            //         UIManager.GetHudController()
            //             .ShowFloatingText(rayCast.transform.position, "Gold +" + gold, Color.yellow);
            //     }
            //     
            //     Object.Destroy(rayCast.transform.gameObject);
            //     --Quantity;
            // }
            // else
            // {
            //     Debug.Log("Raycast Miss");
            // }
        }

        void CheckHit()
        {
            _swinging = false;
            
            Debug.DrawLine(_position, _position + _direction * 3);
            
            RaycastHit2D rayCast = Physics2D.Raycast(_position, _direction, 3, (int) UnityLayer.Boulder);
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
