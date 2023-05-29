using System;
using System.Collections.Generic;
using Mandragora.Services;
using Mandragora.Systems;
using Unit = Mandragora.UnitBased.Unit;

namespace Mandragora.Commands
{
    public class CommandService : IService
    {
        public Dictionary<Unit, BaseCommand> CurrentUnitsCommand { get; }
        public Dictionary<Unit, Queue<BaseCommand>> CommandsQueue { get; private set; }
        public Dictionary<Unit, Queue<BaseCommand>> PreAlertCommandsQueue { get; private set; }

        public AlertService AlertService { get; }

        public CommandService(AlertService alertService)
        {
            CurrentUnitsCommand = new Dictionary<Unit, BaseCommand>();
            CommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
            PreAlertCommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
            AlertService = alertService;
            alertService.OnAlertStateChanged += SetAlertState;
        }

        ~CommandService()
        {
            AlertService.OnAlertStateChanged -= SetAlertState;
        }
        
        public Action<Unit> OnAnyActionCanceled;
        public Action<Unit> OnAnyQueueChanged;
        
        public string GetUnitCommandLines(Unit unit)
        {
            string resultString = "";
            if (AlertService.IsAlert) return "Hide in hangar!";
            if (CurrentUnitsCommand.TryGetValue(unit, out var currentCommand))
            {
                resultString += currentCommand + "\r\n";
            }

            if (CommandsQueue.TryGetValue(unit, out var queue))
            {
                foreach (var command in queue)
                {
                    resultString += command.ToString();
                    resultString += "\r\n";
                }
            }

            return resultString;
        }   
        
        private void SetAlertState(bool isAlert)
        {
            if (isAlert)
            {
                PreAlertCommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
                foreach (var commandQueue in CommandsQueue)
                {
                    var unit = commandQueue.Key;
                    PreAlertCommandsQueue.Add(unit, new Queue<BaseCommand>());
                }
            }
            else
            {
                CommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
            }

            OnAnyQueueChanged?.Invoke(GameSession.Instance.UnitActionSystem.SelectedUnit);
        }

        public void StashUnitCommandLines(Unit unit)
        {
            if (!CommandsQueue.TryGetValue(unit, out Queue<BaseCommand> commandQueue)) return;
            
            var currentCommand = CurrentUnitsCommand[unit];
            if (currentCommand != null)
            {
                PreAlertCommandsQueue[unit].Enqueue(currentCommand);
            }

            foreach (BaseCommand command in commandQueue)
            {
                PreAlertCommandsQueue[unit].Enqueue(command);
            }

            OnAnyActionCanceled?.Invoke(unit);
        }

        public void ReadFromStashUnitCommandLines(Unit unit)
        {
            if (!PreAlertCommandsQueue.ContainsKey(unit)) return;
            Queue<BaseCommand> commandQueue = PreAlertCommandsQueue[unit];
            CommandsQueue[unit] = new Queue<BaseCommand>(commandQueue);

            PlayCommandFromQueue(unit);
        }
        
        public void PlayCommandFromQueue(Unit unit)
        {
            if (!CommandsQueue.TryGetValue(unit, out var queue) || queue.Count == 0) return;

            CurrentUnitsCommand[unit] = queue.Dequeue();
            CurrentUnitsCommand[unit].StartCommandExecution();
            OnAnyQueueChanged?.Invoke(unit);

            unit.IsBusy = true;
        }
    }
}