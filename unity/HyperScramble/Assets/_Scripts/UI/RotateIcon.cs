using UnityEngine;

public class RotateIcon : MonoBehaviour
{

    void Update()
    {
        // Rotate the icon around its Y-axis at a speed of 50 degrees per second
        transform.Rotate(0, 50 * Time.deltaTime, 0);        
    }
}
