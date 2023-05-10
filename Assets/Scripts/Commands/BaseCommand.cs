using System.Collections.Generic;
using UnityEngine;

namespace Mandragora.Commands
{
    public abstract class BaseCommand
    {
        public static Queue<BaseCommand> CommandsQueue = new Queue<BaseCommand>();
        public static bool PlayingQueue { get; private set; }

        public void AddToQueue()
        {
            CommandsQueue.Enqueue(this);
            if (!PlayingQueue) PlayCommandFromQueue();
        }
        
        public virtual void StartCommandExecution(){}

        public virtual void StartNewQueue()
        {
            CommandsQueue = new Queue<BaseCommand>();
            CommandsQueue.Enqueue(this);
            PlayCommandFromQueue();
        }
        
        public virtual void CommandExecutionComplete()
        {
            if (CommandsQueue.Count > 0) PlayCommandFromQueue();
            else PlayingQueue = false;
        }

        private static void PlayCommandFromQueue()
        {
            Debug.Log("Start new command");
            PlayingQueue = true;
            CommandsQueue.Dequeue().StartCommandExecution();
        }
    }
}