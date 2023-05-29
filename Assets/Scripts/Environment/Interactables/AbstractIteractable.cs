using System;
using System.Collections.Generic;
using System.Linq;
using Mandragora.Commands;
using Mandragora.Systems;
using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Environment.Interactables
{
    public abstract class AbstractInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] private Transform _interactPosition;
        [SerializeField] private Transform _interactLookAtPosition;
        [SerializeField] private Transform _queueDirection;
        [SerializeField] private Transform _exitPosition;
        
        protected Queue<Unit> UnitsInQueue = new Queue<Unit>();
        protected Unit CurrentUnitInteractWith;
        protected string QueueId;
        private AlertService _alertService;

        public abstract string Name { get; }
        public bool InteractionInProgress { get; private set; }
        public Vector3 InteractPosition => _interactPosition.position;
        public Vector3 InteractLookAtPosition => _interactLookAtPosition.position;
        public event Action<Unit> OnInteractionCompleted;

        private CommandService _commandService;

        private void Start()
        {
            GatherServices();
            
            _commandService.OnAnyActionCanceled += UpdateQueue;
            QueueId = Name + "_Id";
        }

        private void GatherServices()
        {
            _commandService = GameManager.ServiceLocator.Get<CommandService>();
            _alertService = GameManager.ServiceLocator.Get<AlertService>();
        }

        private void UpdateQueue(Unit unit)
        {
            if (!UnitsInQueue.Contains(unit) || _alertService.IsAlert) return;

            UnitsInQueue = new Queue<Unit>(UnitsInQueue.Where(excludedUnit => excludedUnit != unit));
            QueueCommand.OnAnyCommandQueueChanged?.Invoke(UnitsInQueue, QueueId);
        }

        public void StartInteractSequence(Unit unit, bool isQueuedAction)
        {
            if (UnitsInQueue.Contains(unit) && !isQueuedAction) return;
            unit.QueueComponent.Enqueue(unit, UnitsInQueue, QueueId, InteractPosition, _queueDirection.position, isQueuedAction);
            unit.InteractComponent.Interact(this, true);
            unit.MoveComponent.Move(_exitPosition.position, true);
        }

        public virtual void Interact()
        {
            InteractionInProgress = true;
        }

        protected virtual void OnInteractionComplete()
        {
            OnInteractionCompleted?.Invoke(CurrentUnitInteractWith);
            InteractionInProgress = false;
        }

        private void OnDestroy()
        {
            _commandService.OnAnyActionCanceled -= UpdateQueue;
        }
    }
}