using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialCircleManager : MonoBehaviour
{
    private static SocialCircleManager _instance;
    public static SocialCircleManager Instance => SocialCircleManager._instance;

    public GameObject socialCircleUI;
    public GameObject characterSocialCircleUI;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) //test key
        {
            RenderSocialCircleUI();
            ToggleSocialCircleUI();
        }
    }

    public void ToggleSocialCircleUI()
    {
       socialCircleUI.SetActive(!socialCircleUI.activeSelf);
       //currently can only press c on the first screen otherwise they will alternate toggle and never close the menu
       //characterSocialCircleUI.SetActive(!characterSocialCircleUI.activeSelf);

        //lock movement and camera while in menu
       if(socialCircleUI.activeSelf || characterSocialCircleUI.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Confined;
            DemoManager.Instance.StartCoroutine(DemoManager.togglePlayerPause());
        }
       else
        {
            Cursor.lockState = CursorLockMode.Locked;
            DemoManager.Instance.TogglePlayerControls();
        }
    }

    public void RenderSocialCircleUI()
    {
        //stuff goes here eventually
    }
}
