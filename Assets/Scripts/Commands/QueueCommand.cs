using System;
using System.Collections.Generic;
using System.Linq;
using Mandragora.Systems;
using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Commands
{
    public class QueueCommand : BaseCommand
    {
        private Unit _unit;
        private Queue<Unit> _unitsQueue;
        private Vector3 _firstInQueuePosition;
        private Vector3 _queueDirection;
        private int _queuePosition;
        private string _queueId;

        public static Action<Queue<Unit>, string> OnAnyCommandQueueChanged;

        public QueueCommand(Unit unit, Queue<Unit> queue, string queueId, Vector3 firstInQueuePosition,
            Vector3 queueDirection)
        {
            _unit = unit;
            _unitsQueue = queue;
            _firstInQueuePosition = firstInQueuePosition;
            _queueDirection = queueDirection - firstInQueuePosition;
            _queueId = queueId;
            OnAnyCommandQueueChanged += UpdateQueue;
            CommandService.OnAnyActionCanceled += CheckIsThisCommandCanceled;
            _unit.MoveComponent.OnNavMeshReachDestination += AgentReady;
        }

        public override void StartCommandExecution()
        {
            if (!_unitsQueue.Contains(_unit)) _unitsQueue.Enqueue(_unit);

            MoveToNewPosition();
        }

        private void CheckIsThisCommandCanceled(Unit unit)
        {
            if (!CommandService.CurrentUnitsCommand.TryGetValue(unit, out var currentCommand) || currentCommand != this) return;
            _unit.Agent.SetDestination(_unit.transform.position);
            _unit.MoveComponent.IsAgentHavePath = false;
            if (AlertService.IsAlert) return;
            _unitsQueue = new Queue<Unit>(_unitsQueue.Where(excludedUnit => excludedUnit != unit));
            OnAnyCommandQueueChanged?.Invoke(_unitsQueue, _queueId);
            _unit.MoveComponent.OnNavMeshReachDestination -= AgentReady;
            OnAnyCommandQueueChanged -= UpdateQueue;
        }

        private void UpdateQueue(Queue<Unit> newQueue, string queueId)
        {
            if (_queueId != queueId) return;
            _unitsQueue = newQueue;
            MoveToNewPosition();
        }

        private void AgentReady(Unit unit)
        {
            if (!_unit.MoveComponent.IsAgentReachDestination(_firstInQueuePosition)) return;

            CommandExecutionComplete(_unit);
        }

        private void MoveToNewPosition()
        {
            Vector3 moveToPosition = GetQueuePosition();
            if (_queuePosition < 0) return;
            _unit.MoveComponent.IsAgentHavePath = true;
            _unit.Agent.SetDestination(moveToPosition);
        }

        private Vector3 GetQueuePosition()
        {
            _queuePosition = _unitsQueue.ToList().IndexOf(_unit);
            CommandService.OnAnyQueueChanged?.Invoke(_unit);
            Vector3 position = _firstInQueuePosition + _queueDirection * _queuePosition;
            return position;
        }

        protected override void CommandExecutionComplete(Unit unit)
        {
            if (!CommandService.CurrentUnitsCommand.TryGetValue(unit, out var currentCommand) || currentCommand != this) return;
            _unit.MoveComponent.OnNavMeshReachDestination -= AgentReady;
            OnAnyCommandQueueChanged -= UpdateQueue;
            base.CommandExecutionComplete(unit);
        }

        public override string ToString()
        {
            return _queuePosition <= 0 ? "" : $"Staying in queue. Position = {_queuePosition}";
        }
    }
}