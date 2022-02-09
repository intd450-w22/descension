using UnityEngine;

namespace Actor.AI
{
    public abstract class AttackBase : MonoBehaviour
    {
        public float range = 2;
        public float cooldown = 1.0f;  // cooldown until next attack
        private float _timer = 0f;
        void Start()
        {
            GetComponent<AIController>().attack = this;
        }

        void Update()
        {
            _timer += Time.deltaTime;
        }
    
        public void Execute()
        {
            if (_timer > cooldown)
            {
                _timer = 0f;
                ExecuteImplementation();
            }
        }

        protected abstract void ExecuteImplementation();
    }
}
