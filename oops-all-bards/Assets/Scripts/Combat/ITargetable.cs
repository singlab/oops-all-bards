using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An interface that allows classes derived from it to be targetable by player/enemy actions.
public class ITargetable
{
    public virtual string name { get; set; }
}
