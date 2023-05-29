using System.Collections.Generic;
using System.Linq;
using Mandragora.Commands;
using Mandragora.Systems;
using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.UI
{
    public class UnitCommandsList : MonoBehaviour
    {
        [SerializeField] private CommandWidget _commandPrefab;

        private List<CommandWidget> _commandsList;
        private UnitActionSystem _unitActionSystem;
        private CommandService _commandService;

        private void Awake()
        {
            _commandsList = new List<CommandWidget>();

            RegisterServices();
        }

        private void RegisterServices()
        {
            _commandService = GameManager.ServiceLocator.Get<CommandService>();
            _commandService.OnAnyQueueChanged += CheckIsNeedToUpdateUI;

            _unitActionSystem = GameManager.ServiceLocator.Get<UnitActionSystem>();
            _unitActionSystem.OnSelectedUnitChanged += UpdateUIList;
        }

        private void CheckIsNeedToUpdateUI(Unit unit)
        {
            if (unit == _unitActionSystem.SelectedUnit) 
                UpdateUIList();
        }

        private void UpdateUIList()
        {
            if (_unitActionSystem.SelectedUnit == null) return;
           
            TurnOffAllWidgets();
            
            List<string> commandsList = GetCommandsString();

            CreateRequiredWidgets(commandsList);

            FillWidgets(commandsList);
        }

        private void TurnOffAllWidgets()
        {
            foreach (var commandGameObject in _commandsList)
            {
                commandGameObject.gameObject.SetActive(false);
            }
        }

        private List<string> GetCommandsString()
        {
            string commandLines = _commandService.GetUnitCommandLines(_unitActionSystem.SelectedUnit);
            List<string> commandsList = commandLines.Split("\r\n").ToList();
            commandsList =
                new List<string>(commandsList.Where(commandDescription => !string.IsNullOrEmpty(commandDescription)));
            
            return commandsList;
        }

        private void CreateRequiredWidgets(List<string> commandsString)
        {
            for (int i = 0; i < commandsString.Count - _commandsList.Count; i++)
            {
                var commandUi = Instantiate(_commandPrefab, transform);
                _commandsList.Add(commandUi);
            }
        }

        private void FillWidgets(List<string> commandsString)
        {
            for (int i = 0; i < commandsString.Count; i++)
            {
                _commandsList[i].gameObject.SetActive(true);
                _commandsList[i].SetText(commandsString[i]);
            }
        }

        private void OnDestroy()
        {
            _unitActionSystem.OnSelectedUnitChanged -= UpdateUIList;
            _commandService.OnAnyQueueChanged -= CheckIsNeedToUpdateUI;
        }
    }
}
