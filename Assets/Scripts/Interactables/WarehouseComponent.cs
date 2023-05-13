using Mandragora.Commands;
using Mandragora.Utils;

namespace Mandragora.Interactables
{
    public class WarehouseComponent : AbstractInteractable
    {
       public override void Interact()
        {
            _animator.SetTrigger(Idents.Animations.Interact);
        }

        public override void OnInteractionComplete()
        {
            CurrentUnitInteractWith = UnitsInQueue.Dequeue();
            QueueCommand.OnAnyQueueChanged?.Invoke(UnitsInQueue);
            CurrentUnitInteractWith.TriggerAnimation(Idents.Animations.TakeCargo);
            base.OnInteractionComplete();
        }
    }
}