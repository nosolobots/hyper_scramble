using UnityEngine;

public class GeneralOptionsController : PersistentSingleton<GeneralOptionsController>
{
    Controls _controls;
    bool _isGamePaused;
    public bool IsGamePaused => _isGamePaused;

    protected override void Awake()
    {
        base.Awake();

        _controls = new Controls();
        _controls.Player.Sound.performed += _ => ToggleSound();
        _controls.Player.Pause.performed += _ => TogglePause();
        _controls.Player.Exit.performed += _ => Quit();

        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls?.Disable();
    }

    void OnEnable()
    {
        _controls?.Enable();
    }

    void ToggleSound()
    {
        AudioSystem.Instance.ToggleSound();
    }

    void TogglePause()
    {
        if (_isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        // Detiene el juego
        Time.timeScale = 0f;

        // Detiene la reproducción de la música
        AudioSystem.Instance.PauseMusic();

        // Si hay una nave, bloquea los controles
        ShipController ship = GameObject.FindFirstObjectByType<ShipController>();

        // Chequeamos si hay nave, porque en el menú principal no la hay
        if (ship != null)
        {
            ship.enabled = false;
        }

        _isGamePaused = true;
    }

    void ResumeGame()
    {
        // Reanuda la reproducción de la música
        AudioSystem.Instance.ResumeMusic();

        // Reanuda el juego
        Time.timeScale = 1f;

        // Si hay una nave, desbloquea los controles
        ShipController ship = GameObject.FindFirstObjectByType<ShipController>();

        // Chequeamos si hay nave, porque en el menú principal no la hay
        if (ship != null)
        {
            ship.enabled = true;
        }

        _isGamePaused = false;
    }
    
    void Quit()
    {
        Application.Quit();
    }
}
