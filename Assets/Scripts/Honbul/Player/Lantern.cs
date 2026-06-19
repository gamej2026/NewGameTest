using UnityEngine;

namespace Honbul
{
    public class Lantern : MonoBehaviour
    {
        [SerializeField] private Light lanternLight;
        [SerializeField] private float flickerSpeed = 2.2f;
        [SerializeField] private float flickerAmount = 0.15f;

        private float baseIntensity;

        private void Start()
        {
            lanternLight = lanternLight != null ? lanternLight : GetComponent<Light>();
            if (lanternLight == null)
            {
                lanternLight = gameObject.AddComponent<Light>();
            }

            lanternLight.type = LightType.Point;
            lanternLight.range = GameConfig.LanternRange;
            lanternLight.intensity = GameConfig.LanternIntensity;
            lanternLight.color = GameConfig.MemoryGold;
            lanternLight.shadows = LightShadows.Soft;

            baseIntensity = lanternLight.intensity;
        }

        private void Update()
        {
            if (lanternLight == null)
            {
                return;
            }

            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
            float flicker = 1f + (noise - 0.5f) * 2f * flickerAmount;
            lanternLight.intensity = baseIntensity * flicker;
        }
    }
}