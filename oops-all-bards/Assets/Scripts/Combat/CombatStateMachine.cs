using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateMachine : MonoBehaviour
{
    // 1. Start/Init
    // 2. Determine turn order based on all actors' Tempo stat
    // BEGIN LOOP
    // 3. Call to CiF -- get affinities of all party members towards each other
    // 4. Get health values of party
    // 5. If player turn:
    //      a. Call to ABL - Check for moments of stress (e.g. low party health, low affinity) of party members
    //          i. If stress exists for a party member, show message and calculate expected action
    //      b. Await player input
    //          i. If attacking: Call to ABL - Check for opportunities of assistance (e.g. high affinity, low enemy health)
    //      c. Call to CiF - Compare chosen action to expected action for party members under stress and adjust affinity between stressed char and controlled char
    //      e. End turn, change turn owner
    // 6. If enemy turn:
    //      a. Execute AI script           
    // EXIT LOOP ON PARTY WIPE OR ENEMY WIPE 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
