using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectilemove : MonoBehaviour
{
    public float speed;
    public float fireRate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        } else
        {
            Debug.Log("not moving");
        }
    }
    void OnCollisionEnter (Collision co)
    {
        speed = 0;
        Destroy(gameObject);
    }
}
