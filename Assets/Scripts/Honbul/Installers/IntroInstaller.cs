using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Honbul
{
    public class IntroInstaller : ISceneInstaller
    {
        public void Install()
        {
            GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            cameraObject.tag = "MainCamera";
            Camera camera = cameraObject.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = GameConfig.InkBg;

            GameObject volumeObject = new GameObject("Global Volume", typeof(Volume));
            Volume volume = volumeObject.GetComponent<Volume>();
            volume.isGlobal = true;
            volume.priority = 0f;

            Canvas canvas = UIFactory.CreateOverlayCanvas("IntroCanvas");

            Text titleText = UIFactory.CreateText(canvas.transform, "TitleText", "혼 · 잊혀진 마음을 잇다", 58, TextAnchor.MiddleCenter, GameConfig.MemoryGold);
            RectTransform titleRect = titleText.rectTransform;
            titleRect.anchorMin = new Vector2(0.1f, 0.62f);
            titleRect.anchorMax = new Vector2(0.9f, 0.82f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            Text narrationText = UIFactory.CreateText(canvas.transform, "NarrationText", string.Empty, 32, TextAnchor.MiddleCenter, Color.white);
            RectTransform narrationRect = narrationText.rectTransform;
            narrationRect.anchorMin = new Vector2(0.12f, 0.22f);
            narrationRect.anchorMax = new Vector2(0.88f, 0.38f);
            narrationRect.offsetMin = Vector2.zero;
            narrationRect.offsetMax = Vector2.zero;

            SubtitleView subtitleView = narrationText.gameObject.AddComponent<SubtitleView>();
            subtitleView.Init(narrationText);

            Text promptText = UIFactory.CreateText(canvas.transform, "PromptText", "아무 키나 누르세요", 28, TextAnchor.MiddleCenter, GameConfig.SpiritTeal);
            RectTransform promptRect = promptText.rectTransform;
            promptRect.anchorMin = new Vector2(0.3f, 0.08f);
            promptRect.anchorMax = new Vector2(0.7f, 0.16f);
            promptRect.offsetMin = Vector2.zero;
            promptRect.offsetMax = Vector2.zero;

            IntroUI introUI = canvas.gameObject.AddComponent<IntroUI>();
            introUI.Initialize(subtitleView, promptText);
        }
    }
}
