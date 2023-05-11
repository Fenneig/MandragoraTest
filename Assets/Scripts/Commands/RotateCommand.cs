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

        protected override void StartCommandExecution()
        {
            _unitRotateComponent.OnNavMeshRotateReachDirection += CommandExecutionComplete;
            _unitRotateComponent.LookAt(_direction);
        }

        protected override void CommandExecutionComplete()
        {
            _unitRotateComponent.OnNavMeshRotateReachDirection -= CommandExecutionComplete;
            base.CommandExecutionComplete();
        }
    }
}