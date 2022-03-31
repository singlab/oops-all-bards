using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    private static PartyManager _instance;
    public static PartyManager Instance => PartyManager._instance;
    public static List<BasePlayer> currentParty = new List<BasePlayer>();
    public static bool inCombat = false;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } else if (_instance != null)
        {
            Destroy(gameObject);
        }
    }

    public static void AddCharacterToParty(BasePlayer character)
    {
        currentParty.Add(character);
    }

    public static void RemoveCharacterFromParty(BasePlayer character)
    {
        currentParty.Remove(character);
    }

    public static void ToggleInCombat(bool value)
    {
        inCombat = value;
    }
}
