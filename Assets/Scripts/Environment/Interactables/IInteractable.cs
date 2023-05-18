using System;
using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Environment.Interactables
{
    public interface IInteractable
    {
        public string Name { get; }
        public bool InteractionInProgress { get; }
        public Vector3 InteractPosition { get; }
        public Vector3 InteractLookAtPosition { get; }
        public event Action<Unit> OnInteractionCompleted;
        public void StartInteractSequence(Unit unit, bool isQueuedAction);
        public void Interact();
    }
}