using Actor.Player;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Environment
{
    public class postProcessingScript : MonoBehaviour
    {
        public PostProcessVolume volume;
        private Vignette vignette;

        void Start() {
            volume.profile.TryGetSettings(out vignette);
            vignette.intensity.value = 0.9f; // TODO: Get this from a constant or variable
            vignette.enabled.value = true;
        }

        public void SettVignetteIntensity(float value) {
            vignette.intensity.value = value;
        }
    }
}
