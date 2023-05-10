using System;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class UnitActionSystem : MonoBehaviour
    {
        public static UnitActionSystem Instance { get; private set; }
        public event Action OnSelectedUnitChanged;
        
        [SerializeField] private LayerMask _unitLayerMask;
       
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
    }
}