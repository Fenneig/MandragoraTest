using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Commands
{
    public class MoveCommand : BaseCommand
    {
        private Unit _unit;
        private Vector3 _destination;
        private bool _isCommandCanceled;

        public MoveCommand(Unit unit, Vector3 destination)
        {
            _unit = unit;
            _destination = destination;
        }

        public override void StartCommandExecution()
        {
            _unit.MoveComponent.IsAgentHavePath = true;
            _unit.MoveComponent.OnNavMeshReachDestination += CommandExecutionComplete;
            _unit.Agent.SetDestination(_destination);
            OnAnyActionCanceled += CommandCanceled;
            base.StartCommandExecution();
        }

        private void CommandCanceled(Unit _)
        {
            _isCommandCanceled = true;
        }

        protected override void CommandExecutionComplete(Unit unit)
        {
            _unit.MoveComponent.OnNavMeshReachDestination -= CommandExecutionComplete;
            _unit.MoveComponent.IsAgentHavePath = false;
            OnAnyActionCanceled -= CommandCanceled;
            if (!_isCommandCanceled) base.CommandExecutionComplete(unit);
        }

        public override string ToString()
        {
            return $"Moving to {_destination.ToString()}";
        }
    }
}