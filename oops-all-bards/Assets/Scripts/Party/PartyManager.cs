using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    private static PartyManager _instance;
    public static PartyManager Instance => PartyManager._instance;
    public List<BasePlayer> currentParty = new List<BasePlayer>();
    public bool inCombat = false;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } else if (_instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AddCharacterToParty(BasePlayer character)
    {
        currentParty.Add(character);
    }

    public void RemoveCharacterFromParty(BasePlayer character)
    {
        currentParty.Remove(character);
    }

    public void ToggleInCombat(bool value)
    {
        inCombat = value;
    }

    public BasePlayer FindPartyMemberById(int id)
    {
        foreach (BasePlayer p in currentParty) 
        {
            if (p.ID == id)
            {
                return p;
            }
        }
        return null;
    }
}
