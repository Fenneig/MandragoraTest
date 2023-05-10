using Cinemachine;
using UnityEngine;

namespace Mandragora.Camera
{
    public class CameraInputController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _minFollowYOffset = 1f;
        [SerializeField] private float _maxFollowYOffset = 12f;
        [SerializeField] private float _cameraMoveSpeed = 10f;
        [SerializeField] private float _cameraZoomSpeed = 5f;

        private CinemachineTransposer _cinemachineTransposer;
        private Vector3 _targetFollowOffset;

        public Vector2 CameraMoveDirection { get; set; }
        public float CameraZoomAmount { get; set; }

        private void Awake()
        {
            _cinemachineTransposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
            _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
        }

        private void Update()
        {
            HandleMovement();
            HandleZoom();
        }

        private void HandleMovement()
        {
            Vector3 moveVector = transform.forward * CameraMoveDirection.y + transform.right * CameraMoveDirection.x;
            transform.position += moveVector * _cameraMoveSpeed * Time.deltaTime;
        }

        private void HandleZoom()
        {
            var newFollowOffset = _targetFollowOffset.y + CameraZoomAmount;
            _targetFollowOffset.y = Mathf.Lerp(_targetFollowOffset.y, newFollowOffset, _cameraZoomSpeed * Time.deltaTime);
            _targetFollowOffset.y = Mathf.Clamp(newFollowOffset, _minFollowYOffset, _maxFollowYOffset);

            _cinemachineTransposer.m_FollowOffset = 
                Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset, _cameraZoomSpeed * Time.deltaTime);
        }
    }
}