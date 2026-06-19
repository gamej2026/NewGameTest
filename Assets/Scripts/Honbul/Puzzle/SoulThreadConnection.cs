using UnityEngine;

namespace Honbul
{
    public class SoulThreadConnection : MonoBehaviour
    {
        public MemoryNode a;
        public MemoryNode b;

        private LineRenderer line;

        public void Init(MemoryNode from, MemoryNode to)
        {
            a = from;
            b = to;

            line = GetComponent<LineRenderer>();
            if (line == null)
            {
                line = gameObject.AddComponent<LineRenderer>();
            }

            line.positionCount = 2;
            line.widthMultiplier = 0.05f;
            line.useWorldSpace = true;
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = GameConfig.SpiritTeal;
            line.endColor = GameConfig.SpiritTeal;

            Refresh();
        }

        public void Refresh()
        {
            if (line == null || a == null || b == null)
            {
                return;
            }

            line.SetPosition(0, a.AnchorPoint);
            line.SetPosition(1, b.AnchorPoint);
        }

        public void SetColor(Color c)
        {
            if (line == null)
            {
                return;
            }

            line.startColor = c;
            line.endColor = c;
        }

        private void Update()
        {
            Refresh();
        }
    }
}
