using UnityEngine;

public class LegMover : MonoBehaviour
{
    public Transform legTarget;
    public LayerMask groundLayer;
    public float hoverDist;
    public float groundCheckDistance;
    public float legMoveDist;
    public Vector3 halfWayPoint;
    public float liftDistance;

    public float legMovementSpeed;
    public int posIndex;
    public GameObject defaultPos;
    public GameObject defaultPosIdle;
    public bool grounded;
    public LegMover opposingLeg;
    public BodyMover body;
    public AudioSource aud;

    private float originalLegMoveDist;

    // Start is called before the first frame update
    void Start()
    {
        originalLegMoveDist = legMoveDist; // Store the original legMoveDist
        aud = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        if (body.isGrounded)
        {
            // Adjust legMoveDist if body is grabbing
            float currentLegMoveDist = body.isGrabbing ? originalLegMoveDist / 2 : originalLegMoveDist;

            if (Mathf.Abs(body.rb.velocity.x) > 0.1)
            {
                if (Vector2.Distance(transform.position, legTarget.position) > currentLegMoveDist && posIndex == 0 && opposingLeg.grounded == true)
                {
                    halfWayPoint = transform.position;
                    halfWayPoint.y += liftDistance;
                    posIndex = 1;
                }
                else if (posIndex == 1)
                {
                    legTarget.position = Vector3.Lerp(legTarget.position, halfWayPoint, legMovementSpeed * Time.deltaTime);

                    if (Vector2.Distance(legTarget.position, halfWayPoint) <= 0.2f)
                    {
                        posIndex = 2;
                    }
                }
                else if (posIndex == 2)
                {
                    legTarget.position = Vector3.Lerp(legTarget.position, transform.position, legMovementSpeed * Time.deltaTime);

                    if (Vector2.Distance(legTarget.position, transform.position) < 0.2f)
                    {
                        aud.pitch = Random.Range(1.0f, 1.2f);
                        aud.Play();
                        posIndex = 0;
                    }
                }

                if (posIndex == 0)
                {
                    grounded = true;
                }
                else
                {
                    grounded = false;
                }
            }
            else
            {
                legTarget.position = Vector3.Lerp(legTarget.position, transform.position, legMovementSpeed * Time.deltaTime);
                halfWayPoint = transform.position;
            }
        }
        else
        {
            legTarget.position = Vector3.Lerp(legTarget.position, transform.position, legMovementSpeed * Time.deltaTime);
            halfWayPoint = transform.position;
        }
    }

    public void CheckGround()
    {
        if (body.isGrounded)
        {
            if (Mathf.Abs(body.rb.velocity.x) < 0.1 || body.isGrabbing)
            {
                transform.position = defaultPosIdle.transform.position;
            }
            else
            {
                transform.position = defaultPos.transform.position;
            }
            Debug.DrawRay(gameObject.transform.position, Vector3.down * groundCheckDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, Vector3.down, groundCheckDistance, groundLayer);
            if (hit.collider != null)
            {
                Vector3 point = hit.point; // gets the position where the leg hit something
                point.y += hoverDist;
                transform.position = point;
            }
        }
        else
        {
            transform.position = defaultPos.transform.position;
        }
    }
}

