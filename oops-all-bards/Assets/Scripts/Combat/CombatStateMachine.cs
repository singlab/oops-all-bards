using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateMachine : MonoBehaviour
{
    // 1. Start/Init
    // 2. Determine turn order based on all actors' Tempo stat
    // BEGIN LOOP
    // 3. Call to CiF -- get affinities of all party members towards each other and audience towards player
    // 4. Get health values of party
    // 5. Audience turn -- buff?
    // 6. If player turn:
    //      a. Call to ABL - Check for moments of stress (e.g. low party health, high affinity) of party members
    //          i. If stress exists for a party member, show message and calculate expected action
    //              notes: Could do small symbol set ("peril" or "not") and use ABL to begin GetOutOfPeril routine?
    //      b. Await player input
    //          i. If attacking: Call to ABL - Check for opportunities of assistance from party/audience (e.g. high affinity, low enemy health)
    //      c. Call to CiF - Compare chosen action to expected action for party members under stress and adjust affinity between stressed char and controlled char
    //      d. Action effect changes -- calculate damage/modify stats
    //      e. End turn, change turn owner
    // 7. If enemy turn:
    //      a. Execute AI script -- single attack           
    // EXIT LOOP ON PARTY WIPE OR ENEMY WIPE
    // 8a. Win state if enemy wipe
    // 8b. Lose if party wipe

    // What kinds of WMEs+sensors does ABL need for the above to actually work?
    // 1. Party member health
    // 2. Enemy health
    // 3. Party member affinities (currently controlled character vs. others)

    // What ABL actions do we need for the above to actually work?
    // 1. AssistAttack
    // 2. AssistDefend
    // 3. Attack
    // 4. Defend
    // 5. AssistUseItem
    // 6. UseItem
    // 7. AudienceCheer
    // 8. AudienceBoo

    // What state information do we need represented in CiF (for the purposes of this demo)?
    // 1. Relationship/affinity between characters 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
