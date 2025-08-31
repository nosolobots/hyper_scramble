using System.Collections;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    [SerializeField] float cycleTime;
    [SerializeField] float horizontalAmplitude;
    [SerializeField] float verticalHeight;
    [SerializeField] float horizontalSpeed;

    Coroutine _waveCoroutine;

    void Start()
    {
        if (_waveCoroutine == null)
            _waveCoroutine = StartCoroutine(WaveMovement());
    }

    void OnDisable()
    {
        if (_waveCoroutine != null)
        {
            StopCoroutine(_waveCoroutine);
            _waveCoroutine = null;
        }
    }

    void OnEnable()
    {
        if (_waveCoroutine == null)
            _waveCoroutine = StartCoroutine(WaveMovement());
    }

    IEnumerator WaveMovement()
    {
        Vector3 startPosition = transform.position;
        float time = 0f;

        while (true)
        {
            while (time < cycleTime)
            {
                float y = Mathf.Sin((time * 2/ cycleTime) * Mathf.PI) * verticalHeight;
                float x = Mathf.Sin((time * 4/ cycleTime) * Mathf.PI) * horizontalAmplitude;
                startPosition.x -= horizontalSpeed * Time.deltaTime;
                transform.position = startPosition + new Vector3(x, y, 0);
                time += Time.deltaTime;
                yield return null;
            }
            time = 0f;
            transform.position = new Vector3(transform.position.x, startPosition.y, startPosition.z);   
        }
    }
}
