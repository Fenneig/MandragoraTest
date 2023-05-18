using Mandragora.Environment.Interactables;
using Mandragora.Systems;
using Mandragora.UnitBased;

namespace Mandragora.Commands
{
    public class InteractCommand : BaseCommand
    {
        private Unit _unit;
        private IInteractable _interactable;
        private bool _unitReachInteractPosition;
        private bool _isInteractionComplete;

        public InteractCommand(Unit unit, IInteractable interactable)
        {
            _unit = unit;
            _interactable = interactable;
        }

        private void StartRotate(Unit unit)
        {
            if (unit != _unit || _unitReachInteractPosition) return;
            unit.RotateComponent.LookAt(_interactable.InteractLookAtPosition);
            _unitReachInteractPosition = true;
        }

        private void StartInteraction(Unit unit)
        {
            if (unit != _unit || _interactable.InteractionInProgress) return;
            _interactable.Interact();
        }

        protected override void StartCommandExecution()
        {
            if (_isInteractionComplete) base.CommandExecutionComplete(_unit);
            _unitReachInteractPosition = false;
            AddMethodsLinks();
            _unit.Agent.SetDestination(_interactable.InteractPosition);
            _unit.MoveComponent.IsAgentHavePath = true;
        }

        private void AddMethodsLinks()
        {
            _unit.MoveComponent.OnNavMeshReachDestination += StartRotate;
            _unit.RotateComponent.OnNavMeshRotateReachDirection += StartInteraction;
            _interactable.OnInteractionCompleted += CommandExecutionComplete;
            OnAnyActionCanceled += ClearMethodsLinks;
        }

        private void ClearMethodsLinks(Unit unit)
        {
            if (unit != _unit) return;
            _unit.MoveComponent.OnNavMeshReachDestination -= StartRotate;
            _unit.RotateComponent.OnNavMeshRotateReachDirection -= StartInteraction;
            _interactable.OnInteractionCompleted -= CommandExecutionComplete;
            OnAnyActionCanceled -= ClearMethodsLinks;
        }

        protected override void CommandExecutionComplete(Unit unit)
        {
            if (unit != _unit || GameSession.Instance.IsAlert) return;
            if (!CurrentUnitsCommand.TryGetValue(unit, out var currentCommand) || currentCommand != this) return;
            ClearMethodsLinks(unit);
            _isInteractionComplete = true;
            base.CommandExecutionComplete(unit);
        }

        public override string ToString()
        {
            return $"Interact with {_interactable.Name}";
        }
    }
}