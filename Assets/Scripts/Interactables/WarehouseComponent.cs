using System;
using Mandragora.UnitBased;
using Mandragora.Utils;
using UnityEngine;

namespace Mandragora.Interactables
{
    public class WarehouseComponent : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _interactPosition;
        [SerializeField] private Transform _interactLookAtPosition;
        private static readonly int InteractAnimation = Animator.StringToHash("Interact");

        private Unit _currentUnitInteractWith;

        public event Action<Unit> OnInteractionCompleted;

        public void StartInteractSequence(Unit unit, bool isQueuedAction)
        {
            _currentUnitInteractWith = unit;
            unit.MoveComponent.Move(_interactPosition.position, isQueuedAction);
            unit.RotateComponent.Rotate(_interactLookAtPosition.position, true);
            unit.Interact(this, true);
        }

        public void Interact()
        {
            _animator.SetTrigger(InteractAnimation);
        }

        public void OnInteractionComplete()
        {
            _currentUnitInteractWith.TriggerAnimation(Idents.Animations.TakeCargo);
            OnInteractionCompleted?.Invoke(_currentUnitInteractWith);
        }
    }
}