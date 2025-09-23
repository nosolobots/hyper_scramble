using System.Collections;
using UnityEngine;

public class FanfarriaSpawner : MonoBehaviour
{
    [SerializeField] ParticleSystem fanfarriaParticles;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] float randomDelay;
    void Start()
    {
        StartCoroutine(SpawnFanfarria());
    }

    private IEnumerator SpawnFanfarria()
    {
        while (true)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
            Vector3 parentPosition = transform.position;
            spawnPosition += parentPosition;
            ParticleSystem fanfarria = Instantiate(fanfarriaParticles, spawnPosition, Quaternion.identity);
            fanfarria.Play();

            yield return new WaitForSeconds(Random.Range(0f, randomDelay));
        }
    }
}
