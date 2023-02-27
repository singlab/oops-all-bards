using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class buff : MonoBehaviour
{
    public Animator anim;
    public VisualEffect powerbuff;

    private bool isbuffed;

    // Update is called once per frame
    void Update()
    {
        if (anim != null)
        {
            if (Input.GetKeyDown("space") && !isbuffed)
            {
                anim.SetTrigger("PianoSmack");

                if (powerbuff != null)
                {
                    powerbuff.Play();
                }
                isbuffed = true;
                StartCoroutine(ResetBool(isbuffed, 0.5f));
            }
        }
    }
        


    IEnumerator ResetBool( bool boolToReset, float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        isbuffed = !isbuffed;
    }
}
