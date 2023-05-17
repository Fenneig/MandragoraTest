using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Commands
{
    public class MoveCommand : BaseCommand
    {
        private Unit _unit;
        private Vector3 _destination;


        public MoveCommand(Unit unit, Vector3 destination)
        {
            _unit = unit;
            _destination = destination;
            _unit.MoveComponent.OnNavMeshReachDestination += CommandExecutionComplete;
        }

        protected override void StartCommandExecution()
        {
            _unit.MoveComponent.IsAgentHavePath = true;
            _unit.Agent.SetDestination(_destination);
            base.StartCommandExecution();
        }


        protected override void CommandExecutionComplete(Unit unit)
        {
            if (!CurrentUnitsCommand.TryGetValue(unit, out var currentCommand) || currentCommand != this) return;
            if (!_unit.MoveComponent.IsAgentReachDestination(_destination)) return;
            _unit.MoveComponent.OnNavMeshReachDestination -= CommandExecutionComplete;
            _unit.MoveComponent.IsAgentHavePath = false;
            base.CommandExecutionComplete(unit);
        }

        public override string ToString()
        {
            return $"Moving to {_destination.ToString()}";
        }
    }
}