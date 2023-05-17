using System;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class AnimationComponent
    {
        private Animator _manipulatorAnimator;
        private Animator _visualAnimator;

        public event Action OnUnitAnimationComplete;

        public AnimationComponent(Animator manipulatorAnimator, Animator visualAnimator)
        {
            _manipulatorAnimator = manipulatorAnimator;
            _visualAnimator = visualAnimator;
        }

        public void TriggerAnimation(string animationTriggerParameter, AnimatorType animatorType)
        {
            if (animatorType == AnimatorType.Manipulator) _manipulatorAnimator.SetTrigger(animationTriggerParameter);
            else _visualAnimator.SetTrigger(animationTriggerParameter);
        }

        public void SetBoolAnimation(string animationBoolParameter, bool state, AnimatorType animatorType)
        {
            if (animatorType == AnimatorType.Manipulator) _visualAnimator.SetBool(animationBoolParameter, state);
            else _visualAnimator.SetBool(animationBoolParameter, state);
        }

        public void AnimationComplete()
        {
            OnUnitAnimationComplete?.Invoke();
        }
    }
}