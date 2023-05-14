using System;
using Mandragora.Commands;
using Mandragora.Interactables;
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
        [SerializeField] private Animator _animator;
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

        public void TriggerAnimation(string animationTriggerParameter)
        {
            _animator.SetTrigger(animationTriggerParameter);
        }

        private void AnimationComplete()
        {
            OnUnitAnimationComplete?.Invoke();
        }
    }
}