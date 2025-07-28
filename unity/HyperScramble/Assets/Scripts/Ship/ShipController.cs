using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Rendering;

public class ShipController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxHSpeed;
    [SerializeField] float maxVSpeed;
    [SerializeField] float maxYPosition = 40f;

    Rigidbody _rb;
    ShipControls _controls;
    Vector2 _movement;

    void Awake()
    {
        _controls = new ShipControls();
        _controls.Ship.LaunchBomb.performed += _ => LaunchBomb();
        _controls.Ship.FireCannon.performed += _ => FireCannon();
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    void OnEnable()
    {
        _controls.Enable();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ReadInput();
    }

    void FixedUpdate()
    {
        MoveShip();
    }

    void ReadInput()
    {
        _movement = _controls.Ship.Move.ReadValue<Vector2>();
    }

    void MoveShip()
    {
        Vector3 movementVector = new Vector3(_movement.x, _movement.y, 0) * speed * Time.fixedDeltaTime;

        _rb.AddForce(movementVector, ForceMode.Impulse);

        // Clamp ship's velocity to prevent excessive speed
        _rb.linearVelocity = new Vector3(
            Mathf.Clamp(_rb.linearVelocity.x, 0, maxHSpeed),
            Mathf.Clamp(_rb.linearVelocity.y, -maxVSpeed, maxVSpeed),
            0
        );

        // limit ship's positive y position
        if (_rb.position.y > maxYPosition)
        {
            _rb.position = new Vector3(_rb.position.x, maxYPosition, 0);
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, 0f);
        }
    }

    void LaunchBomb()
    {
        WeaponsManager.Instance.LaunchBomb(transform.position);
    }

    void FireCannon()
    {
        WeaponsManager.Instance.FireCannon(transform.position);
    }
}
