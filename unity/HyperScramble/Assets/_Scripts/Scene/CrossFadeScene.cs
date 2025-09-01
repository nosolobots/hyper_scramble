using System.Collections;
using UnityEngine;

public class CrossFadeScene : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    public const float FADE_DURATION = 0.5f;

    void Start()
    {
        FadeIn(FADE_DURATION);
    }

    public void FadeOut(float duration = FADE_DURATION)
    {
        StartCoroutine(FadeSequence(duration, 0f, 1f));
    }

    public void FadeIn(float duration = FADE_DURATION)
    {
        StartCoroutine(FadeSequence(duration, 1f, 0f));
    }

    IEnumerator FadeSequence(float duration, float start, float end)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = start;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(Mathf.Abs(start - elapsedTime / duration));
            yield return null;
        }
        canvasGroup.alpha = end;
    }
}
