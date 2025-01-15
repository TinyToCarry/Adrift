using UnityEngine;

public class SpringJoint2DController : MonoBehaviour
{
    public float activationDistance; // Configurable distance for activation
    private SpringJoint2D springJoint2D;
    private GameObject player;
    private Rigidbody2D playerRigidbody2D;
    private Rigidbody2D rb2d;

    // Mass values
    public float unattachedMass;
    private const float attachedMass = 1f;

    void Start()
    {
        springJoint2D = GetComponent<SpringJoint2D>();
        if (springJoint2D == null)
        {
            Debug.LogError("No SpringJoint2D component found on this GameObject.");
        }

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No GameObject found with the tag 'Player'.");
        }
        else
        {
            playerRigidbody2D = player.GetComponent<Rigidbody2D>();
            if (playerRigidbody2D == null)
            {
                Debug.LogError("The Player GameObject does not have a Rigidbody2D component.");
            }
        }

        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("No Rigidbody2D component found on this GameObject.");
        }
    }

    void Update()
    {
        if (springJoint2D != null && playerRigidbody2D != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance <= activationDistance && Input.GetKey(KeyCode.E))
            {
                springJoint2D.connectedBody = playerRigidbody2D; // Connect the player's Rigidbody2D
                rb2d.mass = attachedMass; // Set mass to 1 when attached
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                springJoint2D.connectedBody = null; // Disconnect the player's Rigidbody2D
                rb2d.mass = unattachedMass; // Set mass to the specified value when not attached
            }

            // Enable or disable the SpringJoint2D based on whether it has a connected body
            springJoint2D.enabled = (springJoint2D.connectedBody != null);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
}


