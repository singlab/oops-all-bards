using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionListener
{
    void OnActionExecuted(string actionName, ActionData data);
}
