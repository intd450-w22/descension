using Actor.Player;
using Managers;
using UnityEngine;

namespace Level
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
