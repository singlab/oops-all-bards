using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;


public class ToolTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    //Reference to tooltip TMP text object
    public TextMeshProUGUI decriptionDisplay;
    //Reference to tooltip(s) panel display
    public CanvasGroup toolTipDisplay;


    // Start is called before the first frame update
    void Start()
    {
        //Reference the TMP game object to manipulate and update tooltips description later
        decriptionDisplay = GameObject.Find("ToolTipDisplay").GetComponentInChildren<TextMeshProUGUI>();
        //Reference the canvasgroup in the tooltip game object in order to adjust whether or not tooltips are visible
        toolTipDisplay = GameObject.Find("ToolTipDisplay").GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Have the tooltip follow the mouse at an offset position
        if(EventSystem.current.IsPointerOverGameObject())
        {
            toolTipDisplay.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 200, Input.mousePosition.z + 10);
        }
        
    }

    //Use to render in tooltips
    public void OnPointerEnter(PointerEventData eventData)
    {

       //Assign/update to appropriate class tooltip text
       //For damage abilities
       if(gameObject.GetComponent<ActionButton>().ability.CombatType == BaseAbility.CombatAbilityTypes.ATTACK)
        {
          decriptionDisplay.text = gameObject.GetComponent<ActionButton>().ability.Name + "\n" + "Cost: " + gameObject.GetComponent<ActionButton>().ability.Cost + "\n" +  "Deals " + gameObject.GetComponent<ActionButton>().ability.Damage + " damage"+ "\n\n" + gameObject.GetComponent<ActionButton>().ability.Description;

        }
       //For defending abilities
       else if(gameObject.GetComponent<ActionButton>().ability.CombatType == BaseAbility.CombatAbilityTypes.DEFEND)
        {
            decriptionDisplay.text = gameObject.GetComponent<ActionButton>().ability.Name + "\n" + "Cost: " + gameObject.GetComponent<ActionButton>().ability.Cost + "\n" + "Protects from " + gameObject.GetComponent<ActionButton>().ability.Damage + " damage" + "\n\n" + gameObject.GetComponent<ActionButton>().ability.Description;

        }
       //CAN ADD ON FOR HEALING AND SUPPORT ABILITIES


       //Make tooltips visible
       toolTipDisplay.alpha = 1f;
        
    }

    //Used to un-render tooltips when not moused over buttons
    public void OnPointerExit(PointerEventData eventData)
    {
        //Make tooltips invisible 
        toolTipDisplay.alpha = 0f;
    }

    //Used to avoid weird issue where tooltips dont dissapear after selecting an action
    public void OnPointerDown(PointerEventData eventData)
    {
            toolTipDisplay.alpha = 0f;
        
    }
}