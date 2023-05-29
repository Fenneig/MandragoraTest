using Mandragora.Systems;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private MeshRenderer _selectionMeshRenderer;

        private UnitActionSystem _unitActionSystem;

        private void Start()
        {
            _unitActionSystem = GameManager.ServiceLocator.Get<UnitActionSystem>();
            _unitActionSystem.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        }

        private void UnitActionSystem_OnSelectedUnitChanged() 
            => UpdateVisual();

        private void UpdateVisual()
        {
            _selectionMeshRenderer.enabled = _unitActionSystem.SelectedUnit == _unit;
        }

        private void OnDestroy()
        {
            _unitActionSystem.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        }
    }
}