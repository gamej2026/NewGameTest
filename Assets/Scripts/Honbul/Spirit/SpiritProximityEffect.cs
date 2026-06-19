using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Honbul
{
    public class SpiritProximityEffect : MonoBehaviour
    {
        public Volume fxVolume;
        public Light spiritGlow;

        private Vignette cachedVignette;
        private float baseGlowIntensity = 0.8f;

        public void SetVignette(Vignette vignette)
        {
            cachedVignette = vignette;
        }

        private void Awake()
        {
            if (spiritGlow != null)
            {
                baseGlowIntensity = Mathf.Max(0.01f, spiritGlow.intensity);
            }

            TryCacheVignette();
        }

        public void SetIntensity(float t01)
        {
            t01 = Mathf.Clamp01(t01);
            TryCacheVignette();

            if (cachedVignette != null)
            {
                float vignette = Mathf.Lerp(0.18f, 0.52f, t01);
                cachedVignette.intensity.Override(vignette);
            }

            if (spiritGlow != null)
            {
                float tremorSpeed = Mathf.Lerp(1.2f, 14f, t01);
                float tremorAmount = Mathf.Lerp(0.02f, 0.38f, t01);
                float tremor = (Mathf.PerlinNoise(Time.time * tremorSpeed, 0f) - 0.5f) * 2f * tremorAmount;
                float target = Mathf.Lerp(baseGlowIntensity * 0.5f, baseGlowIntensity * 2.8f, t01);
                spiritGlow.intensity = Mathf.Max(0f, target + tremor);
            }
        }

        private void TryCacheVignette()
        {
            if (cachedVignette != null)
            {
                return;
            }

            if (PostFxBuilder.TryGetVignette(fxVolume, out Vignette vignette))
            {
                cachedVignette = vignette;
            }
        }
    }
}
