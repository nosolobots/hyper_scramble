using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    [Header("Panels Settings")]
    [SerializeField] CanvasFader[] panels;
    [SerializeField] float[] panelDurations;
    [SerializeField] float delayBeforeFade = 2f;

    [Header("Audio Settings")]
    [SerializeField] AudioClip backgroundMusic;

    Controls _controls;
    Coroutine _waitAndFadeCoroutine;

    void Awake()
    {
        _controls = new Controls();
        _controls.Player.LaunchBomb.performed += _ => StartGame();
        _controls.Player.FireCannon.performed += _ => StartGame();

        _controls.Enable();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _controls.Enable();

        if (_waitAndFadeCoroutine == null)
        {
            _waitAndFadeCoroutine = StartCoroutine(WaitAndFadePanels());
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _controls.Disable();

        if (_waitAndFadeCoroutine != null)
        {
            StopCoroutine(_waitAndFadeCoroutine);
            _waitAndFadeCoroutine = null;
        }
    }

    void StartGame()
    {
        if (GeneralOptionsController.Instance.IsGamePaused) return;

        SceneManager.LoadScene(1);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var persistents = GameObject.FindGameObjectsWithTag("ReloadOnStart");
        foreach (var obj in persistents)
        {
            if (obj.scene.name == null || obj.scene.name == "DontDestroyOnLoad")
            {
                Destroy(obj);
            }
        }
    }

    void Start()
    {
        // Inicia la secuencia de fade de los panels
        if (_waitAndFadeCoroutine == null)
        {
            _waitAndFadeCoroutine = StartCoroutine(WaitAndFadePanels());
        }

        // Inicia la reproducción de música de fondo
        AudioSystem.Instance.PlayMusic(backgroundMusic);
    }

    private IEnumerator WaitAndFadePanels()
    {
        while (true)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                CanvasFader panel = panels[i];
                float duration = panelDurations[i];

                yield return new WaitForSeconds(delayBeforeFade);

                // Activate the panel and fade it in
                panel.gameObject.SetActive(true);
                panel.FadeIn();

                yield return new WaitForSeconds(duration + panel.fadeDuration);

                // Fade out the panel and deactivate it
                panel.FadeOut();
                yield return new WaitForSeconds(panel.fadeDuration);

                panel.gameObject.SetActive(false);
            }
        }
    }
}
