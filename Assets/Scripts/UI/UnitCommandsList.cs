using System.Collections.Generic;
using System.Linq;
using Mandragora.Commands;
using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.UI
{
    public class UnitCommandsList : MonoBehaviour
    {
        [SerializeField] private CommandWidget _commandPrefab;

        private List<CommandWidget> _commandsList = new List<CommandWidget>();
        private Unit _selectedUnit;

        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += UpdateUIList;
            BaseCommand.OnAnyQueueChanged += CheckIsNeedToUpdateUI;
        }

        private void CheckIsNeedToUpdateUI(Unit unit)
        {
            if (unit == UnitActionSystem.Instance.SelectedUnit) 
                UpdateUIList();
        }

        public void UpdateUIList()
        {
            if (UnitActionSystem.Instance.SelectedUnit == null) return;
            foreach (var commandGameObject in _commandsList)
            {
                commandGameObject.gameObject.SetActive(false);
            }
            var commands = BaseCommand.ToString(UnitActionSystem.Instance.SelectedUnit);
            var commandsString = commands.Split("\r\n").ToList();
            commandsString = new List<string>(commandsString.Where(commandDescription => !string.IsNullOrEmpty(commandDescription)));
            
            for (int i = 0; i < commandsString.Count - _commandsList.Count; i++)
            {
                var commandUi = Instantiate(_commandPrefab, transform);
                _commandsList.Add(commandUi);
            }

            for (int i = 0; i < commandsString.Count; i++)
            {
                _commandsList[i].gameObject.SetActive(true);
                _commandsList[i].SetText(commandsString[i]);
            }
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= UpdateUIList;
            BaseCommand.OnAnyQueueChanged -= CheckIsNeedToUpdateUI;
        }
    }
}
