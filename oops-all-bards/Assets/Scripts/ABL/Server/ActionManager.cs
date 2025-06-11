using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        IncomingABLMessage message = JsonConvert.DeserializeObject<IncomingABLMessage>(jsonString);

        if (message == null || string.IsNullOrEmpty(message.msg))
        {
            Debug.LogError("Failed to parse ABL message or message type is empty.");
            return;
        }

        // Convert the nested JObject back into a JSON string to pass to our actions.
        string jsonData = message.data.ToString();

        // The rest of the pipeline remains the same
        ProcessResponse(message.msg, jsonData);
    }


    // A function used to parse an ABLResponse for further processing.
    public void ProcessResponse(string messageType, string jsonData)
    {
        IABLAction action = ABLActionFactory.CreateAction(messageType);
        if (action != null)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                // Pass the extracted JSON data string to the action
                action.Execute(jsonData);
                // The listener system would also be called here
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

// A temporary class to help parse the top-level message
public class IncomingABLMessage
{
    public string msg;
    public int code;
    public JObject data;
}
