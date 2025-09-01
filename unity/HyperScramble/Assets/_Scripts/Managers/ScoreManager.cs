using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ScoreType
{
    None,
    InFlight,
    Rocket,
    RocketLaunched,
    Fuel,
    UFO,
    Mystery,
    Base
}

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Score Values")]
    [SerializeField] int inFlightScore = 10;
    [SerializeField] int rocketScore = 50;
    [SerializeField] int rocketLaunchedScore = 80;
    [SerializeField] int fuelScore = 150;
    [SerializeField] int ufoScore = 100;
    [SerializeField] int mysteryScoreBase = 100;
    [SerializeField] int baseScore = 800;   

    [Header("Extra Life Settings")]
    [SerializeField] int extraLivesThreshold = 10000;
    [SerializeField] AudioClip extraLifeSound;

    int _score;
    public int Score => _score;
    float _elapsedTime;
    Dictionary<ScoreType, int> _scoreTable;
    bool _extraLifeCollected;

    void Start()
    {
        CreateScoreTable();
        ResetScore();
    }

    void CreateScoreTable()
    {
        _scoreTable = new Dictionary<ScoreType, int>
        {
            { ScoreType.InFlight, inFlightScore },
            { ScoreType.Rocket, rocketScore },
            { ScoreType.RocketLaunched, rocketLaunchedScore },
            { ScoreType.Fuel, fuelScore },
            { ScoreType.UFO, ufoScore },
            { ScoreType.Mystery, mysteryScoreBase },
            { ScoreType.Base, baseScore }
        };
    }

    void Update()
    {
        // Check if there's a ship in the scene
        ShipController ship = GameObject.FindFirstObjectByType<ShipController>();
        if (ship == null || ship.enabled == false)
        {
            return; // No ship to add score for
        }

        _elapsedTime += Time.deltaTime;

        // Add score based on elapsed time
        if (_elapsedTime >= 1f)
        {
            _elapsedTime = 0f;
            AddScore(ScoreType.InFlight);
        }
    }

    public void AddScore(ScoreType scoreType)
    {
        int score = _scoreTable.ContainsKey(scoreType) ? _scoreTable[scoreType] : 0;

        if (scoreType == ScoreType.Mystery)
        {
            score += Random.Range(1, 4) * mysteryScoreBase; // score = 100, 200 o 300 
        }

        _score += score;
        if (_score > 99999)
        {
            _score = 99999; // Cap the score at 99999
        }

        // Check for extra lives
        if (!_extraLifeCollected && _score >= extraLivesThreshold)
        {
            _extraLifeCollected = true;

            AudioSystem.Instance.PlayFXSound(extraLifeSound);

            GameManager.Instance.AddLife();
        }

        UpdateScoreUI();
    }

    public void ResetScore()
    {
        _score = 0;
        _elapsedTime = 0f;

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = _score.ToString("D5"); // Display score as 5 digits
        }
    }
}
