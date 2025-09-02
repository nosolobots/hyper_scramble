using System.Collections;
using Unity.Cinemachine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;

public class GameManager : PersistentSingleton<GameManager>
{
    [Header("Game Settings")]
    [SerializeField] float delayToReloadScene = 2f;
    [SerializeField] float delayToRestartGame = 5f;

    [Header("Ship Settings")]
    [SerializeField] float startOffsetX = -30f;
    [SerializeField] float spawnOffsetX = -40f;
    [SerializeField] GameObject shipPrefab;

    [Header("Lives Settings")]
    //[SerializeField] GameObject[] shipUILives;
    [SerializeField] int livesAtStart = 3;
    [SerializeField] GameObject shipUILivesContainer;
    [SerializeField] Sprite shipUILivesImage;

    CinemachineCamera _cinemachineCamera;
    GameObject _playerShip;
    int _currentLives;
    //int _maxLives;

    bool _isLevelComplete = false;
    public bool IsLevelComplete => _isLevelComplete;

    CancellationTokenSource _cts;

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        _cts = new CancellationTokenSource();
    }

    void OnDisable()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        Debug.Log("GameManager: OnDisable called, cancelling tasks.");
    }

    /*
    void OnDisable()
    {
        Debug.Log("GameManager: OnDisable called, stopping all coroutines.");      
        StopAllCoroutines();
    }
    */

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        // Initialize game settings
        //_maxLives = shipUILives.Length;
        //_currentLives = _maxLives;
        _currentLives = livesAtStart;

        // Instantiate first level map
        //LevelMapManager.Instance.NextLeveLMap = 0;
        //LevelMapManager.Instance.LoadNextMap();

        // Initialize the scene
        try
        {
            _ = InitLevelAsync(_cts.Token);
        }
        catch (TaskCanceledException)
        {
            Debug.LogWarning("InitLevelAsync was canceled.");
        }
    }

    async Task InitLevelAsync(CancellationToken token = default)
    {
        //Debug.Log($"Initializing level; lives: {_currentLives}  maxLives: {_maxLives}");
        _isLevelComplete = false;

        // Show level start message
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        UIManager.Instance.DisplayMessage($"Level {levelIndex} Start!");

        // Show the ship UI lives icons
        ShowUILivesIcons();

        // Initialize ship
        await InitShipAsync(token);
    }

    async Task InitShipAsync(CancellationToken token = default)
    {
        // Hide the last ship UI life icon
        //shipUILives[_currentLives - 1].SetActive(false);

        // Instatiate a new player ship
        _playerShip = SpawnShip();

        if (_playerShip == null)
        {
            Debug.LogError("Failed to spawn player ship.");
        }

        // Restart the ship's fuel
        FuelManager.Instance.RestartFuel();

        // Start ship intro sequence
        //StartCoroutine(ShipIntro());
        await ShipIntro(token);

        SetupCamera();
    }

    async Task ShipIntro(CancellationToken token = default)
    {
        float time = 0f;
        float transitionDuration = 2f;

        //Debug.Log("Starting ship intro sequence.");

        // Set the initial and final positions of the ship
        Vector3 startPosition = _playerShip.transform.position;
        Vector3 endPosition = new Vector3(
            Camera.main.transform.position.x + startOffsetX,
            Camera.main.transform.position.y,
            0f
        );

        // Deactivate ship controls
        _playerShip.GetComponent<ShipController>().enabled = false;

        // Deactivate all ship colliders 
        Collider[] colliders = _playerShip.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        // Wait for the ship to be fully initialized
        while (time < transitionDuration)
        {
            time += Time.deltaTime;

            if (_playerShip == null)
            {
                Debug.LogWarning("Player ship is null during intro sequence.");
                continue;
            }

            _playerShip.transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                time / transitionDuration
            );

            // Rotate ship over the transition duration
            _playerShip.transform.rotation = Quaternion.Euler(
                Mathf.Lerp(360f, 0f, time / transitionDuration),
                180f,
                0f
            );

            await Task.Yield();
        }

        // Finalize ship initialization
        _playerShip.transform.position = endPosition;

                // Activate ship controls
        _playerShip.GetComponent<ShipController>().enabled = true;

        // Reactivate ship colliders
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    /*
    IEnumerator ShipIntro()
    {
        float time = 0f;
        float transitionDuration = 2f;

        //Debug.Log("Starting ship intro sequence.");

        // Set the initial and final positions of the ship
        Vector3 startPosition = _playerShip.transform.position;
        Vector3 endPosition = new Vector3(
            Camera.main.transform.position.x + startOffsetX,
            Camera.main.transform.position.y,
            0f
        );

        // Deactivate ship controls
        _playerShip.GetComponent<ShipController>().enabled = false;

        // Deactivate all ship colliders 
        Collider[] colliders = _playerShip.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        // Wait for the ship to be fully initialized
        while (time < transitionDuration)
        {
            time += Time.deltaTime;

            if (_playerShip == null)
            {
                Debug.LogWarning("Player ship is null during intro sequence.");
                continue;
            }

            _playerShip.transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                time / transitionDuration
            );

            // Rotate ship over the transition duration
            _playerShip.transform.rotation = Quaternion.Euler(
                Mathf.Lerp(360f, 0f, time / transitionDuration),
                180f,
                0f
            );

            yield return null;
        }

        // Finalize ship initialization
        _playerShip.transform.position = endPosition;

        // Activate ship controls
        _playerShip.GetComponent<ShipController>().enabled = true;

        // Reactivate ship colliders
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }

        SetupCamera();
    }
    */
    void SetupCamera()
    {
        // Set up the Cinemachine camera to follow the player ship
        _cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        if (_cinemachineCamera == null)
        {
            Debug.LogError("Cinemachine camera not found in the scene.");
            return;
        }

        Transform target = _playerShip.transform.Find("CamTarget").gameObject.transform;
        _cinemachineCamera.Follow = target;
        _cinemachineCamera.LookAt = target;
    }

    GameObject SpawnShip()
    {
        Vector3 position = new Vector3(
            Camera.main.transform.position.x + spawnOffsetX,
            Camera.main.transform.position.y,
            0f
        );

        GameObject obj = Instantiate(
            shipPrefab,
            position,
            Quaternion.Euler(0, 180f, 0)
        );

        //Debug.Log("Spawned player ship");

        return obj;
    }

    public void LoseLife()
    {
        _currentLives--;
        if (_currentLives <= 0)
        {
            StartCoroutine(RestartGame());
        }
        else
        {
            // Reload the current scene
            StartCoroutine(ReloadScene(SceneManager.GetActiveScene().buildIndex));
        }
    }

    IEnumerator RestartGame()
    {
        AudioSystem.Instance.StopMusic();

        // Show game over message
        UIManager.Instance.DisplayMessage("Game Over!", delayToRestartGame);

        // Wait for a short delay before reloading the scene
        yield return new WaitForSeconds(delayToRestartGame);

        // Fade out and reload the start scene
        yield return FadeOutScene();
    
        SceneManager.LoadScene(0);
    }

    IEnumerator ReloadScene(int sceneIndex, bool restart = false)
    {
        // Wait for a short delay before reloading the scene
        yield return new WaitForSeconds(delayToReloadScene);

        // Start fade out
        yield return FadeOutScene();

        // Wait for the scene to finish loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Initialize the level
        _ = InitLevelAsync();
    }

    IEnumerator FadeOutScene()
    {
        CrossFadeScene fader = FindFirstObjectByType<CrossFadeScene>();
        if (fader != null)
        {
            fader.FadeOut();
            yield return new WaitForSeconds(CrossFadeScene.FADE_DURATION);
        }
    }

    public void AddLife()
    {
        /*
        if (_currentLives >= _maxLives)
        {
            return; // Cannot add more lives than the maximum
        }
        */

        // Enable the next ship UI life icon
        /*
        if (_currentLives < shipUILives.Length)
        {
            shipUILives[_currentLives].SetActive(true);
        }
        */

        // Increment the current lives count
        _currentLives++;

        ShowUILivesIcons();
    }

    public void CompleteLevel()
    {
        if (_isLevelComplete) return; // Prevent multiple completions

        _isLevelComplete = true;
        
        // Show level complete message
        UIManager.Instance.DisplayMessage("Level Complete!", 3f);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 1; // Loop back to the first level if there are no more levels
        }

        StartCoroutine(ReloadScene(nextSceneIndex));
    }

    void ShowUILivesIcons()
    {
        if (shipUILivesContainer == null || shipUILivesImage == null)
        {
            Debug.LogWarning("Ship UI lives container or image is not assigned.");
            return;
        }

        // Clear existing icons
        foreach (Transform child in shipUILivesContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new icons based on current lives
        for (int i = 0; i < _currentLives - 1; i++)
        {
            GameObject icon = new GameObject($"LifeIcon_{i + 1}");
            icon.transform.SetParent(shipUILivesContainer.transform);
            var image = icon.AddComponent<Image>();
            image.sprite = shipUILivesImage;
            // Ajustamos la posición de cada icono respecto al contenedor
            icon.transform.localPosition = new Vector3(i * (-100) - 60, -40, 0); // Adjust spacing as needed
        }
    }
}
