using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Honbul
{
    public class EndingInstaller : ISceneInstaller
    {
        public void Install()
        {
            GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            cameraObject.tag = "MainCamera";

            Camera camera = cameraObject.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = GameConfig.InkBg;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 120f;

            Volume volume = PostFxBuilder.BuildGlobalVolume(true, false);
            if (volume != null && volume.sharedProfile != null && volume.sharedProfile.TryGet(out Bloom bloom))
            {
                bloom.intensity.Override(1.45f);
                bloom.threshold.Override(0.7f);
                bloom.active = true;
            }

            CreateEndingFire();

            Canvas canvas = UIFactory.CreateOverlayCanvas("EndingCanvas");
            string credits =
                "혼 (魂)\n\n" +
                "제작\n" +
                "기획 · 개발 · 연출\n" +
                "혼 팀\n\n" +
                "감사\n" +
                "플레이해 주신 모든 분들께\n" +
                "깊이 감사드립니다.\n\n" +
                "당신의 마음에도\n" +
                "잊힌 온기가 닿기를.";

            Text creditsText = UIFactory.CreateText(canvas.transform, "CreditsText", credits, 52, TextAnchor.UpperCenter, GameConfig.MemoryGold);

            RectTransform rect = creditsText.rectTransform;
            rect.anchorMin = new Vector2(0.12f, 0f);
            rect.anchorMax = new Vector2(0.88f, 1f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.anchoredPosition = new Vector2(0f, -520f);

            CreditsUI creditsUI = canvas.gameObject.AddComponent<CreditsUI>();
            creditsUI.creditsRect = rect;
            creditsUI.scrollSpeed = 40f;
        }

        private static void CreateEndingFire()
        {
            GameObject fireObject = new GameObject("EndingDokkaebiFire", typeof(ParticleSystem));
            fireObject.transform.position = new Vector3(0f, 0.4f, 8f);
            ParticleSystem ps = fireObject.GetComponent<ParticleSystem>();

            var main = ps.main;
            main.loop = true;
            main.playOnAwake = true;
            main.startLifetime = new ParticleSystem.MinMaxCurve(1f, 1.8f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.9f, 2.2f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.2f);
            main.startColor = new ParticleSystem.MinMaxGradient(new Color(GameConfig.SpiritTeal.r, GameConfig.SpiritTeal.g, GameConfig.SpiritTeal.b, 0.92f));
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = ps.emission;
            emission.rateOverTime = 38f;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.radius = 0.34f;
            shape.angle = 14f;

            ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
                if (shader == null)
                {
                    shader = Shader.Find("Sprites/Default");
                }

                Material mat = new Material(shader);
                if (mat.HasProperty("_BaseColor"))
                {
                    mat.SetColor("_BaseColor", GameConfig.SpiritTeal);
                }

                if (mat.HasProperty("_Color"))
                {
                    mat.SetColor("_Color", GameConfig.SpiritTeal);
                }

                renderer.material = mat;
            }

            GameObject lightObject = new GameObject("EndingFireLight", typeof(Light));
            lightObject.transform.position = new Vector3(0f, 1.2f, 8f);
            Light light = lightObject.GetComponent<Light>();
            light.type = LightType.Point;
            light.range = 10f;
            light.intensity = 1.2f;
            light.color = GameConfig.SpiritTeal;
        }
    }
}
