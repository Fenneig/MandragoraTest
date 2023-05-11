using System;
using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Interactables
{
    public class WarehouseComponent : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _interactPosition;
        [SerializeField] private Transform _interactLookAtPosition;
        private static readonly int Interact1 = Animator.StringToHash("Interact");

        public event Action OnInteractionCompleted;

        public void StartInteractSequence(Unit unit, bool isQueuedAction)
        {
            unit.MoveComponent.Move(_interactPosition.position, isQueuedAction);
            unit.RotateComponent.Rotate(_interactLookAtPosition.position, true);
            unit.Interact(this, true, InteractType.TakeCargo);
        }

        public void Interact()
        {
            _animator.SetTrigger(Interact1);
        }

        public void OnInteractionComplete()
        {
            Debug.Log("Interaction completed!");
            OnInteractionCompleted?.Invoke();
        }
        
    }
}