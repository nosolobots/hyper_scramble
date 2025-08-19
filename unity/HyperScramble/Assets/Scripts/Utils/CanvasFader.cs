using System.Collections;
using TMPro;
using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    public float fadeDuration = 1f;

    Coroutine _currentFade;
    CanvasGroup _canvasGroup;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f; // Start with the canvas hidden
    }

    public void FadeIn()
    {
        if (_currentFade != null) StopCoroutine(_currentFade);

        _currentFade = StartCoroutine(FadeCanvas(0f, 1f));
    }

    public void FadeOut()
    {
        if (_currentFade != null) StopCoroutine(_currentFade);

        _currentFade = StartCoroutine(FadeCanvas(1f, 0f));
    }

    private IEnumerator FadeCanvas(float start, float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _canvasGroup.alpha = end;
    }
}
