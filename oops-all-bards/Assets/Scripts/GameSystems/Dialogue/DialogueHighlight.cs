using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;


public class DialogueHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color originalTextColor;
    public DialogueHighlightType highlightType = DialogueHighlightType.Default;

    private TMP_Text textComponent;
    private Button responseButton; // To control clickability

    // Define your bright red hover color
    public Color hoverColor = Color.red; // Assign in Inspector or keep as default red
    public Color failedCheckColor = Color.gray;
    public Color passedCheckColor = Color.green; // Color for passed check when NOT hovered

    void Awake()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
        responseButton = GetComponent<Button>(); // Get the Button component on the same GameObject

        if (textComponent == null)
        {
            Debug.LogError("DialogueHighlight: TMP_Text component not found in children!", gameObject);
            enabled = false;
            return;
        }
        if (responseButton == null)
        {
            Debug.LogWarning("DialogueHighlight: Button component not found on this GameObject. Clickability control will not work.", gameObject);
        }
        originalTextColor = textComponent.color;
    }

    void Start()
    {
        ApplyRestingColor();
        UpdateClickability();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textComponent == null || (responseButton != null && !responseButton.interactable)) return; // Don't highlight if not interactable

        if (highlightType == DialogueHighlightType.FailedSkillCheck)
        {
            textComponent.color = failedCheckColor; // Failed checks stay their resting color (gray) on hover
        }
        else // Default or PassedSkillCheck
        {
            textComponent.color = hoverColor; // Bright red for hover
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textComponent == null) return;
        ApplyRestingColor(); // Revert to the appropriate "at rest" color
    }

    // Sets the visual state when not hovered
    public void ApplyRestingColor()
    {
        if (textComponent == null) return;

        switch (highlightType)
        {
            case DialogueHighlightType.PassedSkillCheck:
                textComponent.color = passedCheckColor; // Or your desired "passed" color
                break;
            case DialogueHighlightType.FailedSkillCheck:
                textComponent.color = failedCheckColor;
                break;
            case DialogueHighlightType.Default:
            default:
                textComponent.color = originalTextColor;
                break;
        }
    }

    // Call this to set the initial state and also if highlightType changes dynamically
    public void InitializeHighlight(DialogueHighlightType type)
    {
        highlightType = type;
        if (!Application.isPlaying) return; // Guard against editor-time calls if Start hasn't run

        ApplyRestingColor();
        UpdateClickability();
    }

    private void UpdateClickability()
    {
        if (responseButton != null)
        {
            if (highlightType == DialogueHighlightType.FailedSkillCheck)
            {
                responseButton.interactable = false;
            }
            else
            {
                responseButton.interactable = true;
            }
        }
    }

    // Renamed SetDefault to be more descriptive for external calls
    public void SetToDefaultState()
    {
        InitializeHighlight(DialogueHighlightType.Default);
    }

    public enum DialogueHighlightType
    {
        Default,
        PassedSkillCheck,
        FailedSkillCheck
    }
}