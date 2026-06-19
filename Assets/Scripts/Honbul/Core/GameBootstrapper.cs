using UnityEngine;
using UnityEngine.SceneManagement;

namespace Honbul
{
    public class GameBootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneKind kind = SceneKind.Intro;

            if (sceneName == GameConfig.GameSceneName)
            {
                kind = SceneKind.Game;
            }
            else if (sceneName == GameConfig.EndingSceneName)
            {
                kind = SceneKind.Ending;
            }

            ISceneInstaller installer;
            switch (kind)
            {
                case SceneKind.Game:
                    installer = new GameInstaller();
                    break;
                case SceneKind.Ending:
                    installer = new EndingInstaller();
                    break;
                default:
                    installer = new IntroInstaller();
                    break;
            }

            installer.Install();
        }
    }
}
