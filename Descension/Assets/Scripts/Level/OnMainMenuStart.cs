using Managers;
using UnityEngine;

namespace Level
{
    public class OnMainMenuStart : MonoBehaviour
    {
        void Start()
        {
            SoundManager.StartMainMenuBackgroundAudio();
        }
    }
}
