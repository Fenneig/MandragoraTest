using Mandragora.Commands;
using Mandragora.Environment.Interactables;

namespace Mandragora.UnitBased
{
    public class InteractComponent
    {
        private Unit _unit;

        public InteractComponent(Unit unit)
        {
            _unit = unit;
        }

        public void Interact(IInteractable interactable, bool isQueueCommand)
        {
            var command = new InteractCommand(_unit, interactable);
            if (isQueueCommand) command.AddToQueue(_unit);
            else command.StartNewQueue(_unit);
        }
    }
}