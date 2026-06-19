using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Honbul
{
    public static class PostFxBuilder
    {
        public static Volume BuildGlobalVolume(bool bloom, bool vignette)
        {
            GameObject volumeObject = new GameObject("Global Volume", typeof(Volume));
            Volume volume = volumeObject.GetComponent<Volume>();
            volume.isGlobal = true;

            VolumeProfile profile = ScriptableObject.CreateInstance<VolumeProfile>();
            volume.sharedProfile = profile;

            if (bloom)
            {
                Bloom b = profile.Add<Bloom>(true);
                b.intensity.Override(0.6f);
                b.active = true;
            }

            if (vignette)
            {
                Vignette v = profile.Add<Vignette>(true);
                v.intensity.Override(0.35f);
                v.active = true;
            }

            return volume;
        }

        public static void EnableFog(Color color, float density)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = color;
            RenderSettings.fogMode = FogMode.Exponential;
            RenderSettings.fogDensity = density;
        }

        public static bool TryGetVignette(Volume v, out Vignette vig)
        {
            vig = null;
            return v != null && v.sharedProfile != null && v.sharedProfile.TryGet(out vig);
        }
    }
}