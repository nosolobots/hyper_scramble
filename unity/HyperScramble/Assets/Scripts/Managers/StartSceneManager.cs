using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    [Header("Panels Settings")]
    [SerializeField] CanvasFader[] panels;
    [SerializeField] float[] panelDurations;
    [SerializeField] float delayBeforeFade = 2f;

    void Start()
    {
        StartCoroutine(WaitAndFadePanels());
    }

    private IEnumerator WaitAndFadePanels()
    {
        while (true)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                CanvasFader panel = panels[i];
                float duration = panelDurations[i];

                yield return new WaitForSeconds(delayBeforeFade);

                // Activate the panel and fade it in
                panel.gameObject.SetActive(true);
                panel.FadeIn();

                yield return new WaitForSeconds(duration + panel.fadeDuration);

                // Fade out the panel and deactivate it
                panel.FadeOut();
                yield return new WaitForSeconds(panel.fadeDuration);

                panel.gameObject.SetActive(false);
            }
        }
    }
}
