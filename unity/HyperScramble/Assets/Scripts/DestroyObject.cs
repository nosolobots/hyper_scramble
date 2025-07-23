using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionPrefab;

    void OnCollisionEnter(Collision collision)
    {
        DestroyWithExplosion();
    }

    protected void DestroyWithExplosion()
    {
        // Instantiate the explosion effect at the ship's position and rotation
        if (explosionPrefab != null)
        {
            // Create the explosion effect
            ParticleSystem ps = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            ps.Play();
            // Destroy the explosion effect after it finishes playing
            Destroy(ps.gameObject, 2f);
        }

        // Destroy the ship GameObject
        Destroy(gameObject);
    }
}
