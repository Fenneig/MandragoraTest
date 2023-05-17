using System;
using Mandragora.Commands;
using Mandragora.Utils;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class MoveComponent 
    {
        private Unit _unit;
        
        private const float THRESHOLD_REACH_POSITION = 1f;
        private bool _isAgentHavePath;

        public bool IsAgentHavePath
        {
            get => _isAgentHavePath;
            set
            {
                _isAgentHavePath = value;
                _unit.SetBoolAnimation(Idents.Animations.Moving, _isAgentHavePath, AnimatorType.Visual);
            }
        }

        public event Action<Unit> OnNavMeshReachDestination;

        public MoveComponent(Unit unit)
        {
            _unit = unit;
        }

        public bool IsAgentReachDestination(Vector3 destination) =>
            Mathf.Abs(_unit.transform.position.magnitude - destination.magnitude) <= THRESHOLD_REACH_POSITION;
        
        public void CheckAgentDestination()
        {
            if (!IsAgentHavePath) return;
            if (_unit.Agent.pathPending) return;
            if (_unit.Agent.remainingDistance > _unit.Agent.stoppingDistance) return;
            if (_unit.Agent.hasPath && _unit.Agent.velocity.sqrMagnitude != 0f) return;
            OnNavMeshReachDestination?.Invoke(_unit);
        }

        public void Move(Vector3 destination, bool isQueueCommand)
        {
            var command = new MoveCommand(_unit, destination);
            if (isQueueCommand) command.AddToQueue(_unit);
            else command.StartNewQueue(_unit);
        }
    }
}