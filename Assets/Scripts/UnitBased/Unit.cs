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
    public class Unit : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] private Animator _animator;
        private NavMeshAgent _agent;
        private MoveComponent _moveComponent;
        private RotateComponent _rotateComponent;
        private IInteractable _interactWith;
        private bool _isQueueCommand;
        public NavMeshAgent Agent => _agent;
        public MoveComponent MoveComponent => _moveComponent;
        public RotateComponent RotateComponent => _rotateComponent;

        public event Action OnAnimationComplete;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _moveComponent = GetComponent<MoveComponent>();
            _rotateComponent = GetComponent<RotateComponent>();
        }

        public void Interact(IInteractable interactable, bool isQueueCommand)
        {
            _interactWith = interactable;
            _isQueueCommand = isQueueCommand;

            CreateInteractionCommand();
        }

        public void CreateInteractionCommand()
        {
            if (_isQueueCommand) new InteractCommand(_interactWith).AddToQueue(this);
            else new InteractCommand(_interactWith).StartNewQueue(this);
        }

        public void TriggerAnimation(string animationTriggerParameter)
        {
            _animator.SetTrigger(animationTriggerParameter);
        }

        private void AnimationComplete()
        {
            OnAnimationComplete?.Invoke();
        }
    }
}