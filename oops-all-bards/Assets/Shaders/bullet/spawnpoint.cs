using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnpoint : MonoBehaviour
{
    public GameObject firepoint;
    public List<GameObject> vfx = new List<GameObject>();
    public rotatetomouse rotateToMouse;

    private GameObject effectToSpawn;
    private float timetofire = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        effectToSpawn = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && Time.time >= timetofire)
        {
            timetofire = Time.time + 1 / effectToSpawn.GetComponent<projectilemove>().fireRate;
            SpawnVFX();
        }
    }
    void SpawnVFX()
    {
        GameObject vfx;

        if(firepoint != null)
        {
            vfx = Instantiate(effectToSpawn, firepoint.transform.position, Quaternion.identity);
            if (rotateToMouse != null)
            {
                vfx.transform.localRotation = rotateToMouse.GetRotation();
            }
        } else
        {
            Debug.Log("not firing");
        }
    }
}
