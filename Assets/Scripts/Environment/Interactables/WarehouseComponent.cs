using Mandragora.Commands;
using Mandragora.UnitBased;
using Mandragora.Utils;

namespace Mandragora.Environment.Interactables
{
    public class WarehouseComponent : AbstractInteractable
    {
        public override string Name => $"Warehouse {gameObject.name}";

        public override void Interact()
        {
            _animator.SetTrigger(Idents.Animations.Interact);
        }

        public override void OnInteractionComplete()
        {
            CurrentUnitInteractWith = UnitsInQueue.Dequeue();
            QueueCommand.OnAnyCommandQueueChanged?.Invoke(UnitsInQueue, QueueId);
            CurrentUnitInteractWith.AnimationComponent.TriggerAnimation(Idents.Animations.TakeCargo, AnimatorType.Manipulator);
            base.OnInteractionComplete();
        }
    }
}