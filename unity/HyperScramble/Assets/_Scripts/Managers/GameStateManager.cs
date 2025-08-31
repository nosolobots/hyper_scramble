using System;
using UnityEngine;

[Serializable]
public enum GameState
{
    MainMenu = 0,
    StartingLevel = 1,
    LevelComplete = 2,
    Playing = 3,
    GameOver = 4,
}

/// <summary>
/// Gestor del estado del juego que maneja los cambios de estado y eventos relacionados.
/// Permite suscribirse a eventos antes y después de cambiar el estado del juego.
/// </summary>
public class GameStateManager : PersistentSingleton<GameStateManager>
{
    /// <summary>
    /// Evento que se dispara antes de cambiar el estado del juego.
    /// </summary>
    public static event Action<GameState> OnBeforeGameStateChange;

    /// <summary>
    /// Evento que se dispara después de cambiar el estado del juego.
    /// </summary>
    public static event Action<GameState> OnAfterGameStateChanged;

    /// <summary>
    /// Referencia al estado actual del juego.
    /// </summary>
    GameState _currentGameState;
    public GameState CurrentGameState => _currentGameState;

    void Start() => ChangeGameState(GameState.MainMenu);

    public void ChangeGameState(GameState newState)
    {
        if (CurrentGameState == newState) return;

        // Notifica antes de cambiar el estado
        OnBeforeGameStateChange?.Invoke(newState);

        _currentGameState = newState;
        switch (newState)
        {
            case GameState.MainMenu:
                // Lógica para el menú principal
                HandleMainMenu();
                break;
            case GameState.StartingLevel:
                // Lógica para iniciar un nivel
                HandleStartingLevel();
                break;
            case GameState.LevelComplete:
                // Lógica tras completar un nivel
                HandleLevelComplete();
                break;
            case GameState.Playing:
                // Lógica para el juego en curso
                HandlePlaying();
                break;
            case GameState.GameOver:
                // Lógica para el fin del juego

                break;
        }

        // Notifica después de cambiar el estado
        OnAfterGameStateChanged?.Invoke(newState);
    }

    private void HandleMainMenu()
    {

    }

    private void HandleStartingLevel()
    {

    }

    private void HandleLevelComplete()
    {

    }

    private void HandlePlaying()
    {

    }
}
