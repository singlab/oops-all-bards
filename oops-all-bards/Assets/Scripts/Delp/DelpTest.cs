using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELPTest : MonoBehaviour
{
    [SerializeField] private DELPEntity entity;
    private Queue<DELPMessage> preparedData;

    void Start()
    {
        PrepareEntityData(entity);
        if (preparedData.Count != 0 )
        {
            DELPMessage msg = preparedData.Dequeue();
            TCPTestClient.Instance.SendMessage<DELPMessage>(msg);
        }
    }

    private void PrepareEntityData(DELPEntity entity)
    {
        foreach (string fact in entity.Facts)
        {
            DELPMessage msg = new DELPMessage(0, fact);
            preparedData.Enqueue(msg);
        }

        foreach (string srule in entity.StrictRules)
        {
            DELPMessage msg = new DELPMessage(1, srule);
            preparedData.Enqueue(msg);
        }

        foreach (string drule in entity.DefeasibleRules)
        {
            DELPMessage msg = new DELPMessage(2, drule);
            preparedData.Enqueue(msg);
        }
    }
}
