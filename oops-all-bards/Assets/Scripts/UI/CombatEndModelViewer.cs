using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEndModelViewer : MonoBehaviour
{
    public bool success;

    // Start is called before the first frame update
    void Start()
    {
        RenderPartyModels(success);
    }

    private void RenderPartyModels(bool success)
    {
        for (int i = 0; i < PartyManager.Instance.currentParty.Count; i++)
        {
            RenderModel(PartyManager.Instance.currentParty[i].Model, i, success);
        }
    }

    private void RenderModel(GameObject model, int i, bool success)
    {
        int offset = (i % 2 == 0) ? -1 : 1;
        GameObject toInstantiate = Instantiate(model, new Vector3(transform.position.x + offset, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0,180,0)), transform);
        toInstantiate.layer = 5;
        foreach (Transform child in toInstantiate.transform)
        {
            child.gameObject.layer = 5;
        }
        Animator a = toInstantiate.GetComponent<Animator>();
        if (success)
        {
            a.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"Controllers/Victory{i+1}");
        } else
        {
            a.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"Controllers/Defeat{i+1}");
        }
    }
}
