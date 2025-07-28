using UnityEngine;
public class WeaponsManager : Singleton<WeaponsManager>
{
    [Header("Bomb Settings")]
    [SerializeField] GameObject bombPrefab;
    [SerializeField] int maxBombs = 2;
    [SerializeField] float bombLaunchForce = 10f;
    [SerializeField] float bombLaunchAngVelZ = -0.5f;
    [SerializeField] Vector3 bombOffset = new Vector3(1f, -1f, 0);
    [SerializeField] Quaternion bombRotation = Quaternion.Euler(0, 0, 90);

    [Header("Cannon Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletLaunchForce = 40f;
    [SerializeField] float destroyDelay = 5f;
    [SerializeField] Vector3 bulletOffset = new Vector3(2f, 0f, 0);
    [SerializeField] Quaternion bulletRotation = Quaternion.Euler(0, 180, 90);

    int _bombsLeft;
    public int BombsLeft => _bombsLeft;

    void Start()
    {
        RestartBombs();
    }

    void RestartBombs()
    {
        _bombsLeft = maxBombs;
    }

    public void ReloadBomb()
    {
        if (_bombsLeft < maxBombs)
        {
            _bombsLeft++;
        }
    }

    public void LaunchBomb(Vector3 position)
    {
        if (_bombsLeft > 0)
        {
            SoundManager.Instance.PlaySound(FXSoundType.DropBomb);

            Vector3 offset = new Vector3(1f, -1f, 0);
            GameObject bomb = Instantiate(
                bombPrefab,
                position + bombOffset,
                bombRotation);

            Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
            bombRb.AddForce(transform.right * bombLaunchForce, ForceMode.Impulse);
            bombRb.angularVelocity = new Vector3(0, 0, bombLaunchAngVelZ);

            _bombsLeft--;            
        }
    }

    public void FireCannon(Vector3 position)
    {
        SoundManager.Instance.PlaySound(FXSoundType.FireLaser);

        GameObject bullet = Instantiate(
            bulletPrefab,
            position + bulletOffset,
            bulletRotation);
        
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(transform.right * bulletLaunchForce, ForceMode.Impulse);

        Destroy(bullet, destroyDelay);
    }

}