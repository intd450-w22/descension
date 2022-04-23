using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Level
{
    public class postProcessingScript : MonoBehaviour
    {
        public PostProcessVolume volume;
        private Vignette vignette;

        void Start() {
            volume.profile.TryGetSettings(out vignette);
            Enable();
        }

        public void Enable() => vignette.enabled.value = true;
        public void Disable() => vignette.enabled.value = false;

        public void SetVignetteIntensity(float value) => vignette.intensity.value = value;
    }
}
