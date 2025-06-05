using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private static ActionManager _instance;
    public static ActionManager Instance => ActionManager._instance;
    private List<IActionListener> _actionListeners = new List<IActionListener>();

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

    public void HandleABLResponse(string jsonString)
    {
        ABLResponse response = JsonUtility.FromJson<ABLResponse>(jsonString);
        ProcessResponse(response);
    }


    // A function used to parse an ABLResponse for further processing.
    public void ProcessResponse(ABLResponse response)
    {
        IABLAction action = ABLActionFactory.CreateAction(response.msg);
        if (action != null)
        {
            // Execute the action on the main thread, to avoid errors.
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                action.Execute(response.data);
                NotifyActionListeners(response.msg, response.data);
            });
        }
    }

    public void AddActionListener(IActionListener listener)
    {
        _actionListeners.Add(listener);
    }

    public void RemoveActionListener(IActionListener listener)
    {
        _actionListeners.Remove(listener);
    }

    private void NotifyActionListeners(string actionName, string jsonData)
    {
        ActionEventData eventData = null;

        // Create the correct data type based on the action name
        switch (actionName)
        {
            case "moveToPosition":
                eventData = JsonUtility.FromJson<MoveToPositionData>(jsonData);
                break;
            case "observeTarget":
                eventData = JsonUtility.FromJson<ObserveTargetData>(jsonData);
                break;
            case "calmConfrontation":
            case "aggressiveConfrontation":
                eventData = JsonUtility.FromJson<ConfrontationData>(jsonData);
                break;
            case "Protect":
                eventData = JsonUtility.FromJson<ProtectData>(jsonData);
                break;
            case "RequestAssistance":
                eventData = JsonUtility.FromJson<RequestAssistanceData>(jsonData);
                break;
            case "Quip":
                eventData = JsonUtility.FromJson<QuipData>(jsonData);
                break;
        }

        if (eventData != null)
        {
            foreach (IActionListener listener in _actionListeners)
            {
                listener.OnActionExecuted(actionName, eventData);
            }
        }
    }
}
