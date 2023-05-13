using System;
using System.Collections.Generic;
using System.Linq;
using Mandragora.Commands;
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
        [SerializeField] private Transform _queueDirection;
        [SerializeField] private Transform _exitPosition;

        private Queue<Unit> _unitsQueue = new Queue<Unit>();

        public event Action<Unit> OnInteractionCompleted;

        private void Start()
        {
            BaseCommand.OnAnyActionCanceled += UpdateQueue;
        }

        private void UpdateQueue(Unit unit)
        {
            if (!_unitsQueue.Contains(unit)) return;

            _unitsQueue = new Queue<Unit>(_unitsQueue.Where(excludedUnit => excludedUnit != unit));
            QueueCommand.OnAnyQueueChanged?.Invoke(_unitsQueue);
        }

        public void StartInteractSequence(Unit unit, bool isQueuedAction)
        {
            if (_unitsQueue.Contains(unit) && !isQueuedAction) return;
            _unitsQueue.Enqueue(unit);
            new QueueCommand(unit, _unitsQueue, _interactPosition.position, _queueDirection.position).AddToQueue(unit);
            unit.RotateComponent.Rotate(_interactLookAtPosition.position, true);
            unit.Interact(this, true);
            if (!isQueuedAction) unit.MoveComponent.Move(_exitPosition.position, true);
        }

        public void Interact()
        {
            _animator.SetTrigger(Idents.Animations.Interact);
        }

        public void OnInteractionComplete()
        {
            var unit = _unitsQueue.Dequeue();
            QueueCommand.OnAnyQueueChanged?.Invoke(_unitsQueue);
            unit.TriggerAnimation(Idents.Animations.TakeCargo);
            OnInteractionCompleted?.Invoke(unit);
        }

        private void OnDestroy()
        {
            BaseCommand.OnAnyActionCanceled -= UpdateQueue;
        }
    }
}