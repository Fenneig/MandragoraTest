using Mandragora.Interactables;
using Mandragora.UnitBased;

namespace Mandragora.Commands
{
    public class InteractCommand : BaseCommand
    {
        private Unit _unit;
        private IInteractable _interactable;

        public InteractCommand(Unit unit, IInteractable interactable)
        {
            _unit = unit;
            _interactable = interactable;
        }

        protected override void StartCommandExecution()
        {
            _interactable.Interact();
            _interactable.OnInteractionCompleted += CommandExecutionComplete;
        }

        protected override void CommandExecutionComplete()
        {
            _interactable.OnInteractionCompleted -= CommandExecutionComplete;
            base.CommandExecutionComplete();
        }
    }
}