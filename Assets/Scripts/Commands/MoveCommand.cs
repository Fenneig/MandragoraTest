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
            OnAnyActionCanceled += CheckIsThisCommandCanceled;
        }

        protected override void StartCommandExecution()
        {
            _isCommandCanceled = false;
            _unit.MoveComponent.IsAgentHavePath = true;
            _unit.MoveComponent.OnNavMeshReachDestination += CommandExecutionComplete;
            _unit.Agent.SetDestination(_destination);
            base.StartCommandExecution();
        }

        private void CheckIsThisCommandCanceled(Unit unit)
        {
            if (CurrentUnitsCommand[unit] == this)
            {
                _isCommandCanceled = true;
                _unit.Agent.SetDestination(_unit.transform.position);
                _unit.MoveComponent.IsAgentHavePath = false;
            }
        }


        protected override void CommandExecutionComplete(Unit unit)
        {
            if (!_isCommandCanceled) return;
            _unit.MoveComponent.OnNavMeshReachDestination -= CommandExecutionComplete;
            OnAnyActionCanceled -= CheckIsThisCommandCanceled;
            _unit.MoveComponent.IsAgentHavePath = false;
            base.CommandExecutionComplete(unit);
        }

        public override string ToString()
        {
            return $"Moving to {_destination.ToString()}";
        }
    }
}