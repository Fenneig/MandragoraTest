using System;
using System.Collections.Generic;
using Unit = Mandragora.UnitBased.Unit;

namespace Mandragora.Commands
{
    public abstract class BaseCommand
    {
        protected static Dictionary<Unit, Queue<BaseCommand>> CommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();
        private static Dictionary<Unit, BaseCommand> _currentUnitsCommand = new Dictionary<Unit, BaseCommand>();

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
            if (queue.Count > 0) PlayCommandFromQueue(unit);
            else unit.IsBusy = false;
        }

        private static void PlayCommandFromQueue(Unit unit)
        {
            if (!CommandsQueue.TryGetValue(unit, out var queue)) return;
            _currentUnitsCommand[unit] = queue.Dequeue();
            _currentUnitsCommand[unit].StartCommandExecution();
            OnAnyQueueChanged?.Invoke(unit);
            unit.IsBusy = true;    
        }

        public static string ToString(Unit unit)
        {
            string resultString = "";
            if (_currentUnitsCommand.TryGetValue(unit, out var currentCommand))
            {
                resultString += currentCommand.ToString() + "\r\n";
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
    }
}