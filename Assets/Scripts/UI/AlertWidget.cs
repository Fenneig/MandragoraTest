using Mandragora.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Mandragora.UI
{
    public class AlertWidget : MonoBehaviour
    {
        [SerializeField] private Button _alertButton;
        [SerializeField] private Sprite _alertSign;
        [SerializeField] private Sprite _releaseSign;
        private bool _isAlert;
        private AlertService _alertService;

        private void Start()
        {
            _alertService = GameManager.ServiceLocator.Get<AlertService>();
            _alertButton.onClick.AddListener(SetAlert);
        }

        private void SetAlert()
        {
            _isAlert = !_isAlert;
            _alertService.SetAlertState(_isAlert);
            _alertButton.image.sprite = _isAlert ? _releaseSign : _alertSign;
        }
    }
}