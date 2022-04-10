using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Environment
{
    public class postProcessingScript : MonoBehaviour
    {
        public PostProcessVolume volume;
        private Vignette vignette;

        [SerializeField] private readonly float _defaultIntensity = 0.9f;
        [SerializeField] private readonly float _torchIntensity = 0.5f;

        void Start() {
            volume.profile.TryGetSettings(out vignette);
            SetDefaultIntensity();
            Enable();

            Debug.Log("Post Processing Start");
        }

        public void Enable() => vignette.enabled.value = true;
        public void Disable() => vignette.enabled.value = false;

        public void SetDefaultIntensity() => SetVignetteIntensity(_defaultIntensity);
        public void SetTorchIntensity() => SetVignetteIntensity(_torchIntensity);
        private void SetVignetteIntensity(float value) => vignette.intensity.value = value;
    }
}
