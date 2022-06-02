using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackFade : MonoBehaviour
{
    public Animator animator;
    private string sceneToLoad;

    public void FadeToLevel(string sceneName)
    {
        sceneToLoad = sceneName;
        animator.SetTrigger("FadeOut");
    }

    void OnFadeComplete()
    {
        if (sceneToLoad != null) { SceneManager.LoadScene(sceneToLoad); }
    }
}
