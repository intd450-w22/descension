using System;
using System.Collections.Generic;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Helpers;

namespace Actor.AI.States
{
    public abstract class AIState : MonoBehaviour
    {
        [Header("Weapon")]
        public float weaponSpriteOffset = 0.3f;
        public float weaponRotationOffset = -45;
        public Vector2 weaponCenterOffset;
        
        private AIController _controller;
        private AIController Controller => _controller ??= GetComponent<AIController>();
        
        protected Transform WeaponTransform;
        protected Vector3 PlayerPosition => PlayerController.Position;
        protected Vector3 Position => Controller.agent.transform.position;
        protected Vector3 Velocity { get => Controller.agent.velocity; set => Controller.agent.velocity = value; }
        protected float Speed { set => Controller.agent.speed = value; }

        private List<Transform> _patrolTargets;
        protected List<Transform> PatrolTargets
        {
            get
            {
                if (_patrolTargets == null)
                {
                    _patrolTargets = new List<Transform>(transform.Find("PatrolTargets").GetComponentsInChildren<Transform>());
                    _patrolTargets.Remove(transform.Find("PatrolTargets"));
                }
                return _patrolTargets;
            }
        }

        public void Awake()
        {
            enabled = false;
            WeaponTransform = Controller.WeaponTransform;
        }

        protected void SetDestination(Vector3 destination)
        {
            Controller.agent.SetDestination(destination);
        }

        protected void ChangeState(AIState newState)
        {
            _controller.SetState(newState);
        }

        // called every frame 
        public abstract void UpdateState();
        
        // called when changing to this state
        public abstract void StartState();
        
        // called when changing to a different state
        public abstract void EndState();
        
        protected void UpdateWeaponTransform(Vector3 target, float weaponSwingAngle = 0)
        {
            var moveDirection = (target - Position).normalized;
            var angle = moveDirection.ToDegrees();

            int sign;
            if (Math.Abs(angle) >= 90)
            {
                sign = 1;
                angle += 180;
            }
            else sign = -1;
            
            var localScale = WeaponTransform.localScale;
            WeaponTransform.localScale = new Vector3(-sign*Math.Abs(localScale.x),localScale.y,localScale.z);
            WeaponTransform.localPosition = moveDirection.GetRotated(sign*weaponSwingAngle) * weaponSpriteOffset + weaponCenterOffset;
            WeaponTransform.localRotation = new Quaternion { eulerAngles = new Vector3(0, 0, angle - sign*weaponRotationOffset + sign*weaponSwingAngle) };
        }
    }
}
