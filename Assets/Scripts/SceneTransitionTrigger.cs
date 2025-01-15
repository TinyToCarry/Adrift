using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionTrigger : MonoBehaviour
{
    public Animator fadeAnimator; // Animator for fading effect
    public float fadeDuration = 1.5f; // Duration of fade animation
    public int sceneIndexToLoad = 2; // Build index of the target scene

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure it's the player triggering
        {
            StartCoroutine(TransitionToScene());
        }
    }

    private IEnumerator TransitionToScene()
    {
        // Trigger fade animation
        if (fadeAnimator != null)
        {
            fadeAnimator.SetBool("isRespawning", true);
        }

        // Wait for the fade animation to complete
        yield return new WaitForSeconds(fadeDuration);

        // Load the target scene
        SceneManager.LoadScene(sceneIndexToLoad);
    }
}
