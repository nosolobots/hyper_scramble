using Unity.Cinemachine;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Settings")]
    [SerializeField] int maxLives = 3;
    [SerializeField] int extraLivesThreshold = 10000;

    [Header("Ship Settings")]
    [SerializeField] Vector3 startPosition = new Vector3(-32, 16, 0);
    [SerializeField] GameObject shipPrefab;

    [Header("Cinemachine Settings")]
    [SerializeField] CinemachineCamera cinemachineCamera;

    GameObject _playerShip;

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        // Initialize game state, spawn ship, etc.
        _playerShip = SpawnShip();

        // Set up the Cinemachine camera to follow the player ship
        Transform target = _playerShip.transform.Find("CamTarget").gameObject.transform;
        cinemachineCamera.Follow = target;
        cinemachineCamera.LookAt = target;

    }

    GameObject SpawnShip()
    {
        GameObject obj = Instantiate(shipPrefab, startPosition,
            Quaternion.Euler(0, 180f, 0));

        return obj;
    }

    void Update()
    {

    }
    

}
