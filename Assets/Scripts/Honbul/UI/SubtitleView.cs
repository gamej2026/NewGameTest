using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Honbul
{
    public class SubtitleView : MonoBehaviour
    {
        [SerializeField] private Text subtitleText;
        private Coroutine running;

        public void Init(Text target)
        {
            subtitleText = target;
            SetAlpha(0f);
        }

        public Coroutine Show(string line, float seconds)
        {
            if (running != null)
            {
                StopCoroutine(running);
            }

            running = StartCoroutine(ShowRoutine(new[] { line }, seconds));
            return running;
        }

        public Coroutine ShowSequence(string[] lines, float perLine)
        {
            if (running != null)
            {
                StopCoroutine(running);
            }

            running = StartCoroutine(ShowRoutine(lines, perLine));
            return running;
        }

        private IEnumerator ShowRoutine(string[] lines, float perLine)
        {
            if (subtitleText == null || lines == null || lines.Length == 0)
            {
                yield break;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                subtitleText.text = lines[i];
                yield return FadeTo(1f, 0.2f);
                yield return new WaitForSeconds(Mathf.Max(0.1f, perLine));
                yield return FadeTo(0f, 0.25f);
            }
        }

        private IEnumerator FadeTo(float target, float duration)
        {
            float start = subtitleText.color.a;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(start, target, elapsed / duration);
                SetAlpha(alpha);
                yield return null;
            }

            SetAlpha(target);
        }

        private void SetAlpha(float alpha)
        {
            if (subtitleText == null)
            {
                return;
            }

            Color color = subtitleText.color;
            color.a = alpha;
            subtitleText.color = color;
        }
    }
}
