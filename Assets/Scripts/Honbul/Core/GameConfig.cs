using UnityEngine;

namespace Honbul
{
    public static class GameConfig
    {
        public const string IntroSceneName = "Intro";
        public const string GameSceneName = "Game";
        public const string EndingSceneName = "Ending";

        public const float WalkSpeed = 2.5f;
        public const float Gravity = -9.81f;

        public const float LanternRange = 7f;
        public const float LanternIntensity = 1.2f;

        public const float CameraDistance = 4.5f;
        public const float CameraHeight = 1.6f;
        public const float CameraCollisionRadius = 0.3f;

        public const float InteractRange = 2.5f;
        public const float SpiritDetectRange = 6f;

        public static readonly Color SpiritTeal = Hex("#7FD1D4");
        public static readonly Color MemoryGold = Hex("#C8A96A");
        public static readonly Color InkBg = Hex("#0B0D12");

        public static Color Hex(string value)
        {
            if (ColorUtility.TryParseHtmlString(value, out Color color))
            {
                return color;
            }

            return Color.white;
        }
    }
}
