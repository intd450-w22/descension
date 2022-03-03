using System;
using Actor.Player;
using Environment;
using Managers;
using UnityEngine;
using Util.Enums;
using Util.Helpers;
using Object = UnityEngine.Object;


namespace Items
{
    public class BowItem : EquippableItem
    {
        const String Name = "Bow";

        public GameObject arrowPrefab;
        public float bowReticleDistance = 2f;
        
        public override string GetName()
        {
            return Name;
        }

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(InventoryManager manager, PlayerController controller, int durability)
        {
            return new Bow(manager, controller, durability, arrowPrefab, bowReticleDistance);
        }
    }
    
    
    
    // logic for bow
    [Serializable]
    class Bow : Equippable
    {
        private PlayerController _controller;
        private Quiver _quiver;
        private Camera _playerCamera;
        private Transform _transform;
        private GameObject _arrowPrefab;
        private Transform _reticle;
        private String _currentControlScheme = ControlScheme.Desktop.ToString();
        private float _bowReticleDistance;
        private bool _execute;
        private PlayerControls _playerControls;
        private InventoryManager _manager;
        
        
        public Bow(InventoryManager manager, PlayerController controller, int durability, GameObject arrowPrefab, float bowReticleDistance)
        {
            _manager = manager;
            _controller = controller;
            this.durability = durability;
            this.name = GetName();
            this._quiver = (Quiver) manager.slots.Find(slot => slot.GetName() == "Quiver");
            
            if (this._quiver != null) Debug.Log("Quiver Found!"); else Debug.Log("Quiver NOT found!");
            

            _reticle = controller.gameObject.GetChildTransformWithName("Reticle");
            if (_reticle != null) _reticle.gameObject.SetActive(false);
            
            _playerCamera = controller.playerCamera;
            _transform = controller.transform;
            _arrowPrefab = arrowPrefab;
            _bowReticleDistance = bowReticleDistance;
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }

        public String GetName()
        {
            return "Bow";
        }

        public override void Update()
        {
            _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();
        }

        public override void FixedUpdate()
        {
            var screenPoint = _controller.playerCamera.WorldToScreenPoint(_transform.localPosition);
            var direction = (Input.mousePosition - screenPoint).normalized;
            
            if (_reticle != null)
            {
                _reticle.gameObject.SetActive(true);
            
                // Set the position of the reticle on the screen according to input type
                if (_currentControlScheme == ControlScheme.Desktop.ToString())
                {
                    // Place the reticle on the cursor 
                    // TODO: Hide the cursor ? 
                    _reticle.position = (Vector2) _playerCamera.ScreenToWorldPoint(Input.mousePosition);
                }
                else if (_currentControlScheme == ControlScheme.Gamepad.ToString())
                {
                    // Place the reticle in a ring around the player 
                    // TODO: Add aiming with the right stick ala Enter the Gungeon 
                    _reticle.position = _transform.position + (direction * _bowReticleDistance);
                }
            }
            
            
            // Debug.Log("Use2A");
            if (!_execute) return;
            _execute = false;
            
            if (_quiver == null || _quiver.durability <= 0)
            {
                UIManager.Instance.GetHudController().ShowText("No arrows to shoot!");
                return;
            }

            if (_quiver.durability <= 0)
            {
                Debug.Log("No arrows in quiver");
                return;
            }


            Vector3 playerPosition = _transform.position;
            
            Debug.DrawLine(playerPosition, playerPosition + direction * 3);
            
            // spawn arrow
            SoundManager.Instance.ArrowAttack();
            GameObject arrowObject = Object.Instantiate(_arrowPrefab, (Vector3)playerPosition + direction, Quaternion.identity);
            arrowObject.transform.localScale = _transform.localScale;
            Arrow arrow = arrowObject.GetComponent<Arrow>();
            arrow.Initialize(direction);
            
            // reduce quiver quantity
            --_quiver.durability;
        }
    }
}
