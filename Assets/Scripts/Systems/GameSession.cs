using System;
using System.Collections.Generic;
using Mandragora.Commands;
using Mandragora.Environment;
using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Systems
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private UnitActionSystem _unitActionSystem;
        [SerializeField] private List<HangarComponent> _sceneHangars;

        public UnitActionSystem UnitActionSystem => _unitActionSystem;
        public bool IsAlert { get; private set; }

        public static GameSession Instance;
        public event Action<bool> OnAlertStateChanged;

        private void Awake()
        {
            Instance ??= this;
        }

        public void SetAlertState(bool alertState)
        {
            IsAlert = alertState;
            BaseCommand.SetAlertState(alertState);
            OnAlertStateChanged?.Invoke(alertState);
            foreach (var hangar in _sceneHangars)
            {
                hangar.SetAlertState(alertState);
            }
        }
    }
}