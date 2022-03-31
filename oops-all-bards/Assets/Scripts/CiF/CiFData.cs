using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CiFData
{
    public List<Affinity> affinities { get; set; }
    public List<Status> statuses { get; set; }
    public List<Trait> traits { get; set; }

    public CiFData(List<Affinity> affinities, List<Status> statuses, List<Trait> traits)
    {
        this.affinities = affinities;
        this.statuses = statuses;
        this.traits = traits;
    }

    public void AddStatus(Status status)
    {
        this.statuses.Add(status);
    }

    public void RemoveStatus(Status status)
    {
        this.statuses.Remove(status);
    }

    public void AddTrait(Trait trait)
    {
        this.traits.Add(trait);
    }

    public void RemoveTrait(Trait trait)
    {
        this.traits.Remove(trait);
    }
}
