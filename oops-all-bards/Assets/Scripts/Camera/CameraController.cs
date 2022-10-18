using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private OAB_KeyBoardControl keyBoardControl;
    [SerializeField] private OAB_KeyBoard_Mouse_Control keyBoardMouseControl;
    private GameObject _player;

    private void Start()
    {
        playerManager.SubscribeControlTypeChange(ChangeControlType);
    }

    public void CameraLinkPlayer(GameObject player)
    {
        _player = player;
        keyBoardControl.Player = player;
        keyBoardMouseControl.Player = player;
    }

    private void ChangeControlType(PlayerManager.ControlType controlType)
    {
        switch (controlType)
        {
            case PlayerManager.ControlType.KEYBOARD_ONLY:
                keyBoardControl.enabled = true;
                keyBoardMouseControl.enabled = false;
                break;
            case PlayerManager.ControlType.KEYBOARD_MOUSE:
                keyBoardControl.enabled = false;
                keyBoardMouseControl.enabled = true;
                break;
        }
    }
    
    public void ToggleControls()
    {
        keyBoardControl._enable = !keyBoardControl._enable;
        keyBoardMouseControl._enable = !keyBoardMouseControl._enable;
    }
    
    
}
