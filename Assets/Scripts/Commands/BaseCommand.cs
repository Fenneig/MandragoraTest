using System.Collections.Generic;
using UnityEngine;

namespace Mandragora.Commands
{
    public abstract class BaseCommand
    {
        protected static Queue<BaseCommand> CommandsQueue = new Queue<BaseCommand>();
        protected static bool PlayingQueue { get; private set; }

        public void AddToQueue()
        {
            CommandsQueue.Enqueue(this);
            if (!PlayingQueue) PlayCommandFromQueue();
        }

        protected virtual void StartCommandExecution(){}

        public void StartNewQueue()
        {
            CommandsQueue = new Queue<BaseCommand>();
            CommandsQueue.Enqueue(this);
            PlayCommandFromQueue();
        }

        protected virtual void CommandExecutionComplete()
        {
            if (CommandsQueue.Count > 0) PlayCommandFromQueue();
            else PlayingQueue = false;
        }

        private static void PlayCommandFromQueue()
        {
            PlayingQueue = true;
            CommandsQueue.Dequeue().StartCommandExecution();
        }
    }
}