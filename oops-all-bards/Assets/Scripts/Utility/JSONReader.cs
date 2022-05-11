using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class JSONReader : MonoBehaviour
{
    public List<TextAsset> files = new List<TextAsset>();
    public BaseClasses baseClasses;
 
    void Start()
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
            Debug.Log(baseClasses.baseClasses);
            foreach (BaseClass c in baseClasses.baseClasses)
            {
                Debug.Log(c.Name);
                Debug.Log(c.Description);
                Debug.Log(c.Type);
                Debug.Log(c.Stats);
            }
        }
    }
}
