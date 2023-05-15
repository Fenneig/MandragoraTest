using System;
using System.Collections.Generic;
using System.Linq;
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

        public QueueCommand(Unit unit, Queue<Unit> queue, Vector3 firstInQueuePosition, Vector3 queueDirection, string queueId)
        {
            _unit = unit;
            _unitsQueue = queue;
            _firstInQueuePosition = firstInQueuePosition;
            _queueDirection = queueDirection - firstInQueuePosition;
            _queueId = queueId;
            OnAnyCommandQueueChanged += UpdateQueue;
        }

        private void UpdateQueue(Queue<Unit> newQueue, string queueId)
        {
            if (_queueId != queueId) return;

            _unitsQueue = newQueue;
            MoveToNewPosition();
        }

        private void AgentReady(Unit unit)
        {
            if (_queuePosition == 0) CommandExecutionComplete(_unit);
        }

        public override void StartCommandExecution()
        {
            _unit.MoveComponent.OnNavMeshReachDestination += AgentReady;
            MoveToNewPosition();
            base.StartCommandExecution();
        }

        private void MoveToNewPosition()
        {
            if (!_unitsQueue.Contains(_unit))
            {
                ClearMethodsLinks();
                return;
            }

            Vector3 moveToPosition = GetQueuePosition();
            _unit.MoveComponent.IsAgentHavePath = true;
            _unit.Agent.SetDestination(moveToPosition);
        }

        private Vector3 GetQueuePosition()
        {
            _queuePosition = _unitsQueue.ToList().IndexOf(_unit);
            OnAnyQueueChanged?.Invoke(_unit);
            Vector3 position = _firstInQueuePosition + _queueDirection * _queuePosition;
            return position;
        }

        private void ClearMethodsLinks()
        {
            _unit.MoveComponent.OnNavMeshReachDestination -= AgentReady;
            OnAnyCommandQueueChanged -= UpdateQueue;
        }

        protected override void CommandExecutionComplete(Unit unit)
        {
            ClearMethodsLinks();
            base.CommandExecutionComplete(unit);
        }

        public override string ToString()
        {
            return _queuePosition == 0 ? "" : $"Stay in queue. Position = {_queuePosition}";
        }
    }
}