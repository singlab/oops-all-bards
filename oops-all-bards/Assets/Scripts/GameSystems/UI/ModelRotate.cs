using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotate : MonoBehaviour
{
    private int speed = 15;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * (speed * Time.deltaTime));
    }
}
