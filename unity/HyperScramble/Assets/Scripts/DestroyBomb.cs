
using UnityEngine;

public class DestroyBomb : DestroyObject
{
    void OnCollisionEnter(Collision collision)
    {
        base.DestroyWithExplosion();

        WeaponsManager.Instance.ReloadBomb();
    }
}