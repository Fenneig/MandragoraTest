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
        private NavMeshAgent _agent;
        private MoveComponent _moveComponent;
        private RotateComponent _rotateComponent;
        public NavMeshAgent Agent => _agent;
        public MoveComponent MoveComponent => _moveComponent;
        public RotateComponent RotateComponent => _rotateComponent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _moveComponent = GetComponent<MoveComponent>();
            _rotateComponent = GetComponent<RotateComponent>();
        }

        public void Interact(IInteractable interactable, bool isQueueCommand, InteractType interactType)
        {
            if (isQueueCommand) new InteractCommand(this, interactable).AddToQueue();
            else new InteractCommand(this, interactable).StartNewQueue();
        }
    }

    public enum InteractType
    {
        DeliverCargo,
        TakeCargo
    }
}