using System;
using Mandragora.Commands;
using Mandragora.Utils;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class MoveComponent 
    {
        private Unit _unit;
        
        public bool IsAgentHavePath { get; set; }

        public event Action<Unit> OnNavMeshReachDestination;

        public MoveComponent(Unit unit)
        {
            _unit = unit;
        }
        

        public void CheckAgentDestination()
        {
            if (!IsAgentHavePath) return;
            if (_unit.Agent.pathPending) return;
            if (_unit.Agent.remainingDistance > _unit.Agent.stoppingDistance) return;
            if (_unit.Agent.hasPath && _unit.Agent.velocity.sqrMagnitude != 0f) return;
            OnNavMeshReachDestination?.Invoke(_unit);
            _unit.SetBoolAnimation(Idents.Animations.Moving, false, AnimatorType.Visual);
        }

        public void Move(Vector3 destination, bool isQueueCommand)
        {
            var command = new MoveCommand(_unit, destination);
            if (isQueueCommand) command.AddToQueue(_unit);
            else command.StartNewQueue(_unit);
            _unit.SetBoolAnimation(Idents.Animations.Moving, true, AnimatorType.Visual);
        }
    }
}