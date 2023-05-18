using Mandragora.Commands;
using Mandragora.UnitBased;
using Mandragora.Utils;

namespace Mandragora.Environment.Interactables
{
    public class FactoryComponent : AbstractInteractable
    {
        public override string Name => $"Factory {gameObject.name}";

        public override void Interact()
        {
            if (UnitsInQueue.Count <= 0)
            {
                base.OnInteractionComplete();
                return;
            }
            base.Interact();
            CurrentUnitInteractWith = UnitsInQueue.Dequeue();
            QueueCommand.OnAnyCommandQueueChanged?.Invoke(UnitsInQueue, QueueId);
            CurrentUnitInteractWith.AnimationComponent.OnUnitAnimationComplete += StartFactoryAnimation;
            CurrentUnitInteractWith.AnimationComponent.TriggerAnimation(Idents.Animations.DeliverCargo, AnimatorType.Manipulator);
        }

        private void StartFactoryAnimation()
        {
            _animator.SetTrigger(Idents.Animations.Interact);
            CurrentUnitInteractWith.AnimationComponent.OnUnitAnimationComplete -= StartFactoryAnimation;
        }
    }
}