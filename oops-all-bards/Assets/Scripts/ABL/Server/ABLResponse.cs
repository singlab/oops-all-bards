using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ABLResponse
{
    // The code returned by the Java server.
    public int code;
    // The message returned by the Java server.
    public string msg;
    // The data represented as a string returned by the Java server.
    public ActionData data;
}

// A class used to hold various things sent as part of the data field of an ABLResponse.
[System.Serializable]
public class ActionData 
{
    // The character ID of the acting character, if any
    public int actingCharacter;
    // The character ID of the target character, if any
    public int targetCharacter;
    // Whether or not the acting character is in combat, if relevant
    public bool inCombat;
}
