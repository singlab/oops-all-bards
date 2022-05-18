using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CiFData
{
    [SerializeField] private List<Affinity> affinities;
    [SerializeField] private List<Status> statuses;
    [SerializeField] private List<Trait> traits;

    public List<Affinity> Affinities
    {
        get { return this.affinities; }
        set { this.affinities = value; }
    }

    public List<Status> Statuses
    {
        get { return this.statuses; }
        set { this.statuses = value; }
    }

    public List<Trait> Traits
    {
        get { return this.traits; }
        set { this.traits = value; }
    }

    public CiFData()
    {
        this.affinities = new List<Affinity>();
        this.statuses = new List<Status>();
        this.traits = new List<Trait>();
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

    public void AddAffinity(Affinity affinity)
    {
        this.affinities.Add(affinity);
    }

    public void RemoveAffinity(Affinity affinity)
    {
        this.affinities.Remove(affinity);
    }

    public int GetAffinityByID(int id)
    {
        foreach (Affinity a in affinities)
        {
            if (a.characterID == id)
            {
                return a.value;
            }
        }
        return 1000;
    }
}
