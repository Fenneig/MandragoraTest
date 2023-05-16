using System;
using System.Collections.Generic;
using Unit = Mandragora.UnitBased.Unit;

namespace Mandragora.Commands
{
    public abstract class BaseCommand
    {
        protected static Dictionary<Unit, Queue<BaseCommand>> CommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
        protected static Dictionary<Unit, BaseCommand> CurrentUnitsCommand = new Dictionary<Unit, BaseCommand>();
        private static Dictionary<Unit, Queue<BaseCommand>> _preAlertCommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();

        public static Action<Unit> OnAnyActionCanceled;
        public static Action<Unit> OnAnyQueueChanged;
        
        public void AddToQueue(Unit unit)
        {
            if (CommandsQueue.TryGetValue(unit, out var queue))
            {
                queue.Enqueue(this);
                if (!unit.IsBusy) PlayCommandFromQueue(unit);
            }
            else
            {
                _preAlertCommandsQueue.Add(unit, new Queue<BaseCommand>());
                var newQueue = new Queue<BaseCommand>();
                CommandsQueue.Add(unit, newQueue);
                newQueue.Enqueue(this);
                PlayCommandFromQueue(unit);
            }

            OnAnyQueueChanged?.Invoke(unit);
        }

        public virtual void StartCommandExecution() { }

        public void StartNewQueue(Unit unit)
        {
            if (CommandsQueue.TryGetValue(unit, out var queue))
            {
                OnAnyActionCanceled?.Invoke(unit);
                queue.Clear();
                queue.Enqueue(this);
            }
            else
            {
                _preAlertCommandsQueue.Add(unit, new Queue<BaseCommand>());
                var newQueue = new Queue<BaseCommand>();
                CommandsQueue.Add(unit, newQueue);
                newQueue.Enqueue(this);
            }
            PlayCommandFromQueue(unit);
            OnAnyQueueChanged?.Invoke(unit);
        }

        protected virtual void CommandExecutionComplete(Unit unit)
        {
            if (!CommandsQueue.TryGetValue(unit, out var queue)) return;
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
            if (!CommandsQueue.TryGetValue(unit, out var queue) || queue.Count == 0) return;
            CurrentUnitsCommand[unit] = queue.Dequeue();
            CurrentUnitsCommand[unit].StartCommandExecution();
            unit.IsBusy = true;
        }

        public static string ToString(Unit unit)
        {
            string resultString = "";
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

        public override string ToString()
        {
            return "";
        }

        public static void SetAlertState(bool isAlert)
        {
            if (isAlert)
            {
                _preAlertCommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
                foreach (var commandQueue in CommandsQueue)
                {
                    var unit = commandQueue.Key;
                    _preAlertCommandsQueue.Add(unit, new Queue<BaseCommand>());
                }
            }
            else
            {
                CommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
            }
        }

        public static void StashUnitCommandLines(Unit unit)
        {
            if (!CommandsQueue.TryGetValue(unit, out var commandQueue)) return;

            _preAlertCommandsQueue[unit].Enqueue(CurrentUnitsCommand[unit]);

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
            CommandsQueue.Add(unit, new Queue<BaseCommand>(commandQueue));

            PlayCommandFromQueue(unit);
        }
    }
}