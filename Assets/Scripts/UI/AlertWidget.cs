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

        private void Awake()
        {
            _alertButton.onClick.AddListener(SetAlert);
        }

        private void SetAlert()
        {
            _isAlert = !_isAlert;
            GameSession.Instance.SetAlertState(_isAlert);
            _alertButton.image.sprite = _isAlert ? _releaseSign : _alertSign;
        }
    }
}