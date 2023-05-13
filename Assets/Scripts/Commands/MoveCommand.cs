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
        }

        public override void StartCommandExecution()
        {
            _unit.MoveComponent.IsAgentHavePath = true;
            _unit.MoveComponent.OnNavMeshReachDestination += CommandExecutionComplete;
            _unit.Agent.SetDestination(_destination);
        }

        protected override void CommandExecutionComplete(Unit unit)
        {
            _unit.MoveComponent.OnNavMeshReachDestination -= CommandExecutionComplete;
            base.CommandExecutionComplete(unit);
        }
    }
}