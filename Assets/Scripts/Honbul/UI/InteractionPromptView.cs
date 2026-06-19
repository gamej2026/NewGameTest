using UnityEngine;
using UnityEngine.UI;

namespace Honbul
{
    public class InteractionPromptView : MonoBehaviour
    {
        [SerializeField] private Text promptText;

        public void Init(Text target)
        {
            promptText = target;
            Hide();
        }

        public void Set(string content)
        {
            if (promptText == null)
            {
                return;
            }

            promptText.text = content;
            promptText.gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (promptText == null)
            {
                return;
            }

            promptText.gameObject.SetActive(false);
        }
    }
}
