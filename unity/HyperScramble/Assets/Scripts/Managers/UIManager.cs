using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Main Text")]
    [SerializeField] TextMeshProUGUI mainText;

    const float msgDefaultDuration = 2f;

    public void DisplayMessage(string text, float duration = msgDefaultDuration)
    {
        StartCoroutine(DisplayMessageCoroutine(text, duration));
    }

    IEnumerator DisplayMessageCoroutine(string text, float duration)
    {
        mainText.text = text;

        yield return new WaitForSeconds(duration);

        StartCoroutine(FadeOutMessage(2f));
    }

    IEnumerator FadeOutMessage(float duration)
    {
        float elapsedTime = 0f;
        Color color = mainText.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            mainText.color = color;
            yield return null;
        }

        // Clear the text after fading out
        mainText.text = string.Empty;

        // Restore the alpha value to 1
        color.a = 1f;
        mainText.color = color;
    }

}
