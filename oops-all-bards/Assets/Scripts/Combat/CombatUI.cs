using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Cinemachine;

public class CombatUI : MonoBehaviour
{

    private static CombatUI _instance;

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

    //Reference to Virtuoso items
    public PortraitData virtData;
    public GameObject specialSpace;
    public InfluenceAllyTurn influenceAlly;
    public int V = 0;

    //A reference to the virtual cameras for combat.
    public CinemachineVirtualCamera OverviewCamera;
    public CinemachineVirtualCamera AudienceCamera;
    public CinemachineVirtualCamera BandCamera;
    public CinemachineVirtualCamera EnemyCamera;

    public static CombatUI Instance => CombatUI._instance;

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


    // Start is called before the first frame update
    void Start()
    {
        combatManager = combat.GetComponent<CombatManager>();
        RenderUI();
        OverviewCamera.enabled = true;
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

        //Instantiate Virtuoso Bar
        GameObject virtuosoBar = Instantiate(portraitUI, specialSpace.transform);
        virtData = virtuosoBar.GetComponent<PortraitData>();

        virtData.nameText.text = "Virtuoso";
        virtData.healthBar.maxValue = 3;  //need to figure out how to limit this in ui  
        if (virtData.transform.Find("Frame").transform.Find("Background").transform.Find("HealthBar") != null)
        {
            virtData.transform.Find("Frame").transform.Find("Background").transform.Find("FlourishBar").gameObject.SetActive(false);
            virtData.transform.Find("Frame").transform.Find("Background").transform.Find("Icon").gameObject.SetActive(false);
        }

        // Instantiate portrait UI for party and enemies.
        // Need: name, current health/flourish, max health/flourish, portrait
        foreach (BasePlayer p in CombatManager.Instance.party)
        {
            GameObject toInstantiate = Instantiate(portraitUI, partyPortraits.transform);
            PortraitData portraitData = toInstantiate.GetComponent<PortraitData>();
            // Set name text
            Debug.Log("2");
            portraitData.nameText.text = p.Name;
            // Set portraits
            portraitData.icon.sprite = PartyManager.Instance.GetPortraitByName(p.Name);
            // Set health and flourish values and update them
            portraitData.healthBar.maxValue = p.Health;
            portraitData.healthBar.UpdateValueBar(p.Health);
            portraitData.flourishBar.maxValue = p.Flourish;
            portraitData.flourishBar.UpdateValueBar(p.Flourish);


        }

        foreach (BaseEnemy e in CombatManager.Instance.enemies)
        {
            GameObject toInstantiate = Instantiate(portraitUI, enemyPortraits.transform);
            PortraitData portraitData = toInstantiate.GetComponent<PortraitData>();
            toInstantiate.transform.localScale = new Vector3(toInstantiate.transform.localScale.x * -1, toInstantiate.transform.localScale.y, toInstantiate.transform.localScale.z);
            // Set name text
            portraitData.nameText.text = e.Name;
            portraitData.nameText.transform.localScale = new Vector3(-1, 1, 1);
            // Set portraits
            portraitData.icon.sprite = PartyManager.Instance.GetPortraitByName("Piggy");
            // Set health and flourish values and update them
            portraitData.healthBar.maxValue = e.Health;
            portraitData.healthBar.UpdateValueBar(e.Health);
            portraitData.healthBar.transform.Find("Background").transform.Find("HealthText").transform.localScale = new Vector3(-1, 1, 1);
            portraitData.flourishBar.maxValue = e.Flourish;
            portraitData.flourishBar.UpdateValueBar(e.Flourish);
            portraitData.flourishBar.transform.Find("Background").transform.Find("FlourishText").transform.localScale = new Vector3(-1, 1, 1);
            //Dont really need to see the flourish bar for enemies for any reason right now
            //flourishBar.gameObject.SetActive(false); //Turns off flourish bar display for enemies
        }



    }

    // Render the UI for the input.
    public void RenderInputMenu(BasePlayer actingCharacter)
    {
        // Render the UI for the input.
        Debug.Log("Rendering input menu for " + actingCharacter.Name + " now.");
        SetActiveCamera(OverviewCamera);
        Debug.Log(V + " Virtuoso");

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
        // Only render ally target buttons if influence is selected, can change to ability.CombatTypes.SUPPORT if we want player to only be able to support allies.
        if (ability.ID == 99 || ability.CombatType == BaseAbility.CombatAbilityTypes.SUPPORT || ability.CombatType == BaseAbility.CombatAbilityTypes.HEAL || ability.CombatType == BaseAbility.CombatAbilityTypes.DEFEND)
        {
            foreach (BasePlayer p in combatManager.party)
            {
                GameObject toInstantiate = Instantiate(targetButton, combatMenu.transform);
                toInstantiate.GetComponentInChildren<TMP_Text>().text = p.Name;
                toInstantiate.GetComponent<TargetButton>().target = p.Name;
            }
        }

        else
        {
            foreach (BaseEnemy e in combatManager.enemies)
            {
                GameObject toInstantiate = Instantiate(targetButton, combatMenu.transform);
                toInstantiate.GetComponentInChildren<TMP_Text>().text = e.Name;
                toInstantiate.GetComponent<TargetButton>().target = e.Name;

            }
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

    // A function that finds and returns a PortraitData object corresponding to a string name of a character.
    public static PortraitData FindPortrait(string name)
    {
        PortraitData portrait = null;
        for (int i = 0; i < CombatUI.Instance.partyPortraits.transform.childCount; i++)
        {
            PortraitData currentChild = CombatUI.Instance.partyPortraits.transform.GetChild(i).GetComponent<PortraitData>();
            if (currentChild.nameText.text == name)
            {
                portrait = currentChild;
            }
        }

        for (int i = 0; i < CombatUI.Instance.enemyPortraits.transform.childCount; i++)
        {
            PortraitData currentChild = CombatUI.Instance.enemyPortraits.transform.GetChild(i).GetComponent<PortraitData>();
            if (currentChild.nameText.text == name)
            {
                portrait = currentChild;
            }
        }
        return portrait;
    }

    public void ResetCamera()
    {
        OverviewCamera.m_LookAt = null;
        BandCamera.m_LookAt = null;
        EnemyCamera.m_LookAt = null;
        AudienceCamera.m_LookAt = null;
    }

    public void SetActiveCamera(CinemachineVirtualCamera camera)
    {
        ResetCamera();
        OverviewCamera.enabled = false;
        BandCamera.enabled = false;
        EnemyCamera.enabled = false;
        AudienceCamera.enabled = false;
        camera.enabled = true;
    }
}
