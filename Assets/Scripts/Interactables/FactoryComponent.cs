using System;
using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Interactables
{
    public class FactoryComponent : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _interactPosition;
        [SerializeField] private Transform _interactLookAtPosition;
        
        public event Action OnInteractionCompleted;

        public void StartInteractSequence(Unit unit, bool isQueuedAction)
        {
            unit.MoveComponent.Move(_interactPosition.position, isQueuedAction);
            unit.RotateComponent.Rotate(_interactLookAtPosition.position, true);
            unit.Interact(this, true, InteractType.DeliverCargo);
        }

        public void Interact()
        {
            
        }
    }
}