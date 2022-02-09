using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class postProcessingScript : MonoBehaviour
{
    public PostProcessVolume volume;
    private Vignette vignette;

    // Start is called before the first frame update
    void Start() {
        volume.profile.TryGetSettings(out vignette);
        vignette.intensity.value = 1f;
        vignette.enabled.value = true;
    }

    // Update is called once per frame
    void Update() {
        if (FindObjectOfType<player>().torchQuantity > 0) {
            vignette.intensity.value = 0.5f;
        } else {
            vignette.intensity.value = 1f;
        }
    }
}
