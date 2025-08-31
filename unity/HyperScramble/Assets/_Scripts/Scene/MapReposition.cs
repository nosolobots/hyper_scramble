using UnityEngine;

public class MapReposition : MonoBehaviour
{
    [SerializeField] float xOffset;

    void Update()
    {
        if (transform.position.x < -xOffset)
        {
            // Reposition the map to the right
            transform.position = new Vector3(transform.position.x + xOffset * 2, 0, 0);
        }
    }
}
