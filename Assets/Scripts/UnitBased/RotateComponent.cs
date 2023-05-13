using System;
using System.Collections;
using Mandragora.Commands;
using UnityEngine;

namespace Mandragora.UnitBased
{
    [RequireComponent(typeof(Unit))]
    public class RotateComponent : MonoBehaviour
    {
        [SerializeField] private float _rotateSpeed;

        private Unit _unit;
        
        private const float ROTATE_THRESHOLD = 2f;
        
        public event Action<Unit> OnNavMeshRotateReachDirection;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        public void Rotate(Vector3 direction, bool isQueueCommand)
        {
            var command = new RotateCommand(this, direction);
            if (isQueueCommand) command.AddToQueue(_unit);
            else command.StartNewQueue(_unit);
        }
        
        public void LookAt(Vector3 direction)
        {
            StartCoroutine(LookRoutine(direction));
        }

        private IEnumerator LookRoutine(Vector3 direction)
        {
            Quaternion lookAt = Quaternion.LookRotation(direction - transform.position);
            
            while (Mathf.Abs(transform.eulerAngles.y - lookAt.eulerAngles.y) > ROTATE_THRESHOLD &&
                   Mathf.Abs(transform.eulerAngles.y - lookAt.eulerAngles.y) - 360 <= -ROTATE_THRESHOLD)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, _rotateSpeed * Time.deltaTime);
                yield return null;
            }

            transform.LookAt(direction);
            OnNavMeshRotateReachDirection?.Invoke(_unit);
        }
    }
}