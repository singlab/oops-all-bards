using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelViewer : MonoBehaviour
{

    private int index = 0;
    private int max = 4;
    private GameObject modelToRender;
    public List<GameObject> models = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        UpdateModelViewer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Increment index
    public void IncrementIndex()
    {
        this.index++;
        if (index > max)
        {
            index = 0;
        }
        UpdateModelViewer();
    }

    // Decrement index
    public void DecrementIndex()
    {
        this.index--;
        if (index < 0)
        {
            index = max;
        }
        UpdateModelViewer();
    }

    // Update the model viewer
    private void UpdateModelViewer()
    {
        modelToRender = models[index];
        RenderModel(modelToRender);
    }

    // Render given model
    public void RenderModel(GameObject model)
    {
        if (transform.childCount > 0)
        {
            GameObject currentChild = transform.GetChild(0).gameObject;
            Destroy(currentChild);
        }
        GameObject toInstantiate = Instantiate(model, transform.position, Quaternion.Euler(new Vector3(0,180,0)));
        // GameObject toInstantiate = Instantiate(model, new Vector3(transform.position.x,-1,transform.position.z), Quaternion.identity);
        toInstantiate.transform.parent = gameObject.transform;
        toInstantiate.layer = 5;
        foreach (Transform child in toInstantiate.transform)
        {
            child.gameObject.layer = 5;
        }
        toInstantiate.AddComponent<ModelRotate>();   
    }

    public GameObject GetModel()
    {
        return modelToRender;
    }
}
