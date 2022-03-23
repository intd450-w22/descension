using UnityEngine;

namespace Actor.AI.States
{
    public abstract class AIState : MonoBehaviour
    {
        protected AIController Controller;

        public void Start()
        {
            Controller = GetComponent<AIController>();
        }
        public abstract void UpdateState();

        public abstract void Initialize();
    }
}
