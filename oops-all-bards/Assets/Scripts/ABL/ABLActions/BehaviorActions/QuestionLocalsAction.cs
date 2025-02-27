using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionLocalsAction : IABLAction
{
    public void Execute(ActionData data)
    {
        //Get data from the action data
        int actingCharacter = data.actingCharacter;
        int targetCharacter = data.targetCharacter;

        // Get the acting character, and target character's location
        // Move acting character to the targets location
        // Animate dialogue between the two characters
    }
}
