using UnityEngine;

namespace Honbul
{
    public class MemoryPickup : Interactable
    {
        public MemoryNodeData data;
        public SubtitleView subtitle;
        public ObjectiveView objective;

        public System.Action<MemoryNodeData> OnCollected;

        private bool collected;

        public override string PromptText => "조사하기 (E)";

        private void Reset()
        {
            EnsureCollider();
        }

        private void Awake()
        {
            EnsureCollider();
        }

        public override void Interact(PlayerInteractor by)
        {
            if (collected)
            {
                return;
            }

            collected = true;

            if (subtitle != null && data != null)
            {
                subtitle.Show(data.Body, 3.2f);
            }

            GameState.CluesCollected++;
            OnCollected?.Invoke(data);

            if (objective != null)
            {
                objective.SetObjective(PuzzleData.ObjectiveCollect(GameState.CluesCollected, GameState.TotalClues));
            }

            gameObject.SetActive(false);
        }

        private void EnsureCollider()
        {
            if (GetComponent<Collider>() != null)
            {
                return;
            }

            SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
            sphere.radius = 0.45f;
        }
    }
}
