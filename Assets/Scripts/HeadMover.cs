using UnityEngine;

public class HeadMover : MonoBehaviour
{
    public LayerMask interactLayer;
    public GameObject visionArea;
    public Transform headTarget;

    private Collider2D visionCollider;
    private Transform visionAreaChild; // Reference to the child GameObject's Transform

    // Start is called before the first frame update
    void Start()
    {
        visionCollider = visionArea.GetComponent<Collider2D>();
        visionAreaChild = visionArea.transform.GetChild(0); // Assuming the child is the first child
    }

    // Update is called once per frame
    void Update()
    {
        headTarget = FindClosestTarget();

        if (headTarget != null)
        {
            // Move instantly to the headTarget position
            transform.position = headTarget.position;
        }
        else
        {
            // Move instantly to the visionAreaChild position
            transform.position = visionAreaChild.position;
        }
    }

    Transform FindClosestTarget()
    {
        Collider2D[] colliders = new Collider2D[16]; // Adjust size as needed
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(interactLayer);

        int count = visionCollider.OverlapCollider(filter, colliders);

        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            float distance = Vector2.Distance(visionArea.transform.position, colliders[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = colliders[i].transform;
            }
        }

        return closestTarget;
    }
}

