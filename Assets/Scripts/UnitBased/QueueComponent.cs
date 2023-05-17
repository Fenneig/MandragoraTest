using System.Collections.Generic;
using Mandragora.Commands;
using UnityEngine;

namespace Mandragora.UnitBased
{
    public class QueueComponent
    {
        private Unit _unit;
        
        public QueueComponent(Unit unit)
        {
            _unit = unit;
        }

        public void Enqueue(Unit unit, Queue<Unit> queue, string queueID, Vector3 firstInQueuePosition, Vector3 queueDirection, bool isQueuedAction)
        {
            var command = new QueueCommand(unit, queue, queueID, firstInQueuePosition, queueDirection);
            if (isQueuedAction) command.AddToQueue(_unit);
            else command.StartNewQueue(_unit);
        }
    }
}