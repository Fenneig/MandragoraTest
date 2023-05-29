using Mandragora.Commands;
using Mandragora.Systems;
using Mandragora.UnitBased;
using Mandragora.Utils;
using UnityEngine;

namespace Mandragora.Environment
{
    public class HangarComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Unit _linkedUnit;
        [SerializeField] private Transform _unitStayPosition;
        [SerializeField] private Transform _unitLookAtPosition;

        private bool _isGateClosed;
        private bool _isAlert;
        private bool _isUnitInSafePosition;
        private AlertService _alertService;
        private CommandService _commandService;

        private void Start()
        {
            GetServices();
            _linkedUnit.MoveComponent.OnNavMeshReachDestination += UnitReachSafePosition;
            _alertService.OnAlertStateChanged += SetAlertState;
        }

        private void GetServices()
        {
            _alertService = GameManager.ServiceLocator.Get<AlertService>();
            _commandService = GameManager.ServiceLocator.Get<CommandService>();
        }

        private void UnitReachSafePosition(Unit unit)
        {
            if (_isAlert && unit == _linkedUnit)
            {
                _linkedUnit.MoveComponent.IsAgentHavePath = false;
                _linkedUnit.RotateComponent.LookAt(_unitLookAtPosition.position);
                _animator.SetBool(Idents.Animations.HangarDoorIsClosed, true);
                _isUnitInSafePosition = true;
            }
        }

        private void GateOpened()
        {
            if (_isUnitInSafePosition)
            {
                _commandService.ReadFromStashUnitCommandLines(_linkedUnit);
            }

            _isGateClosed = false;
        }

        private void GateClosed()
        {
            _isGateClosed = true;
        }

        public void SetAlertState(bool alertState)
        {
            _isAlert = alertState;

            if (_isAlert)
            {
                _commandService.StashUnitCommandLines(_linkedUnit);
                _linkedUnit.Agent.SetDestination(_unitStayPosition.position);
                _linkedUnit.MoveComponent.IsAgentHavePath = true;
                _isUnitInSafePosition = false;
            }
            else
            {
                if (_isGateClosed)
                {
                    _animator.SetBool(Idents.Animations.HangarDoorIsClosed, false);
                }
                else
                {
                    _commandService.ReadFromStashUnitCommandLines(_linkedUnit);
                }
            }
        }

        private void OnDestroy()
        {
            _linkedUnit.MoveComponent.OnNavMeshReachDestination -= UnitReachSafePosition;
            _alertService.OnAlertStateChanged -= SetAlertState;
        }
    }
}