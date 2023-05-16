using System;
using Mandragora.UnitBased;

namespace Mandragora.Environment.Interactables
{
    public interface IInteractable
    {
        public string Name { get; }
        public event Action<Unit> OnInteractionCompleted;
        public void StartInteractSequence(Unit unit, bool isQueuedAction);
        public void Interact();
    }
}