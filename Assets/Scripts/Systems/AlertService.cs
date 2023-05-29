using System;
using Mandragora.Services;

namespace Mandragora.Systems
{
    public class AlertService : IService
    {
        public bool IsAlert { get; private set; }

        public event Action<bool> OnAlertStateChanged;
        
        public void SetAlertState(bool alertState)
        {
            IsAlert = alertState;
            OnAlertStateChanged?.Invoke(alertState);
        }
    }
}