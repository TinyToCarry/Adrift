using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class TextVisibility : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        // Ensure the TextMeshProUGUI component is initialized properly
        if (tutorialText == null)
        {
            tutorialText = GetComponent<TextMeshProUGUI>();
            if (tutorialText == null)
            {
                UnityEngine.Debug.LogError("TextMeshProUGUI not found or assigned.");
                enabled = false; // Disable the script if TextMeshProUGUI is not found or assigned.
                return;
            }
        }

        // Start with fully transparent text
        tutorialText.alpha = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeOut());
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            tutorialText.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tutorialText.alpha = 1f; // Ensure it's fully visible

        UnityEngine.Debug.Log("Text faded in.");
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            tutorialText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tutorialText.alpha = 0f; // Ensure it's fully transparent

        UnityEngine.Debug.Log("Text faded out.");
    }
}
