using UnityEngine;

namespace Honbul
{
    public class CreditsUI : MonoBehaviour
    {
        public RectTransform creditsRect;
        public float scrollSpeed = 38f;

        private bool requestedLoad;

        private void Update()
        {
            if (creditsRect == null || requestedLoad)
            {
                return;
            }

            Vector2 pos = creditsRect.anchoredPosition;
            pos.y += Mathf.Max(0f, scrollSpeed) * Time.deltaTime;
            creditsRect.anchoredPosition = pos;

            float threshold = creditsRect.rect.height + 720f;
            if (creditsRect.anchoredPosition.y >= threshold)
            {
                requestedLoad = true;
                SceneFlow.LoadIntro();
            }
        }
    }
}
