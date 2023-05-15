using Mandragora.Interactables;
using Mandragora.UnitBased;

namespace Mandragora.Commands
{
    public class InteractCommand : BaseCommand
    {
        private IInteractable _interactable;

        public InteractCommand(IInteractable interactable)
        {
            _interactable = interactable;
        }

        public override void StartCommandExecution()
        {
            _interactable.Interact();
            _interactable.OnInteractionCompleted += CommandExecutionComplete;
            base.StartCommandExecution();
        }

        protected override void CommandExecutionComplete(Unit unit)
        {
            _interactable.OnInteractionCompleted -= CommandExecutionComplete;
            base.CommandExecutionComplete(unit);
        }

        public override string ToString()
        {
            return $"Interact with {_interactable.Name}";
        }
    }
}