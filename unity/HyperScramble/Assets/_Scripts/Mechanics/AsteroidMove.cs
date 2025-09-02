using UnityEngine;

public class AsteroidMove : MonoBehaviour
{
    float _speed;
    public float Speed { get { return _speed; } set { _speed = value; } }

    Rigidbody rockRb;

    void Start()
    {
        rockRb = GetComponentInChildren<Rigidbody>();
        rockRb.AddTorque(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5), ForceMode.Impulse);
    }

    void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);         
    }
}
