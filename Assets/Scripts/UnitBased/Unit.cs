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
        [SerializeField, Header("Rotate stats"), Space] private float _rotateSpeed;
        
        private NavMeshAgent _agent;
        private MoveComponent _moveComponent;
        private RotateComponent _rotateComponent;
        private InteractComponent _interactComponent;
        private QueueComponent _queueComponent;
        private AnimationComponent _animationComponent;
        public NavMeshAgent Agent => _agent;
        public MoveComponent MoveComponent => _moveComponent;
        public RotateComponent RotateComponent => _rotateComponent;
        public InteractComponent InteractComponent => _interactComponent;
        public QueueComponent QueueComponent => _queueComponent;
        public AnimationComponent AnimationComponent => _animationComponent;
        public bool IsBusy { get; set; }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _interactComponent = new InteractComponent(this);
            _queueComponent = new QueueComponent(this);
            _rotateComponent = new RotateComponent(this, _rotateSpeed);
            _moveComponent = new MoveComponent(this);
            _animationComponent = new AnimationComponent(_manipulatorAnimator, _visualAnimator);
        }

        private void Update()
        {
            _moveComponent.CheckAgentDestination();
        }
        
        private void AnimationComplete()
        {
            _animationComponent.AnimationComplete();
        }
    }

    public enum AnimatorType
    {
        Manipulator,
        Visual
    }
}