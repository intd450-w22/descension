using Managers;
using UnityEngine;
using Util.Enums;

public class OnLevelStart : MonoBehaviour
{
    private Scene LevelType;

    void Start()
    {
        // Init any level things
        SoundManager.StartBackgroundAudio();
    }
}
