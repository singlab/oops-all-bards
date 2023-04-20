using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "Entity/NPC")]
public class NPCData : ScriptableObject
{
    [Header("Stats")]
    [SerializeField][Range(0, 5)] public float moveSpeed;
    [SerializeField][Range(0, 100)] public float rotationSpeed;

    [SerializeField][Range(0, 5)] public int rotationTimeMin;
    [SerializeField][Range(0, 5)] public int rotationTimeMax;

    [SerializeField][Range(0, 5)] public int rotationWaitMin;
    [SerializeField][Range(0, 5)] public int rotationWaitMax;

    [SerializeField][Range(0, 5)] public int walkTimeMin;
    [SerializeField][Range(0, 5)] public int walkTimeMax;

    [SerializeField][Range(0, 5)] public int walkWaitMin;
    [SerializeField][Range(0, 5)] public int walkWaitMax;

}
