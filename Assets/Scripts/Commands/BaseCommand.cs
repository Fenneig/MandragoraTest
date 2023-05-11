using System.Collections.Generic;
using Mandragora.UnitBased;

namespace Mandragora.Commands
{
    public abstract class BaseCommand
    {
        protected static Dictionary<Unit, Queue<BaseCommand>> CommandsQueue = new Dictionary<Unit, Queue<BaseCommand>>();

        public void AddToQueue(Unit unit)
        {
            if (CommandsQueue.TryGetValue(unit, out var queue))
            {
                queue.Enqueue(this);
            }
            else
            {
                var newQueue = new Queue<BaseCommand>();
                CommandsQueue.Add(unit, newQueue);
                newQueue.Enqueue(this);
            }
        }

        public virtual void StartCommandExecution(){}

        public void StartNewQueue(Unit unit)
        {
            if (CommandsQueue.TryGetValue(unit, out var queue))
            {
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
        }

        protected virtual void CommandExecutionComplete(Unit unit)
        {
            if (!CommandsQueue.TryGetValue(unit, out var queue)) return;
            if (queue.Count > 0) PlayCommandFromQueue(unit);
        }

        private static void PlayCommandFromQueue(Unit unit)
        {
            if (CommandsQueue.TryGetValue(unit, out var queue))
            {
                queue.Dequeue().StartCommandExecution();
            }
        }
    }
}