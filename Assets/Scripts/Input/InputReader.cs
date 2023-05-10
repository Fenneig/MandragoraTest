using Mandragora.CameraBased;
using Mandragora.UnitBased;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mandragora.Input
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private CameraInputController _inputController;

        public void OnCameraMovement(InputAction.CallbackContext context) =>
            _inputController.CameraMoveDirection = context.ReadValue<Vector2>();

        public void OnCameraZoom(InputAction.CallbackContext context) =>
            _inputController.CameraZoomAmount = context.ReadValue<float>();

        public void OnSelect(InputAction.CallbackContext context)
        {
            if (context.started)
                UnitActionSystem.Instance.HandleUnitSelection(Mouse.current.position.ReadValue());
        }

        public void OnCommand(InputAction.CallbackContext context)
        {
            if (context.started)
                UnitActionSystem.Instance.HandleCommand(Mouse.current.position.ReadValue());
        }

        public void OnAlternativeAction(InputAction.CallbackContext context)
        {
            if (context.started) UnitActionSystem.Instance.IsAlternativeAction = true;
            if (context.canceled) UnitActionSystem.Instance.IsAlternativeAction = false;
        }
    }
}