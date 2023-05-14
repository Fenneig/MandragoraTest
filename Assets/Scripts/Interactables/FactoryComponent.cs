using Mandragora.Commands;
using Mandragora.Utils;

namespace Mandragora.Interactables
{
    public class FactoryComponent : AbstractInteractable
    {
        public override string Name => $"Factory {gameObject.name}";

        public override void Interact()
        {
            CurrentUnitInteractWith = UnitsInQueue.Dequeue();
            QueueCommand.OnAnyQueueChanged?.Invoke(UnitsInQueue, QueueId);
            CurrentUnitInteractWith.OnUnitAnimationComplete += StartFactoryAnimation;
            CurrentUnitInteractWith.TriggerAnimation(Idents.Animations.DeliverCargo);
        }

        private void StartFactoryAnimation()
        {
            _animator.SetTrigger(Idents.Animations.Interact);
            CurrentUnitInteractWith.OnUnitAnimationComplete -= StartFactoryAnimation;
        }
    }
}