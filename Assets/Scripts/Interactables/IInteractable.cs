using System;
using Mandragora.UnitBased;

namespace Mandragora.Interactables
{
    public interface IInteractable
    {
        public event Action OnInteractionCompleted;
        public void StartInteractSequence(Unit unit, bool isQueuedAction);
        public void Interact();
    }
}