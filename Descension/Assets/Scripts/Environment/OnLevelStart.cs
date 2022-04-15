using Items.Pickups;
using Actor.Player;
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
            PlayerController.Enable();

            SoundManager.StartBackgroundAudio();
            UIManager.GetHudController().Reset(); // Move to an earlier call probably 

            GameManager.UnFreeze();
            GameManager.Resume();

            SpawnManager.SpawnCachedPickups();
        }
    }
}
