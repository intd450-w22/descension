using System;
using Actor.Player;
using Environment;
using Managers;
using UnityEngine;
using Util.Enums;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace Items.Pickups
{
    public class PickItem : EquippableItem
    {
        const String Name = "Pick";
        public float lootChance = 20;
        
        public override string GetName()
        {
            return Name;
        }

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance()
        {
            return new Pick(lootChance);
        }
    }
    
    
    
    // logic for pick item
    [Serializable]
    class Pick : Equippable
    {
        private float _lootChance;
        private bool _execute;
        private PlayerControls _playerControls;
        
        
        public Pick(float lootChance)
        {
            this.name = GetName();
            _lootChance = lootChance;
            
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }

        public String GetName()
        {
            return "Pick";
        }
        
        public override void OnDrop()
        {
            ItemSpawner.Instance.DropItem(ItemSpawner.Instance.PickPickupPrefab, durability);
            base.OnDrop();
        }

        public override void Update()
        {
            _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();
        }

        public override void FixedUpdate()
        {
            if (!_execute) return;
            _execute = false;
            
            if (durability <= 0)
            {
                UIManager.Instance.GetHudController().ShowText("No picks!");
                return;
            }
            
            PlayerController controller = GameManager.PlayerController;
            
            var screenPoint = controller.playerCamera.WorldToScreenPoint(controller.transform.localPosition);
            var direction = (Input.mousePosition - screenPoint).normalized;
            
            Vector3 playerPosition = controller.transform.position;
            
            Debug.DrawLine(playerPosition, playerPosition + direction * 3);
            
            int mask = (int) UnityLayer.Boulder;
            RaycastHit2D rayCast = Physics2D.Raycast(playerPosition, direction, 3, mask);
            if (rayCast)
            {
                SoundManager.Instance.RemoveRock();
                
                if (Random.Range(0f, 100f) < _lootChance)
                {
                    float gold = Mathf.Floor(Random.Range(0f, 20f));
                    
                    SoundManager.Instance.GoldFound();
                    
                    InventoryManager.Instance.gold += Mathf.Floor(Random.Range(0f, 20f));
                    
                    UIManager.Instance.GetHudController()
                        .ShowFloatingText(rayCast.transform.position, "Gold +" + gold, Color.yellow);
                }
                
                Object.Destroy(rayCast.transform.gameObject);

                --durability;
            }
            else
            {
                Debug.Log("Raycast Miss");
            }
        }
        
    }
}
