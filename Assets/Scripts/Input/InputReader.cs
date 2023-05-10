using Mandragora.Camera;
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
            Debug.Log("Select");
        }

        public void OnCommand(InputAction.CallbackContext context)
        {
            Debug.Log("Command");
        }
    }
}