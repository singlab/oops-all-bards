using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUIData : MonoBehaviour
{
    public Image portrait;
    public TMP_Text speakerName;
    public TMP_Text nodeText;
    public GameObject nodeContentOrganizer;
    void Awake()
    {
        Debug.Log(gameObject.name);//TEST

        portrait = transform.Find("PortraitPanel").transform.Find("Portrait").GetComponent<Image>();
        speakerName = transform.Find("PortraitPanel").Find("Name").GetComponent<TMP_Text>();
        nodeText = transform.Find("TextPanel").transform.Find("UsableSpace").Find("NodeText").GetComponent<TMP_Text>();
        nodeContentOrganizer = transform.Find("TextPanel").transform.Find("UsableSpace").Find("NodeContentOrganizer").gameObject;
    }

    public void DisableUI()
    {
        gameObject.SetActive(false);
    }
}
