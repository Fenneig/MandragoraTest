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
                OnAnyQueueChanged?.Invoke(unit);
            }
        }

        public static void PlayCommandFromQueue(Unit unit)
        {
            if (!CommandsQueue.TryGetValue(unit, out var queue)) return;
            CurrentUnitsCommand[unit] = queue.Dequeue();
            CurrentUnitsCommand[unit].StartCommandExecution();
            OnAnyQueueChanged?.Invoke(unit);
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

        //TODO: исправить работу очередей при возврате действий
        public static void SetAlertState(bool state)
        {
            if (state)
            {
                _preAlertCommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
                
                foreach (var commandQueue in CommandsQueue)
                {
                    var unit = commandQueue.Key;
                    OnAnyActionCanceled?.Invoke(unit);
                    _preAlertCommandsQueue.Add(unit, new Queue<BaseCommand>());
                    _preAlertCommandsQueue[unit].Enqueue(CurrentUnitsCommand[unit]);
                    CurrentUnitsCommand[unit] = null;
                    foreach (var command in commandQueue.Value)
                    {
                        _preAlertCommandsQueue[unit].Enqueue(command);
                    }
                    
                    CommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
                }
            }
            else
            {
                foreach (var commandQueue in _preAlertCommandsQueue)
                {
                    var unit = commandQueue.Key;
                    CommandsQueue[unit] = new Queue<BaseCommand>(commandQueue.Value);
                }
            }
        }
    }
}