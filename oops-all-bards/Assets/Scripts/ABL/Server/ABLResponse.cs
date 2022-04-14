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
    // This could be for example a JSON representation of an ABLAction.
    public string data;
}
