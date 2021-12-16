using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CombatQueue
{ 
    public Queue<ICombatQueueable> queue { get; set; }

    public CombatQueue()
    {
        this.queue = new Queue<ICombatQueueable>();
    }

    public void Clear()
    {
        this.queue.Clear();
    }

    public void Push(ICombatQueueable queueable)
    {
        this.queue.Enqueue(queueable);
    }

    public ICombatQueueable Pop()
    {
        return this.queue.Dequeue();
    }

    public bool IsEmpty()
    {  
        return this.queue.Count == 0 ? true : false;
    }
}
