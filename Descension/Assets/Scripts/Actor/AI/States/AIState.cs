using System;
using System.Collections.Generic;
using System.Linq;
using Actor.Player;
using Animation;
using Managers;
using UnityEngine;
using UnityEngine.AI;
using Util.Enums;
using Util.Helpers;

namespace Actor.AI.States
{
    public abstract class AIState : MonoBehaviour
    {
        [Header("Weapon")]
        public float weaponSpriteOffset = 0.3f;
        public float weaponRotationOffset = -45;
        public Vector2 weaponCenterOffset;
        
        [Header("Sight")]
        public UnityLayer traceLayers = UnityLayer.Boulder | UnityLayer.Walls | UnityLayer.Player;
        
        protected Vector3 PlayerPosition => PlayerController.Position;
        private AIController Controller => _controller ??= GetComponent<AIController>();
        protected Animator Animator { get; private set; }

        protected AnimationListener AnimationListener => _animationListener ??= GetComponentInChildren<AnimationListener>();
        protected Vector3 Position => Controller.Transform.position;
        protected Vector3 Velocity { get => Agent.velocity; set => Agent.velocity = value; }
        protected float Speed { get => Controller.Agent.speed; set => Controller.Agent.speed = value; }
        protected Transform[] PatrolTargets => _patrolTargets ??= transform.GetChildTransform("PatrolTargets").GetChildTransforms().ToArray();
        protected Transform WeaponTransform;
        protected NavMeshAgent Agent;
        protected Collider2D Collider;
        
        private AIController _controller;
        private AnimationListener _animationListener;
        private Transform[] _patrolTargets;

        public void Awake()
        {
            enabled = false;
            
            WeaponTransform = Controller.WeaponTransform;
            Agent = Controller.Agent;
            Animator = Controller.Animator;
            Collider = Controller.Collider;
        }

        protected void SetDestination(Vector3 destination) => Controller.Agent.SetDestination(destination);

        protected void ChangeState(AIState newState) => _controller.SetState(newState);

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
