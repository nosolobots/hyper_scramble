using UnityEngine;

public class GetSize : MonoBehaviour
{
    [SerializeField] GameObject obj;

    void Start()
    {
        float width = obj.GetComponent<Renderer>().bounds.size.x;
        float height = obj.GetComponent<Renderer>().bounds.size.y;
        float depth = obj.GetComponent<Renderer>().bounds.size.z;

        Debug.Log("Width: " + width);
        Debug.Log("Height: " + height);
        Debug.Log("Depth: " + depth);    
    }
}
