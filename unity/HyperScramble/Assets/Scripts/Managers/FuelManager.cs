using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FuelManager : Singleton<FuelManager>
{

    [Header("Fuel UI")]
    [SerializeField] Slider fuelSlider;

    [Header("Fuel Settings")]
    [SerializeField] float maxFuel = 100f;
    [SerializeField] float consumptionFactor = 1f;
    [SerializeField] float alarmThreshold = 0.4f; // 40% of max fuel

    float _fuel;
    public float Fuel => _fuel;

    bool isOutOfFuel;

    void Start()
    {
        Restart();

        StartCoroutine(FuelMonitorWarning());
    }

    void Update()
    {
        if (isOutOfFuel) return;

        // Check if there's a ship in the scene
        ShipController ship = GameObject.FindFirstObjectByType<ShipController>();
        if (ship == null || ship.enabled == false)
        {
            return; // No ship
        }

        UseFuel(consumptionFactor * Time.deltaTime);
        UpdateFuelSlider();
    }

    private void UpdateFuelSlider()
    {
        fuelSlider.value = _fuel / maxFuel;
    }

    public void Restart()
    {
        _fuel = maxFuel;
    }

    public void AddFuel(float fuel)
    {
        _fuel += fuel;
        if (_fuel > maxFuel) _fuel = maxFuel;
    }

    public void UseFuel(float fuel)
    {
        _fuel -= fuel;
        if (_fuel < 0) _fuel = 0;
    }

    IEnumerator FuelMonitorWarning()
    {
        while (true)
        {
            if (_fuel <= 0)
            {
                // Trigger game over or stop the ship
                OutOfFuel();
                yield break; // Stop the coroutine
            }

            else if (_fuel < maxFuel * alarmThreshold && !isOutOfFuel)
            {
                // Trigger low fuel warning
                SoundManager.Instance.PlaySound(FXSoundType.FuelWarning);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void OutOfFuel()
    {
        isOutOfFuel = true;

        // Play no fuel sound
        SoundManager.Instance.PlaySound(FXSoundType.NoFuel);

        ShipController ship = GameObject.FindFirstObjectByType<ShipController>();

        if (ship != null)
        {
            ship.enabled = false; // Disable ship controls

            Rigidbody rb = ship.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.freezeRotation = false; // Allow rotation
                rb.angularVelocity = new Vector3(-0.5f, 0f, -0.2f);
                rb.useGravity = true;
            }

            ParticleSystem rocket = ship.GetComponentInChildren<ParticleSystem>();
            if (rocket != null)
            {
                rocket.Stop();
            }
        }
    }
}
