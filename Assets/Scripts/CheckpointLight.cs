using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckpointLight : MonoBehaviour
{
    Light2D light2d;
    BodyMover body;

    // Start is called before the first frame update
    void Start()
    {
        light2d = GetComponent<Light2D>();
        light2d.enabled = false;

        // Assuming BodyMover is a component of the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            body = player.GetComponent<BodyMover>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && body != null)
        {
            body.currentRespawn = transform;
            light2d.enabled = true;
        }
    }
}

