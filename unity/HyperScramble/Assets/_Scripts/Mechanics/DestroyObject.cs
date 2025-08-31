using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionPrefab;
    //[SerializeField] FXSoundType fXSoundType = FXSoundType.None;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] public ScoreType scoreType = ScoreType.None;

    const float destroyDelay = 2f;

    void OnCollisionEnter(Collision collision)
    {
        DestroyWithExplosion();
    }

    protected void DestroyWithExplosion()
    {
        // Play sound effect if specified
        /*
        if (fXSoundType != FXSoundType.None)
        {
            SoundManager.Instance.PlaySound(fXSoundType);
        }
        */
        if (explosionSound != null)
        {
            AudioSystem.Instance.PlayFXSound(explosionSound, transform.position);
        }
        
        // Add score if specified
        if (scoreType != ScoreType.None)
        {
            ScoreManager.Instance.AddScore(scoreType);
        }

        // Instantiate the explosion effect at the ship's position and rotation
        if (explosionPrefab != null)
        {
            // Create the explosion effect
            ParticleSystem ps = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            ps.Play();
            // Destroy the explosion effect after it finishes playing
            Destroy(ps.gameObject, destroyDelay);
        }

        // Destroy the ship GameObject
        Destroy(gameObject);
    }
}
