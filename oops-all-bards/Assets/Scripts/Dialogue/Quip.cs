using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Quip
{
    public enum QuipTypes {
        GENERAL,
        COMBAT
    }
    // The text representation of this quip.
    [SerializeField] private string text;
    // The type of quip in a broader context.
    [SerializeField] private QuipTypes type;
    // Any associated CiF status that should trigger this quip.
    [SerializeField] private Status.StatusTypes cifStatusType;
    // Any associated combat status that should trigger this quip.
    [SerializeField] private CombatStatus.CombatStatusTypes combatStatusType;

    public string Text
    {
        get { return this.text; }
        set { this.text = value; }
    }

    public QuipTypes Type
    {
        get { return this.type; }
        set { this.type = value; }
    }

    public Status.StatusTypes CiFStatusType
    {
        get { return this.cifStatusType; }
        set { this.cifStatusType = value; }
    }

    public CombatStatus.CombatStatusTypes CombatStatusType
    {
        get { return this.combatStatusType; }
        set { this.combatStatusType = value; }
    }
}

[System.Serializable]
public class Quips
{
    [SerializeField] public List<Quip> quips = new List<Quip>();

    public List<Quip> GetQuipsByCiFStatus(Status.StatusTypes type)
    {
        Debug.Log("Getting CiF status specific quips.");
        List<Quip> sublist = new List<Quip>();
        IEnumerable<Quip> query = quips.Where(x => x.CiFStatusType == type);

        foreach (Quip q in query)
        {
            sublist.Add(q);
        }
        return sublist;
    }

    public List<Quip> GetQuipsByCombatStatus(CombatStatus.CombatStatusTypes type)
    {
        Debug.Log("Getting combat status specific quips.");
        List<Quip> sublist = new List<Quip>();
        IEnumerable<Quip> query = quips.Where(x => x.CombatStatusType == type);

        foreach (Quip q in query)
        {
            sublist.Add(q);
        }
        return sublist;
    }

    public List<Quip> GetGenericCombatQuips()
    {
        Debug.Log("Getting generic combat quips.");
        List<Quip> sublist = new List<Quip>();
        IEnumerable<Quip> query = quips.Where(x => ((int)x.CombatStatusType == 0 && (int)x.Type == 1));

        foreach (Quip q in query)
        {
            sublist.Add(q);
        }
        return sublist;
    }

    public List<Quip> GetGenericNoncombatQuips()
    {
        Debug.Log("Getting generic noncombat quips.");
        List<Quip> sublist = new List<Quip>();
        IEnumerable<Quip> query = quips.Where(x => ((int)x.CiFStatusType == 0 && (int)x.Type == 0));

        foreach (Quip q in query)
        {
            sublist.Add(q);
        }
        return sublist;
    }

    // public void DebugQuips()
    // {
    //     foreach (Quip q in quips)
    //     {
    //         Debug.Log($"{q.Type} {(int)q.Type}");
    //         Debug.Log($"{q.CiFStatusType} {(int)q.CiFStatusType}");
    //         Debug.Log($"{q.CombatStatusType} {(int)q.CombatStatusType}");

    //         if ((int)q.CiFStatusType == 0 && (int)q.Type == 0)
    //         {
    //             Debug.Log("This is a generic noncombat quip.");
    //         }
    //     }
    // }
}