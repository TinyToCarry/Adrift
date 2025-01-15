using UnityEngine;

public class ColliderHandler : MonoBehaviour
{
    private Collider2D platformCollider;
    private Transform groundCheck;

    // Start is called before the first frame update
    void Start()
    {
        platformCollider = GetComponent<Collider2D>();

        // Assuming the ground check is a child of the player and tagged as "GroundCheck"
        GameObject groundCheckObj = GameObject.FindGameObjectWithTag("GroundCheck");
        if (groundCheckObj != null)
        {
            groundCheck = groundCheckObj.transform;
        }
        else
        {
            Debug.LogError("GroundCheck not found. Ensure the player has a child object tagged as 'GroundCheck'.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (groundCheck != null)
        {
            // Enable the collider if the ground check is above the platform, otherwise disable it
            platformCollider.enabled = groundCheck.position.y > transform.position.y;
        }
    }
}

