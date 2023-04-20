using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderNPCAI : MonoBehaviour
{

    private bool isWandering = false;

    private const string idleAnimation = "Idle";
    private const string walkAnimation = "Walk";

    private enum State { Wandering, RotatingLeft, RotatingRight, Walking, Idle };
    private State state;

    private Animator animator;

    [SerializeField] private NPCData npcData;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator == null) return;

        if (!isWandering)
        {
            StartCoroutine(Wander());
        }

        if (state == State.RotatingRight)
        {
            animator.Play(idleAnimation);
            transform.Rotate(transform.up * Time.deltaTime * npcData.rotationSpeed);
        }
        else if (state == State.RotatingLeft)
        {
            animator.Play(idleAnimation);
            transform.Rotate(transform.up * Time.deltaTime * -npcData.rotationSpeed);
        }
        else if (state == State.Walking)
        {
            animator.Play(walkAnimation);
            transform.position += transform.forward * npcData.moveSpeed * Time.deltaTime;
        }
        else if (state == State.Idle)
        {
            animator.Play(idleAnimation);
        }
    }

    IEnumerator Wander()
    {
        int rotTime = Random.Range(npcData.rotationTimeMin, npcData.rotationTimeMax);
        int rotateWait = Random.Range(npcData.rotationWaitMin, npcData.rotationWaitMax);
        int walkWait = Random.Range(npcData.walkWaitMin, npcData.walkWaitMax);
        int walkTime = Random.Range(npcData.walkTimeMin, npcData.walkTimeMax);
        int rotateLorR = Random.Range(1, 2);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        state = State.Walking;
        yield return new WaitForSeconds(walkTime);
        state = State.Idle;
        yield return new WaitForSeconds(rotateWait);
        if (rotateLorR == 1)
        {
            state = State.RotatingRight;
            yield return new WaitForSeconds(rotTime);
            state = State.Idle;
        }
        if (rotateLorR == 2)
        {
            state= State.RotatingLeft;
            yield return new WaitForSeconds(rotTime);
            state = State.Idle;
        }
        isWandering = false;
    }
}