using Managers;
using UnityEngine;

namespace Environment
{
    public class OnLevelStart : MonoBehaviour
    {
        void Start()
        {
            // Init any level things
            SoundManager.StartBackgroundAudio();
            UIManager.GetHudController().Reset(); // Move to an earlier call probably 

            GameManager.UnFreeze();
            GameManager.Resume();
        }
    }
}
