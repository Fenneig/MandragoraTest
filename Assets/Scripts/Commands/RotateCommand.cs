using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Commands
{
    public class RotateCommand : BaseCommand
    {
        private Vector3 _direction;
        private RotateComponent _unitRotateComponent;
        
        public RotateCommand(RotateComponent unitRotateComponent, Vector3 direction)
        {
            _unitRotateComponent = unitRotateComponent;
            _direction = direction;
        }

        public override void StartCommandExecution()
        {
            _unitRotateComponent.OnNavMeshRotateReachDirection += CommandExecutionComplete;
            _unitRotateComponent.LookAt(_direction);
            base.StartCommandExecution();
        }

        protected override void CommandExecutionComplete(Unit unit)
        {
            _unitRotateComponent.OnNavMeshRotateReachDirection -= CommandExecutionComplete;
            base.CommandExecutionComplete(unit);
        }
    }
}