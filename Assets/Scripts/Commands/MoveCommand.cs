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
            _unit.IsAgentHavePath = true;
            _unit.OnNavMeshReachDestination += CommandExecutionComplete;
            _unit.Agent.SetDestination(_destination);
        }

        public override void CommandExecutionComplete()
        {
            _unit.IsAgentHavePath = CommandsQueue.Count > 0;
            _unit.OnNavMeshReachDestination -= CommandExecutionComplete;
            base.CommandExecutionComplete();
        }
    }
}