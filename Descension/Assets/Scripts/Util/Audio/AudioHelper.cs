using System;
using System.Collections;
using UnityEngine;

namespace Util.Audio
{
    public static class AudioHelper
    {
        public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime, Action onStop = null)
        {
            var startVolume = audioSource.volume;
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }

            if (onStop == null)
                audioSource.Stop();
            else
                onStop();
        }

        public static IEnumerator FadeIn(AudioSource audioSource, float fadeTime, float volume = 1f, Action onStart = null)
        {
            if (onStart == null)
                audioSource.Play();
            else
                onStart();
            audioSource.volume = 0f;
            while (audioSource.volume < volume)
            {
                audioSource.volume += Time.deltaTime / fadeTime;
                yield return null;
            }
        }
    }
}