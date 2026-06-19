using UnityEngine;

namespace Honbul
{
    public class MemoryNode : MonoBehaviour
    {
        public MemoryNodeData data;
        public Renderer rend;

        private bool selected;

        public Vector3 AnchorPoint => transform.position;

        private void Awake()
        {
            if (rend == null)
            {
                rend = GetComponent<Renderer>();
            }
        }

        public void SetHighlight(bool on)
        {
            selected = on;

            if (rend == null)
            {
                return;
            }

            Material mat = rend.material;
            if (mat == null)
            {
                return;
            }

            if (selected)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", GameConfig.MemoryGold * 2.5f);
            }
            else
            {
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}
