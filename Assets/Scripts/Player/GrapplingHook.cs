using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 20f;
    public float grappleSpeed = 30f;
    public float climbDistance = 2f;
    public LineRenderer lineRenderer;

    private Vector3 grappleTarget;
    private bool isGrappling = false;
    private bool isPulling = false;
    private Rigidbody playerRb;

    void Start()
    {
        if (!player) player = transform.root;
        playerRb = player.GetComponent<Rigidbody>();

        if (!lineRenderer)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 0.05f;
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isGrappling)
        {
            TryStartGrapple();
        }

        if (isPulling)
        {
            PullPlayerTowardGrapple();
        }
    }

    void TryStartGrapple()
    {
        Collider[] hits = Physics.OverlapSphere(player.position, detectionRadius);
        Transform nearestRoof = null;
        float closestDist = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Roof"))
            {
                float dist = Vector3.Distance(player.position, hit.ClosestPoint(player.position));
                if (dist < closestDist)
                {
                    closestDist = dist;
                    nearestRoof = hit.transform;
                    grappleTarget = hit.ClosestPoint(player.position); // get nearest point on surface
                }
            }
        }

        if (nearestRoof != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, player.position);
            lineRenderer.SetPosition(1, player.position);
            isGrappling = true;
            StartCoroutine(ShootCable(grappleTarget));
        }
    }

    System.Collections.IEnumerator ShootCable(Vector3 target)
    {
        float t = 0f;
        Vector3 start = player.position;

        while (t < 1f)
        {
            t += Time.deltaTime * grappleSpeed;
            Vector3 point = Vector3.Lerp(start, target, t);
            lineRenderer.SetPosition(1, point);
            yield return null;
        }

        isPulling = true;
    }

    void PullPlayerTowardGrapple()
    {
        Vector3 dir = (grappleTarget - player.position).normalized;
        float dist = Vector3.Distance(player.position, grappleTarget);

        playerRb.linearVelocity = dir * grappleSpeed;

        lineRenderer.SetPosition(0, player.position);
        lineRenderer.SetPosition(1, grappleTarget);

        if (dist < climbDistance)
        {
            // Stop pulling
            isPulling = false;
            isGrappling = false;
            lineRenderer.enabled = false;

            // Climb/jump onto roof
            player.position = grappleTarget + Vector3.up * 1f;
            playerRb.linearVelocity = Vector3.zero;
        }
    }
}
