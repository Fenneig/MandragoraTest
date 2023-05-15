using System;
using Mandragora.Interactables;
using UnityEngine;

namespace Mandragora.UnitBased
{
    //TODO: Возможно будет хорошей идеей избавиться от синглтона и заинжектить 
    //TODO: систему в другой объект. На данный момент, думаю можно и так оставить.
    public class UnitActionSystem : MonoBehaviour
    {
        public static UnitActionSystem Instance { get; private set; }
        public event Action OnSelectedUnitChanged;
        
        [SerializeField] private LayerMask _unitLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private LayerMask _interactableLayerMask;

       
        private Camera _camera;
        private Unit _selectedUnit;
        
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

        private void Awake()
        {
            Instance ??= this;
            _camera = Camera.main;
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
            if (SelectedUnit == null) return;
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