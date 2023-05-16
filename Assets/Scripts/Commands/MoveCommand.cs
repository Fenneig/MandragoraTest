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
            OnAnyActionCanceled += CheckIsThisCommandCanceled;
            base.StartCommandExecution();
        }

        private void CheckIsThisCommandCanceled(Unit unit)
        {
            if (CurrentUnitsCommand[unit] == this)
            {
                ClearMethodsLinks();
                _unit.Agent.SetDestination(_unit.transform.position);
                _unit.MoveComponent.IsAgentHavePath = false;
            }
        }

        private void ClearMethodsLinks()
        {           
            _unit.MoveComponent.OnNavMeshReachDestination -= CommandExecutionComplete;
            OnAnyActionCanceled -= CheckIsThisCommandCanceled;
        }

        protected override void CommandExecutionComplete(Unit unit)
        {
            ClearMethodsLinks();
            _unit.MoveComponent.IsAgentHavePath = false;
            if (!_isCommandCanceled) base.CommandExecutionComplete(unit);
        }

        public override string ToString()
        {
            return $"Moving to {_destination.ToString()}";
        }
    }
}