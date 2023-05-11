using System;
using Mandragora.UnitBased;
using Mandragora.Utils;
using UnityEngine;

namespace Mandragora.Interactables
{
    public class FactoryComponent : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _interactPosition;
        [SerializeField] private Transform _interactLookAtPosition;

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
            _currentUnitInteractWith.OnAnimationComplete += StartAnimation;
            _currentUnitInteractWith.TriggerAnimation(Idents.Animations.DeliverCargo);
        }

        private void StartAnimation()
        {
            _animator.SetTrigger(Idents.Animations.Interact);
            _currentUnitInteractWith.OnAnimationComplete -= StartAnimation;
        }

        private void AnimationComplete()
        {
            OnInteractionCompleted?.Invoke(_currentUnitInteractWith);
        }
    }
}