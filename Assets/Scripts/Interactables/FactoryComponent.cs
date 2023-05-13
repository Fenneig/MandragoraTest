﻿using Mandragora.Commands;
using Mandragora.Utils;

namespace Mandragora.Interactables
{
    public class FactoryComponent : AbstractInteractable
    {
        
        public override void Interact()
        {
            CurrentUnitInteractWith = UnitsInQueue.Dequeue();
            QueueCommand.OnAnyQueueChanged?.Invoke(UnitsInQueue);
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