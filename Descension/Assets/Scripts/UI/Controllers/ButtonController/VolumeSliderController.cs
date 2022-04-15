using System;
using Managers;
using UnityEngine;

namespace UI.Controllers.ButtonController
{
    public class VolumeSliderController : SliderController
    {
        public enum VolumeType
        {
            BackgroundVolume,
            EffectVolume,
            MusicVolume
        }

        [SerializeField] private VolumeType _volumeType;

        public void OnSliderChanged(float value)
        {
            switch (_volumeType)
            {
                case VolumeType.BackgroundVolume:
                    SoundManager.SetBackgroundVolume(value);
                    return;
                case VolumeType.EffectVolume:
                    SoundManager.SetBackgroundVolume(value);
                    return;
                case VolumeType.MusicVolume:
                    SoundManager.SetMusicVolume(value);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}