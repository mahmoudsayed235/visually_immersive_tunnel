using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] 
    private Volume postProcessVolume;

    private Bloom bloom;
    private ColorAdjustments colorAdjustments;

    private void Awake()
    {
        if (postProcessVolume == null)
            postProcessVolume = GetComponent<Volume>();

        if (postProcessVolume.profile.TryGet(out Bloom bloomEffect))
            bloom = bloomEffect;

        if (postProcessVolume.profile.TryGet(out ColorAdjustments colorEffect))
            colorAdjustments = colorEffect;
    }

    public void SetBloom(float intensity)
    {
        if (bloom != null)
            bloom.intensity.value = intensity;
    }

    public void SetSaturation(float saturation)
    {
        if (colorAdjustments != null)
            colorAdjustments.saturation.value = saturation;
    }

    public void EnableBloom(bool enabled)
    {
        if (bloom != null)
            bloom.active = enabled;
    }
}
