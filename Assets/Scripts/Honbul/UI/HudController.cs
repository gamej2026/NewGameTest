using UnityEngine;
using UnityEngine.UI;

namespace Honbul
{
    public class HudController : MonoBehaviour
    {
        private Canvas hudCanvas;
        private SubtitleView subtitleView;
        private InteractionPromptView interactionPromptView;
        private ObjectiveView objectiveView;

        public SubtitleView SubtitleView => subtitleView;
        public InteractionPromptView InteractionPromptView => interactionPromptView;
        public ObjectiveView ObjectiveView => objectiveView;

        private void Awake()
        {
            Build();
        }

        private void Build()
        {
            hudCanvas = UIFactory.CreateOverlayCanvas("HUD");

            Text subtitleText = UIFactory.CreateText(hudCanvas.transform, "SubtitleText", string.Empty, 30, TextAnchor.LowerCenter, GameConfig.MemoryGold);
            RectTransform subtitleRect = subtitleText.rectTransform;
            subtitleRect.anchorMin = new Vector2(0.1f, 0.06f);
            subtitleRect.anchorMax = new Vector2(0.9f, 0.2f);
            subtitleRect.offsetMin = Vector2.zero;
            subtitleRect.offsetMax = Vector2.zero;

            subtitleView = subtitleText.gameObject.AddComponent<SubtitleView>();
            subtitleView.Init(subtitleText);

            Text promptText = UIFactory.CreateText(hudCanvas.transform, "InteractionPromptText", string.Empty, 26, TextAnchor.MiddleCenter, Color.white);
            RectTransform promptRect = promptText.rectTransform;
            promptRect.anchorMin = new Vector2(0.33f, 0.2f);
            promptRect.anchorMax = new Vector2(0.67f, 0.28f);
            promptRect.offsetMin = Vector2.zero;
            promptRect.offsetMax = Vector2.zero;

            interactionPromptView = promptText.gameObject.AddComponent<InteractionPromptView>();
            interactionPromptView.Init(promptText);

            Text objectiveText = UIFactory.CreateText(hudCanvas.transform, "ObjectiveText", "목표: 단서를 모으세요", 24, TextAnchor.UpperLeft, Color.white);
            RectTransform objectiveRect = objectiveText.rectTransform;
            objectiveRect.anchorMin = new Vector2(0.03f, 0.9f);
            objectiveRect.anchorMax = new Vector2(0.45f, 0.98f);
            objectiveRect.offsetMin = Vector2.zero;
            objectiveRect.offsetMax = Vector2.zero;

            objectiveView = objectiveText.gameObject.AddComponent<ObjectiveView>();
            objectiveView.Init(objectiveText);
        }
    }
}
