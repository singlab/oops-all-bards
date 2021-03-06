using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class JSONReader : MonoBehaviour
{
    public List<TextAsset> files = new List<TextAsset>();
    public BaseClasses baseClasses;
    public Dialogues dialogues;
    public Allies allies;
    public Quests quests;
    public Quips quips;
    
    void Awake()
    {
        foreach (TextAsset file in files)
        {
            ReadJSONFile(file);
        }
    }

    void ReadJSONFile(TextAsset file)
    {
        Debug.Log("Reading file named " + file.name);
        if (file.name == "classes")
        {
            baseClasses = JsonUtility.FromJson<BaseClasses>(file.text);
        }
        if (file.name == "dialogues")
        {
            dialogues = JsonUtility.FromJson<Dialogues>(file.text);
        }
        if (file.name == "allies")
        {
            allies = JsonUtility.FromJson<Allies>(file.text);
        }
        if (file.name == "quests")
        {
            quests = JsonUtility.FromJson<Quests>(file.text);
        }
        if (file.name == "quips")
        {
            quips = JsonUtility.FromJson<Quips>(file.text);
        }
    }
}
