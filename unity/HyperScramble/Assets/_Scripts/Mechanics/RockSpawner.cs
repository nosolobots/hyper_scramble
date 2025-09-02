using System.Collections;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Header("Rock Spawner Settings")]
    [SerializeField] GameObject rockPrefab;
    [SerializeField] float offsetY;
    [SerializeField] float offsetX;
    [SerializeField] float spawnInterval;
    [SerializeField] float startDelay;
    [SerializeField] float rockSpeed;
    [SerializeField] float rockLifetime = 4f;

    void Start()
    {
        StartCoroutine(SpawnRocks());
    }

    IEnumerator SpawnRocks()
    {
        // Esperamos el delay inicial
        yield return new WaitForSeconds(startDelay);

        // Obtenemos la referencia a la nave
        GameObject ship = GameObject.FindGameObjectWithTag("Ship");

        while (!GameManager.Instance.IsLevelComplete)
        {
            // No instanciamos si no hay nave
            if (ship == null)
            {
                yield return null;
                continue;
            }
            
            // Calculamos el offset vertical del spawn
            float offsetY = Random.Range(0, this.offsetY);

            // Spawneamos la roca con el offset horizontal respecto a la nave y el offset vertical
            Vector3 spawnPosition = new Vector3(
                ship.transform.position.x + offsetX,
                transform.position.y + offsetY,
                transform.position.z);

            GameObject rock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

            // Establecemos la velocidad de la roca
            rock.GetComponent<AsteroidMove>().Speed = rockSpeed;

            // Destruimos la roca pasados unos segundos
            Destroy(rock, rockLifetime);

            // Esperamos un intervalo antes de spawnear la siguiente roca
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
