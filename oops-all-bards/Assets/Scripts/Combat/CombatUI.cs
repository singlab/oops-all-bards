using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CombatUI : MonoBehaviour
{

    // References to player and enemy portraits.
    public GameObject partyPortraits;
    public GameObject enemyPortraits;
    // A reference to the portrait UI prefab.
    public GameObject portraitUI;
    //A reference to the tooltip UI
    public CanvasGroup currentToolTip;
    // A reference to the combat queue.
    public CombatQueue combatQueue;
    // A reference to the combat action menu.
    public GameObject combatMenu;
    // A reference to the combat action menu button prefab.
    public GameObject actionButton;
    // A reference to the target button prefab.
    public GameObject targetButton;

    //public reference to combat gameobject
    CombatManager combatManager;
    public GameObject combat;

    //A reference to the virtual cameras for combat.
    public GameObject OverviewCamera;
    public GameObject AudienceCamera;
    public GameObject BandCamera;
    public GameObject EnemyCamera;


    // Start is called before the first frame update
    void Start()
    {
        combatManager = combat.GetComponent<CombatManager>();
        RenderUI();
        OverviewCamera.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Debug.Log(Cursor.lockState);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // A function used to render all UI elements for the demo.
    public void RenderUI() 
    {
        // Render the portrait section and combat menu.
        partyPortraits.SetActive(true);
        enemyPortraits.SetActive(true);
        combatMenu.SetActive(true);

        //Set tooltips to initially be invisible
        currentToolTip.alpha = 0f;
        currentToolTip.blocksRaycasts = false;

        // Instantiate portrait UI for party and enemies.
        // Need: name, current health/flourish, max health/flourish, portrait
        foreach (BasePlayer p in combatManager.party)
        {
            GameObject toInstantiate = Instantiate(portraitUI, partyPortraits.transform);
            // Set name text
            toInstantiate.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).GetComponent<TMP_Text>().text = p.Name;
            // Set portraits
            toInstantiate.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = PartyManager.Instance.GetPortraitByName(p.Name);
            // Set health and flourish values and update them
            ValueBar healthBar = toInstantiate.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<ValueBar>();
            healthBar.maxValue = p.Health;
            healthBar.UpdateValueBar(p.Health);
            ValueBar flourishBar = toInstantiate.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponent<ValueBar>();
            flourishBar.maxValue = p.Flourish;
            flourishBar.UpdateValueBar(p.Flourish);
        }

        foreach (BaseEnemy e in combatManager.enemies)
        {
            GameObject toInstantiate = Instantiate(portraitUI, enemyPortraits.transform);
            // Set name text
            toInstantiate.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).GetComponent<TMP_Text>().text = e.Name;
            // Set portraits
            toInstantiate.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = PartyManager.Instance.GetPortraitByName("Piggy");
            // Set health and flourish values and update them
            ValueBar healthBar = toInstantiate.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<ValueBar>();
            healthBar.maxValue = e.Health;
            healthBar.UpdateValueBar(e.Health);
            ValueBar flourishBar = toInstantiate.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponent<ValueBar>();
            flourishBar.maxValue = e.Flourish;
            flourishBar.UpdateValueBar(e.Flourish);
            //Dont really need to see the flourish bar for enemies for any reason right now
            //flourishBar.gameObject.SetActive(false); //Turns off flourish bar display for enemies
        }
    }

    // Render the UI for the input.
    public void RenderInputMenu(BasePlayer actingCharacter) 
    {
        // Render the UI for the input.
        Debug.Log("Rendering input menu for " + actingCharacter.Name + " now.");

        // For however many abilities the player has, create action button prefabs and place them as children of combat menu.
        for (int i = 0; i < actingCharacter.PlayerClass.Abilities.Count; i++)
        {
            BaseAbility currentAbility = actingCharacter.PlayerClass.Abilities[i];
            // Create prefab and update text of button.
            GameObject toInstantiate = Instantiate(actionButton, combatMenu.transform);
            toInstantiate.GetComponentInChildren<TMP_Text>().text = currentAbility.Name;
            // Assign the ability and acting character to the action button script.
            toInstantiate.GetComponent<ActionButton>().ability = currentAbility;
            toInstantiate.GetComponent<ActionButton>().actingCharacter = actingCharacter;
            // // Add on click functions to the buttons to create a PlayerAction queueable.
            toInstantiate.GetComponent<Button>().onClick.AddListener(() =>
            { StartCoroutine(combatManager.SelectTarget(currentAbility, actingCharacter)); }); 

            //Add tooltip script to each ability button
            toInstantiate.AddComponent<ToolTips>();


            // If the ability assigned to the button cannot be executed due to lack of FP, disable it entirely.
            if (currentAbility.Cost > actingCharacter.Flourish)
            {
                toInstantiate.GetComponent<Button>().interactable = false;
            }
        }
    }

    // A function used to destroy all buttons that are children of the combat menu.
    public void ClearCombatMenu()
    {
        for (int i = 0; i < combatMenu.transform.childCount; i++)
        {
            GameObject toDestroy = combatMenu.transform.GetChild(i).gameObject;
            Destroy(toDestroy);
        }
    }

    // A function used to render target buttons.
    public void RenderTargetButtons(BaseAbility ability)
    {
        foreach (BasePlayer p in combatManager.party)
        {
            GameObject toInstantiate = Instantiate(targetButton, combatMenu.transform);
            toInstantiate.GetComponentInChildren<TMP_Text>().text = p.Name;
            toInstantiate.GetComponent<TargetButton>().target = p.Name;
        }
        // Only render ally target buttons if influence is selected, can change to ability.CombatTypes.SUPPORT if we want player to only be able to support allies.
        if (ability.ID == 99)
        {
            return;
        }

        foreach (BaseEnemy e in combatManager.enemies)
        {
            GameObject toInstantiate = Instantiate(targetButton, combatMenu.transform);
            toInstantiate.GetComponentInChildren<TMP_Text>().text = e.Name;
            toInstantiate.GetComponent<TargetButton>().target = e.Name;

        }

        //TEST adding in a back button

        GameObject backButton = Instantiate(targetButton, combatMenu.transform);
        backButton.GetComponentInChildren<TMP_Text>().text = "Back";
        backButton.GetComponent<TargetButton>().target = "back"; //target is now equal to back
    }

    public Tuple<ValueBar, ValueBar> FindValueBars(string name)
    {
        Tuple<ValueBar, ValueBar> relevantBars = new Tuple<ValueBar, ValueBar>(null, null);
        for (int i = 0; i < partyPortraits.transform.childCount; i++)
        {
            Transform currentChild = partyPortraits.transform.GetChild(i);
            Transform desiredChild = currentChild.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3);
            ValueBar healthBar = currentChild.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<ValueBar>();
            ValueBar flourishBar = currentChild.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject.GetComponent<ValueBar>(); ;
            if (desiredChild.GetComponent<TMP_Text>().text == name)
            {
                relevantBars = new Tuple<ValueBar, ValueBar>(healthBar, flourishBar);
            }
        }

        for (int i = 0; i < enemyPortraits.transform.childCount; i++)
        {
            Transform currentChild = enemyPortraits.transform.GetChild(i);
            Transform desiredChild = currentChild.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3);
            ValueBar healthBar = currentChild.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<ValueBar>();
            ValueBar flourishBar = currentChild.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).gameObject.GetComponent<ValueBar>(); ;
            if (desiredChild.GetComponent<TMP_Text>().text == name)
            {
                relevantBars = new Tuple<ValueBar, ValueBar>(healthBar, flourishBar);
            }
        }
        return relevantBars;
    }
}
