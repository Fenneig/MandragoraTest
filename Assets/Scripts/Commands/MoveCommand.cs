﻿using Mandragora.UnitBased;
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

        protected override void StartCommandExecution()
        {
            _unit.MoveComponent.IsAgentHavePath = true;
            _unit.MoveComponent.OnNavMeshReachDestination += CommandExecutionComplete;
            _unit.Agent.SetDestination(_destination);
        }

        protected override void CommandExecutionComplete()
        {
            _unit.MoveComponent.IsAgentHavePath = CommandsQueue.Count > 0;
            _unit.MoveComponent.OnNavMeshReachDestination -= CommandExecutionComplete;
            base.CommandExecutionComplete();
        }
    }
}