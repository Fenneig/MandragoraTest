using System;
using System.Collections;
using Mandragora.Commands;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class RotateComponent : MonoBehaviour
    {
        [SerializeField] private float _rotateSpeed;
        
        private const float ROTATE_THRESHOLD = 3f;
        
        public event Action OnNavMeshRotateReachDirection;

        public void Rotate(Vector3 direction, bool isQueueCommand)
        {
            if (isQueueCommand) new RotateCommand(this, direction).AddToQueue();
            else new RotateCommand(this, direction).StartNewQueue();
        }
        
        public void LookAt(Vector3 direction)
        {
            StartCoroutine(LookRoutine(direction));
        }

        private IEnumerator LookRoutine(Vector3 direction)
        {
            Quaternion lookTo = Quaternion.LookRotation(direction - transform.position);
            
            while (Mathf.Abs(transform.eulerAngles.y - lookTo.eulerAngles.y) > ROTATE_THRESHOLD)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, lookTo, _rotateSpeed * Time.deltaTime);
                yield return null;
            }
            
            OnNavMeshRotateReachDirection?.Invoke();
        }
    }
}