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
            base.Interact();
        }

        protected override void OnInteractionComplete()
        {
            if (UnitsInQueue.Count <= 0)
            {
                base.OnInteractionComplete();
                return;
            }
            CurrentUnitInteractWith = UnitsInQueue.Dequeue();
            QueueCommand.OnAnyCommandQueueChanged?.Invoke(UnitsInQueue, QueueId);
            CurrentUnitInteractWith.AnimationComponent.TriggerAnimation(Idents.Animations.TakeCargo, AnimatorType.Manipulator);
            base.OnInteractionComplete();
        }
    }
}