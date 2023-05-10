using UnityEngine;
using UnityEngine.InputSystem;

namespace Mandragora
{
    public class MousePosition : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLM;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.value);
            if (Physics.Raycast(ray, out var hit, float.MaxValue, _groundLM))
            {
                transform.position = hit.point;
            }
        }
    }
}
