using System;
using System.Collections;
using Mandragora.Commands;
using Mandragora.Utils;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class RotateComponent
    {
        private Unit _unit;
        private float _rotateSpeed;

        private const float ROTATE_THRESHOLD = 2f;
        
        public event Action<Unit> OnNavMeshRotateReachDirection;

        public RotateComponent(Unit unit, float rotateSpeed)
        {
            _unit = unit;
            _rotateSpeed = rotateSpeed;
        }

        public void Rotate(Vector3 direction, bool isQueueCommand)
        {
            var command = new RotateCommand(this, direction);
            if (isQueueCommand) command.AddToQueue(_unit);
            else command.StartNewQueue(_unit);
        }
        
        public void LookAt(Vector3 direction)
        {
            Coroutines.StartRoutine(LookRoutine(direction));
        }

        private IEnumerator LookRoutine(Vector3 direction)
        {
            Quaternion lookAt = Quaternion.LookRotation(direction - _unit.transform.position);
            
            while (Mathf.Abs(_unit.transform.eulerAngles.y - lookAt.eulerAngles.y) > ROTATE_THRESHOLD &&
                   Mathf.Abs(_unit.transform.eulerAngles.y - lookAt.eulerAngles.y) - 360 <= -ROTATE_THRESHOLD)
            {
                _unit.transform.rotation = Quaternion.Slerp(_unit.transform.rotation, lookAt, _rotateSpeed * Time.deltaTime);
                yield return null;
            }

            _unit.transform.LookAt(direction);
            OnNavMeshRotateReachDirection?.Invoke(_unit);
        }
    }
}