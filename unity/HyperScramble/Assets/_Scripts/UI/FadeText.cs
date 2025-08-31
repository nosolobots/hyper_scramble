using System.Collections;
using TMPro;
using UnityEngine;

public class FadeText : MonoBehaviour
{
    [SerializeField] float duration = 1.0f;
    TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        
        StartCoroutine(FadeTextCoroutine());
    }

    IEnumerator FadeTextCoroutine()
    {
        while (true)
        {
            yield return Fading(0.0f, 1.0f); // Start fading in
            yield return new WaitForSeconds(1.0f); // Wait before fading out
            yield return Fading(1.0f, 0.0f); // Start fading out
        }
    }
    IEnumerator Fading(float initialAlpha, float targetAlpha)
    {
        Color originalColor = textMesh.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / duration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }        
}
