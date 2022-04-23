using Managers;
using UnityEngine;

namespace Environment
{
    public class OnMainMenuStart : MonoBehaviour
    {
        void Start()
        {
            SoundManager.StartMainMenuBackgroundAudio();
        }
    }
}
