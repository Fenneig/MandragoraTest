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
        
        public static Action OnAnyQueueChanged;

        public QueueCommand(Unit unit, Queue<Unit> queue, Vector3 firstInQueuePosition, Vector3 queueDirection)
        {
            _unit = unit;
            _unitsQueue = queue;
            _firstInQueuePosition = firstInQueuePosition;
            _queueDirection = queueDirection - firstInQueuePosition;
            _unit.MoveComponent.OnNavMeshReachDestination += AgentReady;
            OnAnyQueueChanged += MoveToNewPosition;
        }

        private void AgentReady(Unit unit)
        {
            if (_queuePosition == 0) CommandExecutionComplete(_unit);
        }

        public override void StartCommandExecution()
        {
            MoveToNewPosition();
        }

        private void MoveToNewPosition()
        {
            if (!_unitsQueue.Contains(_unit))
            {
                ClearMethodsLinks();
                return;
            }
            Vector3 moveToPosition = GetQueuePosition();
            _unit.Agent.SetDestination(moveToPosition);
            _unit.MoveComponent.IsAgentHavePath = true;
        }

        private Vector3 GetQueuePosition()
        {
            _queuePosition = _unitsQueue.ToList().IndexOf(_unit);
            Vector3 position = _firstInQueuePosition + _queueDirection * _queuePosition;
            return position;
        }

        private void ClearMethodsLinks()
        {
            _unit.MoveComponent.OnNavMeshReachDestination -= AgentReady;
            OnAnyQueueChanged -= MoveToNewPosition;
        }

        protected override void CommandExecutionComplete(Unit unit)
        {
            ClearMethodsLinks();
            base.CommandExecutionComplete(unit);
        }
    }
}