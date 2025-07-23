using UnityEngine;

public class CamTargetYLimit : MonoBehaviour
{
    [SerializeField] float posYLimit = 10f; // Limit for the Y position of the camera target
    [SerializeField] float negYLimit = -10f; // Negative limit for the
    Vector3 _parentInitialPosition;
    void Start()
    {
        _parentInitialPosition = transform.parent.position;
    }

    void Update()
    {
        Vector3 _parentPosition = transform.parent.position;
        float clampedY = Mathf.Clamp(
            _parentPosition.y,
            _parentInitialPosition.y + negYLimit,
            _parentInitialPosition.y + posYLimit);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);        
    }
}
