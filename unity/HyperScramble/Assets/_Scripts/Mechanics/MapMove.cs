using UnityEngine;

public class MapMove : MonoBehaviour
{
    [SerializeField] float scrollSpeed;
    
    void Update()
    {
        transform.position -= new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
    }
}
