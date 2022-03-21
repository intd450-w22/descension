using System;
using Actor.Objects;
using Managers;
using UnityEngine;
using Util.Enums;
using Util.Helpers;
using Object = UnityEngine.Object;


namespace Items.Pickups
{
    public class BowItem : EquippableItem
    {
        public static String Name = "Bow";

        public GameObject arrowPrefab;
        public float bowReticleDistance = 2f;
        
        public override string GetName()
        {
            return Name;
        }

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance()
        {
            return new Bow(arrowPrefab, bowReticleDistance);
        }
    }
    
    
    
    // logic for bow
    [Serializable]
    class Bow : Equippable
    {
        private Arrows _arrows;
        private Transform _reticle;
        private GameObject _arrowPrefab;
        private String _currentControlScheme = ControlScheme.Desktop.ToString();
        private float _bowReticleDistance;
        private bool _execute;
        private PlayerControls _playerControls;

        // gets the arrow quiver if we have one
        private Arrows Arrows
        {
            get
            {
                if (_arrows == null)
                {
                    _arrows = (Arrows) InventoryManager.Instance.slots.Find(slot => slot.GetName() == "Arrows");
                }
                return _arrows;
            }
        }
        
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
        
        public Bow(GameObject arrowPrefab, float bowReticleDistance)
        {
            name = GetName();
            
            _arrowPrefab = arrowPrefab;
            _bowReticleDistance = bowReticleDistance;
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public String GetName()
        {
            return BowItem.Name;
        }

        public override void OnEquip()
        {
            Reticle.gameObject.SetActive(true);
        }

        public override void OnUnEquip()
        {
            Reticle.gameObject.SetActive(false);
        }

        public override void OnDrop()
        {
            ItemSpawner.Instance.DropItem(ItemSpawner.Instance.bowPickupPrefab, quantity);
            base.OnDrop();
        }

        public override void Update()
        {
            _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();
        }

        public override void FixedUpdate()
        {
            var screenPoint = GameManager.PlayerController.playerCamera.WorldToScreenPoint(GameManager.PlayerController.transform.localPosition);
            // var screenPoint = _controller.playerCamera.WorldToScreenPoint(_transform.localPosition);
            var direction = (Input.mousePosition - screenPoint).normalized;
            
            // Set the position of the reticle on the screen according to input type
            if (_currentControlScheme == ControlScheme.Desktop.ToString())
            {
                // Place the reticle on the cursor 
                // TODO: Hide the cursor ? 
                Reticle.position = (Vector2) GameManager.PlayerController.playerCamera.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (_currentControlScheme == ControlScheme.Gamepad.ToString())
            {
                // Place the reticle in a ring around the player 
                // TODO: Add aiming with the right stick ala Enter the Gungeon 
                Reticle.position = GameManager.PlayerController.transform.position + (direction * _bowReticleDistance);
            }
            
            
            //******** Try to Execute if key pressed and have arrows *******//
            if (!_execute) return;
            _execute = false;
            
            if (Arrows == null || Arrows.quantity <= 0)
            {
                UIManager.Instance.GetHudController().ShowText("No arrows to shoot!");
                return;
            }

            if (Arrows.quantity <= 0)
            {
                Debug.Log("No arrows in quiver");
                return;
            }


            Vector3 playerPosition = GameManager.PlayerController.transform.position;
            
            Debug.DrawLine(playerPosition, playerPosition + direction * 3);
            
            // spawn arrow
            SoundManager.ArrowAttack();
            GameObject arrowObject = Object.Instantiate(_arrowPrefab, (Vector3)playerPosition + direction, Quaternion.identity);
            arrowObject.transform.localScale = GameManager.PlayerController.transform.localScale;
            Arrow arrow = arrowObject.GetComponent<Arrow>();
            arrow.Initialize(direction);
            
            // reduce quiver quantity
            _arrows.SetQuantity(_arrows.quantity - 1);
        }
        
    }
}
