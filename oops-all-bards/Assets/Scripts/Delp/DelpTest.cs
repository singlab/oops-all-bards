using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELPTest : MonoBehaviour
{
    [SerializeField] private DELPEntity entity;
    private Queue<DELPMessage> preparedData = new Queue<DELPMessage>();

    void Start()
    {
        PrepareEntityData(entity);
    }

    void Update()
    {
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
            DelpBelief belief = new DelpBelief(fact);
            string data = JsonUtility.ToJson(belief);
            DELPMessage msg = new DELPMessage(0, "delp", data);
            preparedData.Enqueue(msg);
        }

        foreach (string srule in entity.StrictRules)
        {
            DelpBelief belief = new DelpBelief(srule);
            string data = JsonUtility.ToJson(belief);
            DELPMessage msg = new DELPMessage(1, "delp", data);
            preparedData.Enqueue(msg);
        }

        foreach (string drule in entity.DefeasibleRules)
        {
            DelpBelief belief = new DelpBelief(drule);
            string data = JsonUtility.ToJson(belief);
            Debug.Log(data);
            DELPMessage msg = new DELPMessage(2, "delp", data);
            preparedData.Enqueue(msg);
        }
    }
}

[System.Serializable]
public class DelpBelief
{
    [SerializeField] public string belief;

    public DelpBelief(string belief)
    {
        this.belief = belief;
    }
}
