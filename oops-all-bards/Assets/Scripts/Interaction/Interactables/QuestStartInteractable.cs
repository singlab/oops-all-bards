using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStartInteractable : MonoBehaviour, IInteractable
{
    public int questId;
    bool triggering;
    bool triggered = false;
    public GameObject questStartPrefab;

    public void Start()
    {
        ApplyQuestStartPrefab();
    }

    public void Execute()
    {
        if (!triggered)
        {
            QuestManager.Instance.AcceptQuest(QuestManager.Instance.jsonReader.quests.GetQuest(questId));
        }
        triggered = true;
        DestroyQuestStartPrefab();
    }

    void Update()
    {
        if (triggering && Input.GetKeyDown(KeyCode.F))
        {
            Execute();
            triggering = false; // Prevent double triggering
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Use CompareTag for better performance
        {
            triggering = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggering = false;
        }
    }

    void ApplyQuestStartPrefab()
    {
        Transform target = transform.Find("CameraTarget");
        if (target == null)
        {
            Debug.LogWarning($"CameraTarget not found on NPC '{gameObject.name}'!");
            return;
        }

        GameObject questStart = Instantiate(questStartPrefab, target.position, Quaternion.identity);
        questStart.transform.SetParent(target);
    }

    void DestroyQuestStartPrefab()
    {
        Transform questStart = transform.Find("QuestStart(Clone)");
        if (questStart != null)
        {
            Destroy(questStart.gameObject);
        }
    }
}
