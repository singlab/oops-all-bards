using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viv;

public class CustomDictionary : MonoBehaviour
{
    [SerializeField] private List<SupertaskBindings> supertaskBindings = new List<SupertaskBindings>();
    [SerializeField] private List<BehaviorBindings> behaviorBindings = new List<BehaviorBindings>();
    [SerializeField] private List<CharacterBindings> characterBindings = new List<CharacterBindings>();
    private Dictionary<string, List<string>> supertaskDict = new Dictionary<string, List<string>>();
    private Dictionary<string, BehaviorData> behaviorDict = new Dictionary<string, BehaviorData>();
    private Dictionary<int, string> characterDict = new Dictionary<int, string>();
    private static CustomDictionary _instance;
    public static CustomDictionary Instance => _instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach (var kvp in supertaskBindings)
        {
            supertaskDict[kvp.key] = kvp.val;
        }

        foreach (var kvp in behaviorBindings)
        {
            behaviorDict[kvp.key] = kvp.val;
        }

        foreach (var kvp in characterBindings)
        {
            characterDict[kvp.key] = kvp.val;
        }
    }

    public Dictionary<string, List<string>> SupertaskDict
    {
        get { return supertaskDict; }
    }

    public Dictionary<string, BehaviorData> BehaviorDict
    {
        get { return behaviorDict; }
    }

    public Dictionary<int, string> CharacterDict
    {
        get { return characterDict; }
    }
}
