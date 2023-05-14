using Mandragora.Commands;
using Mandragora.Interactables;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class InteractComponent : MonoBehaviour
    {
        private Unit _unit;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        public void Interact(IInteractable interactable, bool isQueueCommand)
        {
            var command = new InteractCommand(interactable);
            if (isQueueCommand) command.AddToQueue(_unit);
            else command.StartNewQueue(_unit);
        }
    }
}