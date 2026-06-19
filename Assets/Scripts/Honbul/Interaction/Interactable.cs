using UnityEngine;

namespace Honbul
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract string PromptText { get; }

        public abstract void Interact(PlayerInteractor by);
    }
}