using UnityEngine;
using UnityEngine.UI;

public class FuelManager : Singleton<FuelManager>
{
    
    [SerializeField] float maxFuel = 100f;
    [SerializeField] float consumptionFactor = 1f;
    [SerializeField] Slider fuelSlider;

    float _fuel;
    public float Fuel => _fuel;

    void Start()
    {
        _fuel = maxFuel;
        UpdateFuelSlider();
    }

    void Update()
    {
        _fuel -= consumptionFactor * Time.deltaTime; // Decrease fuel over time
        if (_fuel < 0) _fuel = 0;
        UpdateFuelSlider();
    }

    public void AddFuel(int fuel)
    {
        _fuel += fuel;
        UpdateFuelSlider();
    }

    public void UseFuel(int fuel)
    {
        _fuel -= fuel;
        if (_fuel < 0) _fuel = 0;
        UpdateFuelSlider();
    }

    private void UpdateFuelSlider()
    {
        fuelSlider.value = _fuel / maxFuel;
    }
}
