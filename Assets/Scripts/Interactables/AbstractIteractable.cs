﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mandragora.Commands;
using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Interactables
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

        public abstract string Name { get; }
        public event Action<Unit> OnInteractionCompleted;

        private void Start()
        {
            BaseCommand.OnAnyActionCanceled += UpdateQueue;
            QueueId = Name + "_Id";
        }

        private void UpdateQueue(Unit unit)
        {
            if (!UnitsInQueue.Contains(unit)) return;

            UnitsInQueue = new Queue<Unit>(UnitsInQueue.Where(excludedUnit => excludedUnit != unit));
            QueueCommand.OnAnyQueueChanged?.Invoke(UnitsInQueue, QueueId);
        }

        public void StartInteractSequence(Unit unit, bool isQueuedAction)
        {
            if (UnitsInQueue.Contains(unit) && !isQueuedAction) return;
            var tempqueue = new Queue<Unit>(UnitsInQueue);
            tempqueue.Enqueue(unit);
            var command = new QueueCommand(unit, tempqueue, _interactPosition.position, _queueDirection.position, QueueId);
            if (isQueuedAction) command.AddToQueue(unit);
            else command.StartNewQueue(unit);
            UnitsInQueue = new Queue<Unit>(tempqueue);
            unit.RotateComponent.Rotate(_interactLookAtPosition.position, true);
            unit.InteractComponent.Interact(this, true);
            unit.MoveComponent.Move(_exitPosition.position, true);
        }

        public abstract void Interact();

        public virtual void OnInteractionComplete()
        {
            OnInteractionCompleted?.Invoke(CurrentUnitInteractWith);
        }

        private void OnDestroy()
        {
            BaseCommand.OnAnyActionCanceled -= UpdateQueue;
        }
    }
}