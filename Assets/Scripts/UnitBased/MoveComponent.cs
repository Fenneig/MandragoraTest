﻿using System;
using Mandragora.Commands;
using UnityEngine;

namespace Mandragora.UnitBased
{
    [RequireComponent(typeof(Unit))]
    public class MoveComponent : MonoBehaviour
    {
        public bool IsAgentHavePath { get; set; }

        private Unit _unit;
        
        public event Action OnNavMeshReachDestination;

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
            OnNavMeshReachDestination?.Invoke();
        }

        public void Move(Vector3 destination, bool isQueueCommand)
        {
            if (isQueueCommand) new MoveCommand(_unit, destination).AddToQueue();
            else new MoveCommand(_unit, destination).StartNewQueue();
        }
    }
}