using System.Collections;
using UnityEngine;

namespace Honbul
{
    public class SpiritController : MonoBehaviour
    {
        public Transform player;
        public ParticleSystem shimmer;
        public float detectRange = GameConfig.SpiritDetectRange;
        public PurificationVFX purify;
        public SpiritProximityEffect proximity;
        public ObjectiveView objective;

        private bool purifying;

        private void Reset()
        {
            detectRange = GameConfig.SpiritDetectRange;
        }

        private void Update()
        {
            if (player == null)
            {
                if (proximity != null)
                {
                    proximity.SetIntensity(0f);
                }

                return;
            }

            float distance = Vector3.Distance(transform.position, player.position);
            float t01 = Mathf.InverseLerp(Mathf.Max(1.01f, detectRange), 1f, distance);
            t01 = Mathf.Clamp01(t01);

            if (proximity != null)
            {
                proximity.SetIntensity(t01);
            }

            if (shimmer != null)
            {
                var emission = shimmer.emission;
                emission.rateOverTime = Mathf.Lerp(4f, 42f, t01);
            }
        }

        public void OnPuzzleSolved()
        {
            if (purifying)
            {
                return;
            }

            StartCoroutine(PurifyRoutine());
        }

        private IEnumerator PurifyRoutine()
        {
            purifying = true;

            if (objective != null)
            {
                objective.SetObjective("혼이 정화되고 있다...");
            }

            if (purify != null)
            {
                yield return purify.Play();
            }

            SceneFlow.LoadEnding();
        }
    }
}
