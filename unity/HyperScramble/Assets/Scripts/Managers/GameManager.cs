using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    [SerializeField] int initialFuel = 100;
    [SerializeField] int maxBombs = 2;

    int _score;
    public int Score => _score;

    int _fuel;
    public int Fuel => _fuel;

    int _bombsLeft;
    public int BombsLeft => _bombsLeft;

    protected override void Awake()
    {
        base.Awake();

        _bombsLeft = maxBombs;
    }

    public void AddScore(int points)
    {
        _score += points;
    }

    public void AddFuel(int fuel)
    {
        _fuel += fuel;
    }

    public void UseFuel(int fuel)
    {
        _fuel -= fuel;
        if (_fuel < 0) _fuel = 0;
    }

    public void DropBomb()
    {
        if (_bombsLeft > 0)
        {
            _bombsLeft--;
        }
    }

    public void ReloadBomb()
    {
        if (_bombsLeft < maxBombs)
        {
            _bombsLeft++;
        }
    }
}