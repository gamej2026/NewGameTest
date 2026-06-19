using UnityEngine;

namespace Honbul
{
    public class PlayerInteractor : MonoBehaviour
    {
        public float range = GameConfig.InteractRange;
        public InteractionPromptView prompt;

        private Interactable current;

        private void Reset()
        {
            range = GameConfig.InteractRange;
        }

        private void Update()
        {
            current = FindBestInteractable();

            if (current != null)
            {
                if (prompt != null)
                {
                    prompt.Set(current.PromptText);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    current.Interact(this);
                }
            }
            else if (prompt != null)
            {
                prompt.Hide();
            }
        }

        private Interactable FindBestInteractable()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);

            Interactable best = null;
            float bestScore = float.MinValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                Collider col = colliders[i];
                if (col == null)
                {
                    continue;
                }

                Interactable candidate = col.GetComponent<Interactable>();
                if (candidate == null)
                {
                    candidate = col.GetComponentInParent<Interactable>();
                }

                if (candidate == null)
                {
                    continue;
                }

                Vector3 to = candidate.transform.position - transform.position;
                to.y = 0f;
                float dist = to.magnitude;
                if (dist > range || dist <= 0.001f)
                {
                    continue;
                }

                Vector3 dir = to / dist;
                float front = Vector3.Dot(transform.forward, dir);
                if (front < -0.15f)
                {
                    continue;
                }

                float score = front * 2f - dist;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = candidate;
                }
            }

            return best;
        }
    }
}