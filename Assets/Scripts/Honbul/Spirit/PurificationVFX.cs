using System.Collections;
using UnityEngine;

namespace Honbul
{
    public class PurificationVFX : MonoBehaviour
    {
        public ParticleSystem dokkaebiFire;
        public Light fireLight;
        public Renderer spiritBody;

        private Material spiritMaterial;
        private Vector3 baseScale = Vector3.one;

        private void Awake()
        {
            if (spiritBody != null)
            {
                spiritMaterial = spiritBody.material;
                baseScale = spiritBody.transform.localScale;
            }

            if (fireLight != null)
            {
                fireLight.color = GameConfig.SpiritTeal;
            }

            if (dokkaebiFire != null)
            {
                var main = dokkaebiFire.main;
                main.startColor = new ParticleSystem.MinMaxGradient(GameConfig.SpiritTeal);
            }
        }

        public IEnumerator Play()
        {
            float duration = 2.4f;
            float elapsed = 0f;

            float lightStart = fireLight != null ? fireLight.intensity : 0f;
            Color baseColor = Color.white;
            bool hasBase = false;

            if (spiritMaterial != null)
            {
                if (spiritMaterial.HasProperty("_BaseColor"))
                {
                    baseColor = spiritMaterial.GetColor("_BaseColor");
                    hasBase = true;
                }
                else if (spiritMaterial.HasProperty("_Color"))
                {
                    baseColor = spiritMaterial.GetColor("_Color");
                    hasBase = true;
                }
            }

            if (dokkaebiFire != null)
            {
                dokkaebiFire.Play();
            }

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                if (fireLight != null)
                {
                    fireLight.intensity = Mathf.Lerp(lightStart, 0f, t);
                }

                if (spiritBody != null)
                {
                    spiritBody.transform.localScale = Vector3.Lerp(baseScale, Vector3.zero, t);
                }

                if (spiritMaterial != null && hasBase)
                {
                    Color c = baseColor;
                    c.a = Mathf.Lerp(baseColor.a, 0f, t);

                    if (spiritMaterial.HasProperty("_BaseColor"))
                    {
                        spiritMaterial.SetColor("_BaseColor", c);
                    }

                    if (spiritMaterial.HasProperty("_Color"))
                    {
                        spiritMaterial.SetColor("_Color", c);
                    }
                }

                yield return null;
            }

            if (fireLight != null)
            {
                fireLight.intensity = 0f;
            }

            if (spiritBody != null)
            {
                spiritBody.gameObject.SetActive(false);
            }

            if (dokkaebiFire != null)
            {
                dokkaebiFire.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }
}
