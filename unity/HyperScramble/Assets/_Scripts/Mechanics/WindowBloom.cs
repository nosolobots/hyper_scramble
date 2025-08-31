using Unity.VisualScripting;
using UnityEngine;

public class WindowBloom : MonoBehaviour
{
    [SerializeField] Material materialOff;
    [SerializeField] Material materialOn;
    [SerializeField] float bloomDuration = 0.5f;
    [SerializeField] GameObject[] windows;

    int currentWindowIndex = 0;
    Coroutine _bloomCoroutine;

    void Start()
    {
        if (_bloomCoroutine == null)
            _bloomCoroutine = StartCoroutine(BloomWindows());
    }
    
    void OnDisable()
    {
        if (_bloomCoroutine != null)
        {
            StopCoroutine(_bloomCoroutine);
            _bloomCoroutine = null;
        }
    }

    void OnEnable()
    {
        if (_bloomCoroutine == null)
            _bloomCoroutine = StartCoroutine(BloomWindows());
    }

    System.Collections.IEnumerator BloomWindows()
    {
        while (true)
        {
            // Turn off the current window
            windows[currentWindowIndex].GetComponent<Renderer>().material = materialOff;

            // Move to the next window
            currentWindowIndex = (currentWindowIndex + 1) % windows.Length;

            // Turn on the next window
            windows[currentWindowIndex].GetComponent<Renderer>().material = materialOn;

            // Wait for the bloom duration
            yield return new WaitForSeconds(bloomDuration);
        }
    }
}
