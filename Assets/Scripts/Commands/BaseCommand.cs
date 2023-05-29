using System.Collections.Generic;
using Mandragora.Systems;
using Unit = Mandragora.UnitBased.Unit;

namespace Mandragora.Commands
{
    public abstract class BaseCommand
    {
        protected CommandService CommandService => 
            _commandService ??= GameManager.ServiceLocator.Get<CommandService>();
        protected AlertService AlertService => 
            _alertService ??= GameManager.ServiceLocator.Get<AlertService>();

        private CommandService _commandService;
        private AlertService _alertService;

        public void AddToQueue(Unit unit)
        {
            if (CommandService.CommandsQueue.TryGetValue(unit, out var queue))
            {
                queue.Enqueue(this);
                if (!unit.IsBusy) CommandService.PlayCommandFromQueue(unit);
            }
            else
            {
                CommandService.PreAlertCommandsQueue.Add(unit, new Queue<BaseCommand>());
                var newQueue = new Queue<BaseCommand>();
                CommandService.CommandsQueue.Add(unit, newQueue);
                newQueue.Enqueue(this);
                CommandService.PlayCommandFromQueue(unit);
            }

            CommandService.OnAnyQueueChanged?.Invoke(unit);
        }

        public virtual void StartCommandExecution() { }

        public void StartNewQueue(Unit unit)
        {
            if (AlertService.IsAlert) return;
            if (CommandService.CommandsQueue.TryGetValue(unit, out var queue))
            {
                CommandService.OnAnyActionCanceled?.Invoke(unit);
                queue.Clear();
                queue.Enqueue(this);
            }
            else
            {
                CommandService.PreAlertCommandsQueue.Add(unit, new Queue<BaseCommand>());
                var newQueue = new Queue<BaseCommand>();
                CommandService.CommandsQueue.Add(unit, newQueue);
                newQueue.Enqueue(this);
            }
            CommandService.PlayCommandFromQueue(unit);
            CommandService.OnAnyQueueChanged?.Invoke(unit);
        }

        protected virtual void CommandExecutionComplete(Unit unit)
        {
            if (AlertService.IsAlert) return;
            if (!CommandService.CommandsQueue.TryGetValue(unit, out var queue)) return;
            if (queue.Count > 0)
            {
                CommandService.PlayCommandFromQueue(unit);
            }
            else
            {
                unit.IsBusy = false;
                CommandService.CurrentUnitsCommand[unit] = null;
            }

            CommandService.OnAnyQueueChanged?.Invoke(unit);
        }

        public override string ToString()
        {
            return "";
        }
    }
}