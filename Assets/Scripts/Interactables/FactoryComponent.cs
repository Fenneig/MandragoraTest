using Mandragora.Commands;
using Mandragora.UnitBased;
using Mandragora.Utils;

namespace Mandragora.Interactables
{
    public class FactoryComponent : AbstractInteractable
    {
        public override string Name => $"Factory {gameObject.name}";

        public override void Interact()
        {
            CurrentUnitInteractWith = UnitsInQueue.Dequeue();
            QueueCommand.OnAnyCommandQueueChanged?.Invoke(UnitsInQueue, QueueId);
            CurrentUnitInteractWith.OnUnitAnimationComplete += StartFactoryAnimation;
            CurrentUnitInteractWith.TriggerAnimation(Idents.Animations.DeliverCargo, AnimatorType.Manipulator);
        }

        private void StartFactoryAnimation()
        {
            _animator.SetTrigger(Idents.Animations.Interact);
            CurrentUnitInteractWith.OnUnitAnimationComplete -= StartFactoryAnimation;
        }
    }
}