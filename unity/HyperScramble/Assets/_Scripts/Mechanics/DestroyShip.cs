using UnityEngine;

public class DestroyShip : DestroyObject
{
    void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.GodMode)
            return;

        base.DestroyWithExplosion();

        GameManager.Instance.LoseLife();
    }
}
