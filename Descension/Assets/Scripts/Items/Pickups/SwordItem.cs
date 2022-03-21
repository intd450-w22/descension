using System;
using Actor.AI;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;
using Util.Helpers;


namespace Items.Pickups
{
    public class SwordItem : EquippableItem
    {
        public static String Name = "Sword";
        public float damage = 10f;
        public float swordReticleDistance = 2f;
        
        public override string GetName()
        {
            return Name;
        }

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity)
        {
            return new Sword(damage, swordReticleDistance, slotIndex, quantity, maxQuantity, inventorySprite);
        }
    }
    
    
    
    // logic for sword
    [Serializable]
    class Sword : Equippable
    {
        private Transform _reticle;
        private float _swordDamage;
        private float _swordReticleDistance;
        private bool _execute;
        private PlayerControls _playerControls;
        
        // gets the reticle object
        private Transform Reticle
        {
            get
            {
                if (_reticle == null)
                {
                    _reticle = GameManager.PlayerController.gameObject.GetChildTransformWithName("Reticle");
                }
                return _reticle;
            }
        }
        
        public Sword(float swordDamage, float swordReticleDistance, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = GetName();

            _swordDamage = swordDamage;
            _swordReticleDistance = swordReticleDistance;
            
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override String GetName()
        {
            return SwordItem.Name;
        }
        
        public override void SpawnDrop()
        {
            ItemSpawner.Instance.DropItem(ItemSpawner.Instance.swordPickupPrefab, Quantity);
        }

        public override void OnEquip()
        {
            Reticle.gameObject.SetActive(true);
        }

        public override void OnUnEquip()
        {
            Reticle.gameObject.SetActive(false);
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
            
            Reticle.position = position + (direction * _swordReticleDistance);
            
            Debug.DrawLine(position, position + direction * _swordReticleDistance);

            //******** Try to Execute if key pressed *******//
            if (!_execute) return;
            _execute = false;
            
            if (Quantity <= 0)
            {
                UIManager.Instance.GetHudController().ShowText("Sword has no durability!");
                return;
            }
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Vector2 attackPoint = position + (direction * _swordReticleDistance);
            
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint, new Vector2(2, 2), angle, (int) UnityLayer.Enemy);
            foreach (Collider2D hit in hitEnemies)
            {
                AIController enemyController = hit.gameObject.GetComponent<AIController>();
                if (enemyController == null) enemyController = hit.gameObject.GetComponentInParent<AIController>();
                
                enemyController.InflictDamage(_swordDamage);
            }

            if (hitEnemies.Length >= 1) --Quantity;
        }
        
    }
}
