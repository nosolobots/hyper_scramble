using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Rendering;

public class ShipController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxHSpeed;
    [SerializeField] float maxVSpeed;
    [SerializeField] float maxYPosition = 40f;

    [Header("Bomb Settings")]
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float bombLaunchForce = 10f;
    [SerializeField] float bombLaunchAngVelZ = -0.5f;

    [Header("Cannon Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletLaunchForce = 20f;
    [SerializeField] float destroyDelay = 5f;

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
        if (GameManager.Instance.BombsLeft > 0)
        {
            Vector3 offset = new Vector3(1f, -1f, 0);
            GameObject bomb = Instantiate(bombPrefab, transform.position + offset, Quaternion.Euler(0, -90, 0));
            Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
            bombRb.AddForce(-transform.right * bombLaunchForce, ForceMode.Impulse);
            bombRb.angularVelocity = new Vector3(0, 0, bombLaunchAngVelZ);

            GameManager.Instance.DropBomb();
        }
    }

    void FireCannon()
    {
        Vector3 offset = new Vector3(2f, 0f, 0);
        GameObject bullet = Instantiate(bulletPrefab, transform.position + offset, Quaternion.Euler(0, 0, 90));
        //GameObject bullet = Instantiate(bulletPrefab, transform.position + offset, Quaternion.Euler(90, 180, 0));
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(-transform.right * bulletLaunchForce, ForceMode.Impulse);
        Destroy(bullet, destroyDelay);
    }
}
