using System.Collections;
using UnityEditor;
using UnityEngine;

public class RocketLaunch : MonoBehaviour
{
    [Header("Rocket Launch Settings")]
    [SerializeField] float launchForce = 10f;
    [SerializeField] float launchProb = 0.5f;
    [SerializeField] float checkDelay = 0.2f;

    ParticleSystem rocketTrailPrefab;
    Rigidbody rb;
    bool isLaunched = false;

    void Start()
    {
        rocketTrailPrefab = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship") && !isLaunched)
        {
            StartCoroutine(CheckLaunch(other.transform.position));
        }
    }

    IEnumerator CheckLaunch(Vector3 shipPosition)
    {
        while (!isLaunched)
        {
            if (Random.Range(0f, 1f) < launchProb)
            {
                isLaunched = true;

                LaunchRocket(shipPosition);

                yield break; // Exit after launching
            }

            yield return new WaitForSeconds(checkDelay);
        }
    }

    void LaunchRocket(Vector3 shipPosition)
    {
        // Start the rocket trail effect
        if (rocketTrailPrefab != null)
            rocketTrailPrefab.Play();

        // Apply force to the rocket's Rigidbody in the direction of the ship
        if (rb != null)
        {
            int launchDirection = (shipPosition.x - transform.position.x) > 0 ? -1 : 1;
            float randomForce = Random.Range(0.5f, 1.5f) * launchForce;
            rb.AddForce(Vector3.up * randomForce, ForceMode.Impulse);
            float angularVelocity = Random.Range(0.25f, 0.5f) * launchDirection;
            rb.angularVelocity = new Vector3(0, 0, angularVelocity);
        }

        // Optionally, you can destroy the rocket after a certain time
        Destroy(gameObject, 5f);
    }
}
