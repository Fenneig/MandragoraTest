using System;
using Mandragora.Commands;
using UnityEngine;
using UnityEngine.AI;

namespace Mandragora.UnitBased
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(MoveComponent))]
    [RequireComponent(typeof(RotateComponent))]
    [RequireComponent(typeof(InteractComponent))]
    public class Unit : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] private Animator _manipulatorAnimator;
        [SerializeField] private Animator _visualAnimator;
        private NavMeshAgent _agent;
        private MoveComponent _moveComponent;
        private RotateComponent _rotateComponent;
        private InteractComponent _interactComponent;
        public NavMeshAgent Agent => _agent;
        public MoveComponent MoveComponent => _moveComponent;
        public RotateComponent RotateComponent => _rotateComponent;
        public InteractComponent InteractComponent => _interactComponent;
        public bool IsBusy { get; set; }
        public event Action OnUnitAnimationComplete;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _moveComponent = GetComponent<MoveComponent>();
            _rotateComponent = GetComponent<RotateComponent>();
            _interactComponent = GetComponent<InteractComponent>();
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

        private void AnimationComplete()
        {
            OnUnitAnimationComplete?.Invoke();
        }

        public override string ToString()
        {
            return "";
        }

        [ContextMenu("Play command")]
        public void PlayCommand()
        {
            BaseCommand.PlayCommandFromQueue(this);
        }
    }

    public enum AnimatorType
    {
        Manipulator,
        Visual
    }
}