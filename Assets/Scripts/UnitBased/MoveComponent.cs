using System;
using Mandragora.Commands;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class MoveComponent : MonoBehaviour
    {
        public bool IsAgentHavePath { get; set; }

        private Unit _unit;
        
        public event Action<Unit> OnNavMeshReachDestination;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        private void Update()
        {
            CheckAgentDestination();
        }

        private void CheckAgentDestination()
        {
            if (!IsAgentHavePath) return;
            if (_unit.Agent.pathPending) return;
            if (_unit.Agent.remainingDistance > _unit.Agent.stoppingDistance) return;
            if (_unit.Agent.hasPath && _unit.Agent.velocity.sqrMagnitude != 0f) return;
            OnNavMeshReachDestination?.Invoke(_unit);
        }

        public void Move(Vector3 destination, bool isQueueCommand)
        {
            var command = new MoveCommand(_unit, destination);
            if (isQueueCommand) command.AddToQueue(_unit);
            else command.StartNewQueue(_unit);
        }
    }
}