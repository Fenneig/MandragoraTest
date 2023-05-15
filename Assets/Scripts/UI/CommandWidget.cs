using UnityEngine;
using UnityEngine.UI;

namespace Mandragora.UI
{
    public class CommandWidget : MonoBehaviour
    {
        [SerializeField] private Text _commandText;

        public void SetText(string text)
        {
            _commandText.text = text;
        }
    }
}