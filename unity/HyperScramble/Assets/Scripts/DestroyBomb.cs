
using UnityEngine;

public class DestroyBomb : DestroyObject
{
    void OnCollisionEnter(Collision collision)
    {
        base.DestroyWithExplosion();

        GameManager.Instance.ReloadBomb();
    }
}