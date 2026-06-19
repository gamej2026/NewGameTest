using UnityEngine;
using UnityEngine.UI;

namespace Honbul
{
    public class ObjectiveView : MonoBehaviour
    {
        [SerializeField] private Text objectiveText;

        public void Init(Text target)
        {
            objectiveText = target;
        }

        public void SetObjective(string content)
        {
            if (objectiveText == null)
            {
                return;
            }

            objectiveText.text = content;
        }
    }
}
