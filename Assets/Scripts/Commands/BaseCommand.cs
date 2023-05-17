using System;
using System.Collections.Generic;
using Mandragora.Systems;
using Unit = Mandragora.UnitBased.Unit;

namespace Mandragora.Commands
{
    public abstract class BaseCommand
    {
        protected static Dictionary<Unit, BaseCommand> CurrentUnitsCommand = new Dictionary<Unit, BaseCommand>();
        private static Dictionary<Unit, Queue<BaseCommand>> _commandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
        private static Dictionary<Unit, Queue<BaseCommand>> _preAlertCommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();

        public static Action<Unit> OnAnyActionCanceled;
        public static Action<Unit> OnAnyQueueChanged;
        
        public void AddToQueue(Unit unit)
        {
            if (_commandsQueue.TryGetValue(unit, out var queue))
            {
                queue.Enqueue(this);
                if (!unit.IsBusy) PlayCommandFromQueue(unit);
            }
            else
            {
                _preAlertCommandsQueue.Add(unit, new Queue<BaseCommand>());
                var newQueue = new Queue<BaseCommand>();
                _commandsQueue.Add(unit, newQueue);
                newQueue.Enqueue(this);
                PlayCommandFromQueue(unit);
            }

            OnAnyQueueChanged?.Invoke(unit);
        }

        protected virtual void StartCommandExecution() { }

        public void StartNewQueue(Unit unit)
        {
            if (GameSession.Instance.IsAlert) return;
            if (_commandsQueue.TryGetValue(unit, out var queue))
            {
                OnAnyActionCanceled?.Invoke(unit);
                queue.Clear();
                queue.Enqueue(this);
            }
            else
            {
                _preAlertCommandsQueue.Add(unit, new Queue<BaseCommand>());
                var newQueue = new Queue<BaseCommand>();
                _commandsQueue.Add(unit, newQueue);
                newQueue.Enqueue(this);
            }
            PlayCommandFromQueue(unit);
            OnAnyQueueChanged?.Invoke(unit);
        }

        protected virtual void CommandExecutionComplete(Unit unit)
        {
            if (GameSession.Instance.IsAlert) return;
            if (!_commandsQueue.TryGetValue(unit, out var queue)) return;
            if (queue.Count > 0)
            {
                PlayCommandFromQueue(unit);
            }
            else
            {
                unit.IsBusy = false;
                CurrentUnitsCommand[unit] = null;
            }

            OnAnyQueueChanged?.Invoke(unit);
        }

        private static void PlayCommandFromQueue(Unit unit)
        {
            if (!_commandsQueue.TryGetValue(unit, out var queue) || queue.Count == 0) return;

            CurrentUnitsCommand[unit] = queue.Dequeue();
            CurrentUnitsCommand[unit].StartCommandExecution();
            OnAnyQueueChanged?.Invoke(unit);

            unit.IsBusy = true;
        }

        public static string ToString(Unit unit)
        {
            string resultString = "";
            if (GameSession.Instance.IsAlert) return "Hide in hangar!";
            if (CurrentUnitsCommand.TryGetValue(unit, out var currentCommand))
            {
                resultString += currentCommand + "\r\n";
            }

            if (_commandsQueue.TryGetValue(unit, out var queue))
            {
                foreach (var command in queue)
                {
                    resultString += command.ToString();
                    resultString += "\r\n";
                }
            }

            return resultString;
        }

        public override string ToString()
        {
            return "";
        }

        public static void SetAlertState(bool isAlert)
        {
            if (isAlert)
            {
                _preAlertCommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
                foreach (var commandQueue in _commandsQueue)
                {
                    var unit = commandQueue.Key;
                    _preAlertCommandsQueue.Add(unit, new Queue<BaseCommand>());
                }
            }
            else
            {
                _commandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
            }

            OnAnyQueueChanged?.Invoke(GameSession.Instance.UnitActionSystem.SelectedUnit);
        }

        public static void StashUnitCommandLines(Unit unit)
        {
            if (!_commandsQueue.TryGetValue(unit, out var commandQueue)) return;
            
            var currentCommand = CurrentUnitsCommand[unit];
            if (currentCommand != null)
            {
                _preAlertCommandsQueue[unit].Enqueue(currentCommand);
            }

            foreach (var command in commandQueue)
            {
                _preAlertCommandsQueue[unit].Enqueue(command);
            }

            OnAnyActionCanceled?.Invoke(unit);
        }

        public static void ReadFromStashUnitCommandLines(Unit unit)
        {
            if (!_preAlertCommandsQueue.ContainsKey(unit)) return;
            var commandQueue = _preAlertCommandsQueue[unit];
            _commandsQueue.Add(unit, new Queue<BaseCommand>(commandQueue));

            PlayCommandFromQueue(unit);
        }
    }
}