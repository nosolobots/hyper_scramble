using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;

    void Update()
    {
        // Rotate the skybox around the Y-axis
        float rotationAngle = Time.time * rotationSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", rotationAngle);
    }
}
