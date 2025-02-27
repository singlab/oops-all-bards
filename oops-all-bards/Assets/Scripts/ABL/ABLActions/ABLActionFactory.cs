using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ABLActionFactory
{
    private static Dictionary<string, IABLAction> _actionMap = new Dictionary<string, IABLAction>()
    {
        { "Protect", new ProtectAction() },
        { "RequestAssistance", new RequestAssistanceAction() },
        { "Quip", new QuipAction() },
        { "QuestionLocals", new QuestionLocalsAction() },
        // Add other actions here...
    };

    public static IABLAction CreateAction(string actionName)
    {
        if (_actionMap.ContainsKey(actionName))
        {
            return _actionMap[actionName];
        }
        else
        {
            Debug.LogError("Unknown ABL action: " + actionName);
            return null;
        }
    }
}
