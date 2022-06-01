using UnityEngine;

namespace Animation
{
    public class AnimationListener : MonoBehaviour
    {
        public enum AnimationEvent : int
        {
            ExecuteAttack = 1,
            FinishAttack  = 2,
        }
        
        public delegate void OnBroadcast(AnimationEvent id);
        private OnBroadcast _onBroadcast;

        public void SetOnBroadcast(OnBroadcast onBroadcast) => _onBroadcast = onBroadcast;
        
        public void BroadcastEvent(AnimationEvent e)
        {
            _onBroadcast?.Invoke(e);
        }
    }
}
