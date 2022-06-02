using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBubbleCanvas : MonoBehaviour
{
    private float destroyTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
