using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Honbul
{
    public class IntroUI : MonoBehaviour
    {
        private SubtitleView subtitleView;
        private Text promptText;

        private readonly string[] narration =
        {
            "마음이 끊어진 자리에서, 혼은 길을 잃었다.",
            "흩어진 기억을 잇고, 잊힌 마음을 되돌려라.",
            "실을 따라가면, 다시 만날 수 있다."
        };

        public void Initialize(SubtitleView subtitle, Text prompt)
        {
            subtitleView = subtitle;
            promptText = prompt;

            if (promptText != null)
            {
                promptText.text = "아무 키나 누르세요";
                promptText.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            StartCoroutine(PlayIntroSequence());
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                SceneFlow.LoadGame();
            }
        }

        private IEnumerator PlayIntroSequence()
        {
            if (subtitleView != null)
            {
                subtitleView.ShowSequence(narration, 2.1f);
                yield return new WaitForSeconds(narration.Length * 2.55f);
            }

            if (promptText != null)
            {
                promptText.gameObject.SetActive(true);
            }
        }
    }
}
