using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Environment
{
    public class postProcessingScript : MonoBehaviour
    {
        public PostProcessVolume volume;
        private Vignette vignette;

        [SerializeField] public static readonly float DefaultIntensity = 0.9f;
        [SerializeField] public static readonly float TorchIntensity = 0.5f;

        void Start() {
            volume.profile.TryGetSettings(out vignette);
            SetVignetteIntensity(DefaultIntensity);
            Enable();
        }

        public void Enable() => vignette.enabled.value = true;
        public void Disable() => vignette.enabled.value = false;

        public void SetVignetteIntensity(float value) => vignette.intensity.value = value;
    }
}
