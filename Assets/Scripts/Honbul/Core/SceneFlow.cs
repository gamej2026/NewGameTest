using UnityEngine.SceneManagement;

namespace Honbul
{
    public static class SceneFlow
    {
        public static void LoadIntro()
        {
            SceneManager.LoadScene(GameConfig.IntroSceneName);
        }

        public static void LoadGame()
        {
            SceneManager.LoadScene(GameConfig.GameSceneName);
        }

        public static void LoadEnding()
        {
            SceneManager.LoadScene(GameConfig.EndingSceneName);
        }
    }
}
