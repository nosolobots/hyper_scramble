using UnityEngine;

public class DestroyShip : DestroyObject
{
    void OnCollisionEnter(Collision collision)
    {
        base.DestroyWithExplosion();

        GameManager.Instance.LoseLife();
    }
}
