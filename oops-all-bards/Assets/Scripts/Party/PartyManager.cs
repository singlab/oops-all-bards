using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    private static PartyManager _instance;
    public static PartyManager Instance => PartyManager._instance;
    public List<BasePlayer> currentParty = new List<BasePlayer>();
    public bool inCombat = false;
    public GameObject partyUI;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            RenderPartyUI();
            TogglePartyUI();
        }
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

    private void TogglePartyUI()
    {
        AssignPartyUI();
        partyUI.SetActive(!partyUI.activeSelf);
    }

    private void RenderPartyUI()
    {
        AssignPartyUI();
        Transform partyMembersContainer = partyUI.transform.GetChild(0).Find("PartyMembers");
        foreach (BasePlayer p in currentParty)
        {

        }
    }

    private void AssignPartyUI()
    {
        if (partyUI == null)
        {
            partyUI = GameObject.Find("PartyUI");
        }
    }
}
