using Items.Pickups;
using Managers;
using UnityEngine;
using Util.Helpers;

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

            SpawnManager.SpawnDroppedPickups();
        }
    }
}
