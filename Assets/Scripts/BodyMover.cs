using System.Collections;
using UnityEngine;

public class BodyMover : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public Transform currentRespawn;

    public Rigidbody2D rb;
    private Animator anim;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public bool isGrounded;
    public bool isFallingToDeath;
    public bool isDead;
    public float maxFallSpeed;

    private bool isFacingRight = true;

    public bool isGrabbing;

    public float VerticalVelocity
    {
        get { return rb.velocity.y; }
    }

    private bool isCoroutineRunning = false;

    public GameObject canvasGroupObject;

    void Start()
    {
        isDead = false;
        isFallingToDeath = false;

        if (canvasGroupObject != null)
        {
            anim = canvasGroupObject.GetComponent<Animator>();
            if (anim == null)
            {
                Debug.LogError("Animator component not found on the Canvas Group.");
            }
        }
        else
        {
            Debug.LogError("Canvas Group object not assigned.");
        }
    }

    void Update()
    {
        if (!isDead)
        {
            if (rb.velocity.y < maxFallSpeed)
            {
                isFallingToDeath = true;
            }

            isGrabbing = Input.GetKey(KeyCode.E);
            float currentMoveSpeed = isGrabbing ? moveSpeed / 2 : moveSpeed;

            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * currentMoveSpeed, rb.velocity.y);

            if (!isGrabbing)
            {
                if (moveInput > 0 && !isFacingRight)
                {
                    Flip();
                }
                else if (moveInput < 0 && isFacingRight)
                {
                    Flip();
                }
            }

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (isGrounded && isFallingToDeath)
            {
                isDead = true;
            }

            if (isGrounded && !isGrabbing && Input.GetButtonDown("Jump") && !isFallingToDeath)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            if (!isGrabbing && Input.GetButtonUp("Jump") && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }

            AdjustRotationBasedOnMovement(moveInput);
        }
        else
        {
            if (!isCoroutineRunning)
            {
                StartCoroutine(Death());
            }
        }
    }

    void AdjustRotationBasedOnMovement(float moveInput)
    {
        float targetRotation;

        if (moveInput > 0)
        {
            targetRotation = -105f; // Rotate forward
        }
        else if (moveInput < 0)
        {
            targetRotation = -75f; // Rotate backward
        }
        else
        {
            targetRotation = -90f; // Reset rotation when idle
        }

        // Smoothly interpolate between current and target rotation
        rb.rotation = Mathf.LerpAngle(rb.rotation, targetRotation, Time.deltaTime * 2f);
    }


    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.y *= -1;
        transform.localScale = scaler;
    }

    IEnumerator Death()
    {
        isCoroutineRunning = true;
        rb.velocity = Vector3.zero;
        rb.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
        float targetAngle = isFacingRight ? -95 : -85;
        rb.transform.rotation = Quaternion.Euler(0, 0, targetAngle);

        yield return new WaitForSeconds(2);

        if (anim != null)
        {
            anim.SetBool("isAlive", false);
            anim.SetBool("isRespawning", true);
        }
        yield return new WaitForSeconds(1);

        rb.gravityScale = 0;
        rb.transform.position = currentRespawn.transform.position;
        rb.transform.rotation = Quaternion.Euler(0, 0, -90);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(1);

        rb.gravityScale = 1;
        isDead = false;
        isFallingToDeath = false;
        isCoroutineRunning = false;

        if (anim != null)
        {
            anim.SetBool("isRespawning", false);
            anim.SetBool("isAlive", true);
        }
        yield return new WaitForSeconds(1);
    }
}
