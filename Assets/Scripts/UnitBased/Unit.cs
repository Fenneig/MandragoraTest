using System;
using Mandragora.Commands;
using UnityEngine;
using UnityEngine.AI;

namespace Mandragora.UnitBased
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        public NavMeshAgent Agent => _agent;

        public event Action OnNavMeshReachDestination;

        public bool IsAgentHavePath { get; set; }

        private void Update()
        {
            CheckAgentDestination();
        }

        private void CheckAgentDestination()
        {
            if (!IsAgentHavePath) return;
            if (_agent.pathPending) return;
            if (_agent.remainingDistance > _agent.stoppingDistance) return;
            if (_agent.hasPath && _agent.velocity.sqrMagnitude != 0f) return;
            OnNavMeshReachDestination?.Invoke();
        }

        public void Move(Vector3 destination, bool isQueueCommand)
        {
            if (isQueueCommand) new MoveCommand(this, destination).AddToQueue();
            else new MoveCommand(this, destination).StartNewQueue();
        }
    }
}