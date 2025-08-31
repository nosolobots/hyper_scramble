using UnityEngine;

public class DestroyFuel : DestroyObject
{
    [SerializeField] float fuelAmount = 10f;
    void OnCollisionEnter(Collision collision)
    {
        base.DestroyWithExplosion();

        FuelManager.Instance.AddFuel(fuelAmount);
    }
}
