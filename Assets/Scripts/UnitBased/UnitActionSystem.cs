using System;
using Mandragora.Environment.Interactables;
using Mandragora.Systems;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class UnitActionSystem : MonoBehaviour
    {
        public event Action OnSelectedUnitChanged;
        
        [SerializeField] private LayerMask _unitLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private LayerMask _interactableLayerMask;

        private Camera _camera;
        private Unit _selectedUnit;
        private AlertService _alertService;

        public Unit SelectedUnit
        {
            get => _selectedUnit;
            private set
            {
                _selectedUnit = value;
                OnSelectedUnitChanged?.Invoke();
            }
        }
        
        public bool IsAlternativeAction { get; set; }

        private void Start()
        {
            _camera = Camera.main;
            _alertService = GameManager.ServiceLocator.Get<AlertService>();
        }

        public void HandleUnitSelection(Vector3 mouseScreenPosition)
        {
            var ray = _camera.ScreenPointToRay(mouseScreenPosition);
            
            if (!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _unitLayerMask)) return;
            if (!hit.collider.gameObject.TryGetComponent(out Unit unit)) return;
            if (SelectedUnit == unit) return;

            SelectedUnit = unit;
        }

        public void HandleCommand(Vector3 mouseScreenPosition)
        {
            if (SelectedUnit == null || _alertService.IsAlert) return;
            var ray = _camera.ScreenPointToRay(mouseScreenPosition);
           
            if (Physics.Raycast(ray, out RaycastHit interactableHit, float.MaxValue, _interactableLayerMask))
            {
                interactableHit.collider.GetComponent<IInteractable>().StartInteractSequence(SelectedUnit, IsAlternativeAction);
                return;
            }
            
            if (!Physics.Raycast(ray, out RaycastHit groundHit, float.MaxValue, _groundLayerMask)) return;
            SelectedUnit.MoveComponent.Move(groundHit.point, IsAlternativeAction);
        }
    }
}