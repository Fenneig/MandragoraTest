using System;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private MeshRenderer _selectionMeshRenderer;

        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        }

        private void UnitActionSystem_OnSelectedUnitChanged() 
            => UpdateVisual();

        private void UpdateVisual()
        {
            _selectionMeshRenderer.enabled = UnitActionSystem.Instance.SelectedUnit == _unit;
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        }
    }
}