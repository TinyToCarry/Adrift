using UnityEngine;

public class ArmMover : MonoBehaviour
{
    public LayerMask interactLayer;

    public float armMovementSpeed;
    public GameObject armTargetPoint1;
    public GameObject armTargetPoint2;
    public GameObject armTargetPointMid;
    public BodyMover body;
    public LegMover sameSideLegMover;
    public LegMover oppSideLegMover;

    private float sameSideLegLastUpdate;
    private float oppSideLegLastUpdate;

    public GameObject grabArea;
    public Transform armGrabTarget;

    private Collider2D grabCollider;

    // Start is called before the first frame update
    void Start()
    {
        sameSideLegLastUpdate = Time.time;
        oppSideLegLastUpdate = Time.time;
        grabCollider = grabArea.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        armGrabTarget = FindClosestTarget();
        if (armGrabTarget != null)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, armGrabTarget.transform.position, armMovementSpeed * Time.deltaTime);
        }
        else
        {
            if (Mathf.Abs(body.rb.velocity.x) > 0.1)
            {
                // Check which leg mover was updated last
                if (sameSideLegMover.posIndex == 0)
                {
                    sameSideLegLastUpdate = Time.time;
                }

                if (oppSideLegMover.posIndex == 0)
                {
                    oppSideLegLastUpdate = Time.time;
                }

                // Determine which leg mover was updated most recently
                if (sameSideLegLastUpdate > oppSideLegLastUpdate)
                {
                    gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, armTargetPoint1.transform.position, armMovementSpeed * Time.deltaTime);
                }
                else if (oppSideLegLastUpdate > sameSideLegLastUpdate)
                {
                    gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, armTargetPoint2.transform.position, armMovementSpeed * Time.deltaTime);
                }
            }
            else
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, armTargetPointMid.transform.position, armMovementSpeed * Time.deltaTime);
            }
        }

        
    }

    Transform FindClosestTarget()
    {
        Collider2D[] colliders = new Collider2D[16]; // Adjust size as needed
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(interactLayer);

        int count = grabCollider.OverlapCollider(filter, colliders);

        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            float distance = Vector2.Distance(grabArea.transform.position, colliders[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = colliders[i].transform;
            }
        }

        return closestTarget;
    }
}
