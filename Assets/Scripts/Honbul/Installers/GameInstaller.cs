using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Honbul
{
    public class GameInstaller : ISceneInstaller
    {
        public void Install()
        {
            MemoryNodeData[] fragments = PuzzleData.LetterFragments();
            GameState.Reset();
            GameState.TotalClues = fragments.Length;

            HanokAlleyBuilder.Build();

            CreateDirectionalLight();
            Volume volume = PostFxBuilder.BuildGlobalVolume(true, true);
            PostFxBuilder.EnableFog(Color.Lerp(GameConfig.InkBg, GameConfig.SpiritTeal, 0.14f), 0.03f);

            GameObject player = CreatePlayer();
            PlayerInteractor interactor = player.GetComponent<PlayerInteractor>();
            ThirdPersonController controller = player.GetComponent<ThirdPersonController>();

            Camera camera = CreateMainCamera(player.transform);
            ThirdPersonCamera thirdPersonCamera = camera.GetComponent<ThirdPersonCamera>();
            if (thirdPersonCamera != null)
            {
                thirdPersonCamera.target = player.transform;
            }

            if (controller != null)
            {
                controller.cameraTransform = camera.transform;
            }

            GameObject hudObject = new GameObject("HUD Root");
            HudController hud = hudObject.AddComponent<HudController>();

            if (interactor != null)
            {
                interactor.prompt = hud.InteractionPromptView;
            }

            if (hud.ObjectiveView != null)
            {
                hud.ObjectiveView.SetObjective(PuzzleData.ObjectiveExplore() + "\n" + PuzzleData.ObjectiveCollect(0, GameState.TotalClues));
            }

            SpiritController spirit = CreateSpirit(player.transform, hud, volume);

            GameObject puzzleObject = new GameObject("SoulThreadPuzzle", typeof(SoulThreadPuzzle));
            SoulThreadPuzzle puzzle = puzzleObject.GetComponent<SoulThreadPuzzle>();
            puzzle.cam = camera;
            puzzle.story = hud.SubtitleView;
            puzzle.objective = hud.ObjectiveView;
            puzzle.Build(fragments, new Vector3(0f, 1.35f, 23.6f));

            if (spirit != null)
            {
                puzzle.OnSolved += spirit.OnPuzzleSolved;
            }

            GameObject flowObject = new GameObject("GameFlowCoordinator", typeof(GameFlowCoordinator));
            GameFlowCoordinator coordinator = flowObject.GetComponent<GameFlowCoordinator>();
            coordinator.Initialize(puzzle, hud.ObjectiveView, GameState.TotalClues);

            CreateMemoryPickups(fragments, hud, coordinator);
        }

        private static void CreateDirectionalLight()
        {
            GameObject lightObject = new GameObject("Moon Light", typeof(Light));
            Light sun = lightObject.GetComponent<Light>();
            sun.type = LightType.Directional;
            sun.color = new Color(0.62f, 0.74f, 0.8f);
            sun.intensity = 0.35f;
            sun.shadows = LightShadows.Soft;
            lightObject.transform.rotation = Quaternion.Euler(27f, -28f, 0f);
        }

        private static GameObject CreatePlayer()
        {
            GameObject player = new GameObject("Player", typeof(CharacterController), typeof(ThirdPersonController), typeof(PlayerInteractor));
            player.transform.position = new Vector3(0f, 0.95f, 2.2f);

            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.height = 1.8f;
                cc.radius = 0.35f;
                cc.center = new Vector3(0f, 0.9f, 0f);
            }

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Body";
            body.transform.SetParent(player.transform, false);
            body.transform.localPosition = new Vector3(0f, 0f, 0f);
            body.transform.localScale = new Vector3(0.55f, 0.9f, 0.55f);
            Collider bodyCollider = body.GetComponent<Collider>();
            if (bodyCollider != null)
            {
                Object.Destroy(bodyCollider);
            }

            Renderer bodyRenderer = body.GetComponent<Renderer>();
            if (bodyRenderer != null)
            {
                bodyRenderer.sharedMaterial = HanokAlleyBuilder.UrpLit(new Color(0.2f, 0.22f, 0.24f), false);
            }

            GameObject lanternObject = new GameObject("Lantern", typeof(Light), typeof(Lantern));
            lanternObject.transform.SetParent(player.transform, false);
            lanternObject.transform.localPosition = new Vector3(0f, 1.4f, 0.35f);

            return player;
        }

        private static Camera CreateMainCamera(Transform target)
        {
            GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener), typeof(ThirdPersonCamera));
            cameraObject.tag = "MainCamera";

            Camera camera = cameraObject.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = GameConfig.InkBg;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 120f;

            cameraObject.transform.position = target.position + new Vector3(0f, 2.3f, -4.7f);
            cameraObject.transform.rotation = Quaternion.Euler(14f, 0f, 0f);

            return camera;
        }

        private static SpiritController CreateSpirit(Transform player, HudController hud, Volume fxVolume)
        {
            GameObject spiritObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spiritObject.name = "Spirit";
            spiritObject.transform.position = new Vector3(0f, 1.2f, 26.2f);
            spiritObject.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);

            Renderer spiritRenderer = spiritObject.GetComponent<Renderer>();
            if (spiritRenderer != null)
            {
                spiritRenderer.sharedMaterial = HanokAlleyBuilder.UrpLit(GameConfig.SpiritTeal, true);
            }

            SpiritController spiritController = spiritObject.AddComponent<SpiritController>();
            SpiritProximityEffect proximity = spiritObject.AddComponent<SpiritProximityEffect>();
            PurificationVFX purification = spiritObject.AddComponent<PurificationVFX>();

            GameObject glowObject = new GameObject("SpiritGlow", typeof(Light));
            glowObject.transform.SetParent(spiritObject.transform, false);
            glowObject.transform.localPosition = Vector3.zero;
            Light glow = glowObject.GetComponent<Light>();
            glow.type = LightType.Point;
            glow.range = 6.2f;
            glow.intensity = 1.1f;
            glow.color = GameConfig.SpiritTeal;

            GameObject shimmerObject = new GameObject("Shimmer", typeof(ParticleSystem));
            shimmerObject.transform.SetParent(spiritObject.transform, false);
            shimmerObject.transform.localPosition = Vector3.zero;
            ParticleSystem shimmer = shimmerObject.GetComponent<ParticleSystem>();
            ConfigureShimmerParticle(shimmer);

            GameObject fireObject = new GameObject("DokkaebiFire", typeof(ParticleSystem));
            fireObject.transform.SetParent(spiritObject.transform, false);
            fireObject.transform.localPosition = new Vector3(0f, -0.4f, 0f);
            ParticleSystem fire = fireObject.GetComponent<ParticleSystem>();
            ConfigureDokkaebiFireParticle(fire);

            GameObject fireLightObject = new GameObject("PurifyLight", typeof(Light));
            fireLightObject.transform.SetParent(spiritObject.transform, false);
            fireLightObject.transform.localPosition = Vector3.zero;
            Light fireLight = fireLightObject.GetComponent<Light>();
            fireLight.type = LightType.Point;
            fireLight.color = GameConfig.SpiritTeal;
            fireLight.intensity = 1.4f;
            fireLight.range = 5.4f;

            spiritController.player = player;
            spiritController.shimmer = shimmer;
            spiritController.detectRange = GameConfig.SpiritDetectRange;
            spiritController.purify = purification;
            spiritController.proximity = proximity;
            spiritController.objective = hud.ObjectiveView;

            proximity.fxVolume = fxVolume;
            proximity.spiritGlow = glow;

            purification.dokkaebiFire = fire;
            purification.fireLight = fireLight;
            purification.spiritBody = spiritRenderer;

            return spiritController;
        }

        private static void CreateMemoryPickups(MemoryNodeData[] fragments, HudController hud, GameFlowCoordinator coordinator)
        {
            float startZ = 6f;
            float endZ = 22f;

            for (int i = 0; i < fragments.Length; i++)
            {
                float t = fragments.Length <= 1 ? 0f : (float)i / (fragments.Length - 1);
                float z = Mathf.Lerp(startZ, endZ, t);
                float x = i % 2 == 0 ? -1.5f : 1.5f;

                GameObject pickupObject = GameObject.CreatePrimitive(i % 2 == 0 ? PrimitiveType.Cube : PrimitiveType.Sphere);
                pickupObject.name = "MemoryPickup_" + i;
                pickupObject.transform.position = new Vector3(x, 0.8f, z);
                pickupObject.transform.localScale = Vector3.one * 0.52f;

                Renderer renderer = pickupObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial = HanokAlleyBuilder.UrpLit(GameConfig.MemoryGold, true);
                }

                MemoryPickup pickup = pickupObject.AddComponent<MemoryPickup>();
                pickup.data = fragments[i];
                pickup.subtitle = hud.SubtitleView;
                pickup.objective = hud.ObjectiveView;
                pickup.OnCollected += coordinator.HandlePickupCollected;
            }
        }

        private static void ConfigureShimmerParticle(ParticleSystem ps)
        {
            if (ps == null)
            {
                return;
            }

            var main = ps.main;
            main.loop = true;
            main.playOnAwake = true;
            main.duration = 1.1f;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.35f, 0.7f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.05f, 0.35f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.04f, 0.09f);
            main.startColor = new ParticleSystem.MinMaxGradient(new Color(GameConfig.SpiritTeal.r, GameConfig.SpiritTeal.g, GameConfig.SpiritTeal.b, 0.8f));
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = ps.emission;
            emission.rateOverTime = 8f;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.55f;

            ParticleSystemRenderer psRenderer = ps.GetComponent<ParticleSystemRenderer>();
            if (psRenderer != null)
            {
                psRenderer.material = CreateParticleMaterial(new Color(GameConfig.SpiritTeal.r, GameConfig.SpiritTeal.g, GameConfig.SpiritTeal.b, 0.72f));
            }
        }

        private static void ConfigureDokkaebiFireParticle(ParticleSystem ps)
        {
            if (ps == null)
            {
                return;
            }

            var main = ps.main;
            main.loop = false;
            main.playOnAwake = false;
            main.duration = 2.3f;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.8f, 1.45f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(1f, 2.4f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.2f);
            main.startColor = new ParticleSystem.MinMaxGradient(new Color(GameConfig.SpiritTeal.r, GameConfig.SpiritTeal.g, GameConfig.SpiritTeal.b, 0.9f));
            main.gravityModifier = 0f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = ps.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 56) });

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.radius = 0.18f;
            shape.angle = 10f;

            ParticleSystemRenderer psRenderer = ps.GetComponent<ParticleSystemRenderer>();
            if (psRenderer != null)
            {
                psRenderer.material = CreateParticleMaterial(new Color(GameConfig.SpiritTeal.r, GameConfig.SpiritTeal.g, GameConfig.SpiritTeal.b, 0.95f));
            }
        }

        private static Material CreateParticleMaterial(Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
            if (shader == null)
            {
                shader = Shader.Find("Sprites/Default");
            }

            Material material = new Material(shader);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            return material;
        }
    }

    public class GameFlowCoordinator : MonoBehaviour
    {
        private SoulThreadPuzzle puzzle;
        private ObjectiveView objective;
        private int total;
        private bool unlocked;

        public void Initialize(SoulThreadPuzzle targetPuzzle, ObjectiveView targetObjective, int clueTotal)
        {
            puzzle = targetPuzzle;
            objective = targetObjective;
            total = Mathf.Max(0, clueTotal);
            unlocked = false;
        }

        public void HandlePickupCollected(MemoryNodeData _)
        {
            if (unlocked)
            {
                return;
            }

            if (GameState.CluesCollected < total)
            {
                return;
            }

            unlocked = true;

            if (objective != null)
            {
                objective.SetObjective(PuzzleData.ObjectiveConnect());
            }

            if (puzzle != null)
            {
                puzzle.Unlock();
            }
        }
    }
}
