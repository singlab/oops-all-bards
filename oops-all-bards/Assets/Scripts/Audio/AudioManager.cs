using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private static AudioManager _instance;
    private AudioSource source;
    public static AudioManager Instance => AudioManager._instance;
    public List<AudioClip> tracks;

    // Singleton pattern
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } else if (_instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    public void ChangeTrack(int trackNum)
    {
        source.clip = tracks[trackNum];
        source.Play();
    }
}
