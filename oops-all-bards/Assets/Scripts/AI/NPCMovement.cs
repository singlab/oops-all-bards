using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{

    private NavMeshAgent agent;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (agent.hasPath)
        {
            if (agent.remainingDistance < 0.1)
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    void SetGoalDestination(Transform goal)
    {
        animator.SetBool("isMoving", true);
        agent.destination = goal.position;
    }

    public void SendQuintonToBackroom()
    {
        Transform goal = GameObject.Find("QuintonQuestTrigger").transform;
        SetGoalDestination(goal);
    }

    public void SendQuintonToPlayer()
    {
        Transform goal = GameObject.FindGameObjectWithTag("Player").transform;
        SetGoalDestination(goal);
    }

    public bool HasStopped()
    {
        return agent.velocity == Vector3.zero;
    }
}
