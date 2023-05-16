using Mandragora.CameraBased;
using Mandragora.Systems;
using Mandragora.UnitBased;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mandragora.Input
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private CameraInputController _inputController;

        private UnitActionSystem _unitActionSystem;

        private void Start()
        {
            _unitActionSystem = GameSession.Instance.UnitActionSystem;
        }

        public void OnCameraMovement(InputAction.CallbackContext context) =>
            _inputController.CameraMoveDirection = context.ReadValue<Vector2>();

        public void OnCameraZoom(InputAction.CallbackContext context) =>
            _inputController.CameraZoomAmount = context.ReadValue<float>();

        public void OnSelect(InputAction.CallbackContext context)
        {
            if (context.started) _unitActionSystem.HandleUnitSelection(Mouse.current.position.ReadValue());
        }

        public void OnCommand(InputAction.CallbackContext context)
        {
            if (context.started) _unitActionSystem.HandleCommand(Mouse.current.position.ReadValue());
        }

        public void OnAlternativeAction(InputAction.CallbackContext context)
        {
            if (context.started) _unitActionSystem.IsAlternativeAction = true;
            if (context.canceled) _unitActionSystem.IsAlternativeAction = false;
        }
    }
}