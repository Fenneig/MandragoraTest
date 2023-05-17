using System;
using UnityEngine;
using UnityEngine.AI;

namespace Mandragora.UnitBased
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour
    {
        [SerializeField, Header("Animator")] private Animator _manipulatorAnimator;
        [SerializeField] private Animator _visualAnimator;
        [SerializeField, Header("Rotate stats")] private float _rotateSpeed;
        
        private NavMeshAgent _agent;
        private MoveComponent _moveComponent;
        private RotateComponent _rotateComponent;
        private InteractComponent _interactComponent;
        private QueueComponent _queueComponent;
        public NavMeshAgent Agent => _agent;
        public MoveComponent MoveComponent => _moveComponent;
        public RotateComponent RotateComponent => _rotateComponent;
        public InteractComponent InteractComponent => _interactComponent;
        public QueueComponent QueueComponent => _queueComponent;
        public bool IsBusy { get; set; }
        public event Action OnUnitAnimationComplete;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _interactComponent = new InteractComponent(this);
            _queueComponent = new QueueComponent(this);
            _rotateComponent = new RotateComponent(this, _rotateSpeed);
            _moveComponent = new MoveComponent(this);
        }

        private void Update()
        {
            _moveComponent.CheckAgentDestination();
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
    }

    public enum AnimatorType
    {
        Manipulator,
        Visual
    }
}