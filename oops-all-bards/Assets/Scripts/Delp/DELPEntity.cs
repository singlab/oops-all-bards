using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DELP;

[CreateAssetMenu]
public class DELPEntity : ScriptableObject
{
    [SerializeField] private List<string> facts;
    [SerializeField] private List<string> strictRules;
    [SerializeField] private List<string> defeasibleRules;
    private Queue<DELPMessage> preparedData = new Queue<DELPMessage>();

    public void AddFact(string fact)
    {
        if (!facts.Contains(fact))
        {
            Debug.Log($"<color=green>[{this.name}] Adding Fact:</color> {fact}");
            facts.Add(fact);
            PrepareAndUpdate();
        }
    }

    public void RemoveFact(string fact)
    {
        if (facts.Contains(fact))
        {
            Debug.Log($"<color=red>[{this.name}] Removing Fact:</color> {fact}");
            facts.Remove(fact);
            PrepareAndUpdate();
        }
    }

    public void AddStrictRule(string strictRule)
    {
        if (!strictRules.Contains(strictRule))
        {
            Debug.Log($"<color=green>[{this.name}] Adding Strict Rule:</color> {strictRule}");
            strictRules.Add(strictRule);
            PrepareAndUpdate();
        }
    }

    public void RemoveStrictRule(string strictRule)
    {
        if (strictRules.Contains(strictRule))
        {
            Debug.Log($"<color=red>[{this.name}] Removing Strict Rule:</color> {strictRule}");
            strictRules.Remove(strictRule);
            PrepareAndUpdate();
        }
    }

    public void AddDefeasibleRule(string defeasibleRule)
    {
        if (!defeasibleRules.Contains(defeasibleRule))
        {
            Debug.Log($"<color=green>[{this.name}] Adding Defeasible Rule:</color> {defeasibleRule}");
            defeasibleRules.Add(defeasibleRule);
            PrepareAndUpdate();
        }
    }

    public void RemoveDefeasibleRule(string defeasibleRule)
    {
        if (defeasibleRules.Contains(defeasibleRule))
        {
            Debug.Log($"<color=red>[{this.name}] Removing Defeasible Rule:</color> {defeasibleRule}");
            defeasibleRules.Remove(defeasibleRule);
            PrepareAndUpdate();
        }
    }

    public void PrepareEntityData()
    {
        preparedData.Clear();

        foreach (string fact in this.facts)
        {
            DELPBelief belief = new DELPBelief(fact);
            string data = JsonUtility.ToJson(belief);
            DELPMessage msg = new DELPMessage(0, "delp", data);
            preparedData.Enqueue(msg);
        }

        foreach (string srule in this.strictRules)
        {
            DELPBelief belief = new DELPBelief(srule);
            string data = JsonUtility.ToJson(belief);
            DELPMessage msg = new DELPMessage(1, "delp", data);
            preparedData.Enqueue(msg);
        }

        foreach (string drule in this.defeasibleRules)
        {
            DELPBelief belief = new DELPBelief(drule);
            string data = JsonUtility.ToJson(belief);
            DELPMessage msg = new DELPMessage(2, "delp", data);
            preparedData.Enqueue(msg);
        }
    }

    public void UpdateKnowledgeBase()
    {
        while (preparedData.Count != 0)
        {
            DELPMessage msg = preparedData.Dequeue();
            TCPTestClient.Instance.SendMessage<DELPMessage>(msg);
        }
    }

    public void PrepareAndUpdate()
    {
        Debug.Log($"<color=orange>[{this.name}] Preparing and sending full knowledge base update to server...</color>");
        Debug.Log(this.ToString());
        PrepareEntityData();
        UpdateKnowledgeBase();
    }

    public void QueryKnowledgeBase(string query)
    {
        DELPQuery q = new DELPQuery(query);
        DELPMessage msg = q.PrepareQuery();
        TCPTestClient.Instance.SendMessage<DELPMessage>(msg);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"--- DELP Entity State Snapshot: {this.name} ---");

        sb.AppendLine("\n== FACTS ==");
        if (facts.Count == 0) sb.AppendLine(" (None)");
        else foreach (var fact in facts) sb.AppendLine($" - {fact}");

        sb.AppendLine("\n== STRICT RULES ==");
        if (strictRules.Count == 0) sb.AppendLine(" (None)");
        else foreach (var rule in strictRules) sb.AppendLine($" - {rule}");

        sb.AppendLine("\n== DEFEASIBLE RULES ==");
        if (defeasibleRules.Count == 0) sb.AppendLine(" (None)");
        else foreach (var rule in defeasibleRules) sb.AppendLine($" - {rule}");

        sb.AppendLine("------------------------------------");
        return sb.ToString();
    }

    public List<string> Facts
    {
        get { return facts; }
    }

    public List<string> StrictRules
    {
        get { return strictRules; }
    }

    public List<string> DefeasibleRules
    {
        get { return defeasibleRules; }
    }
}
