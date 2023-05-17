using Mandragora.Environment.Interactables;
using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Commands
{
    public class InteractCommand : BaseCommand
    {
        private IInteractable _interactable;

        public InteractCommand(IInteractable interactable)
        {
            _interactable = interactable;
        }

        protected override void StartCommandExecution()
        {
            _interactable.Interact();
            _interactable.OnInteractionCompleted += CommandExecutionComplete;
            base.StartCommandExecution();
        }

        protected override void CommandExecutionComplete(Unit unit)
        {
            if (!CurrentUnitsCommand.TryGetValue(unit, out var currentCommand) || currentCommand != this) return;
            _interactable.OnInteractionCompleted -= CommandExecutionComplete;
            base.CommandExecutionComplete(unit);
        }

        public override string ToString()
        {
            return $"Interact with {_interactable.Name}";
        }
    }
}