using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ABLActionFactory
{
    private static Dictionary<string, IABLAction> _actionMap = new Dictionary<string, IABLAction>()
    {
        { "moveToPosition", new MoveToPositionAction() },
        { "observeTarget", new ObserveTargetAction() },
        { "calmConfrontation", new CalmConfrontationAction() },
        { "aggressiveConfrontation", new AggressiveConfrontationAction() },
        { "Protect", new ProtectAction() },
        { "RequestAssistance", new RequestAssistanceAction() },
        { "Quip", new QuipAction() },
        { "moveToTarget", new MoveToTargetAction() },
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
