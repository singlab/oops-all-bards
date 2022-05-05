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

// A class used to hold the characters IDs sent as part of the data field of an ABLResponse.
[System.Serializable]
public class ActionData 
{
    public int actingCharacter;
    public int targetCharacter;
}
