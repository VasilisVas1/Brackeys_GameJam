using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public float hopForce = 8f;    // Vertical force applied when hopping
    public float moveForce = 4f;   // Forward movement force
    public float hopInterval = 1f; // Time between hops

    public GameObject player;
    private Rigidbody rb;
    public LayerMask groundLayer; // Assign the correct ground layer in the Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        InvokeRepeating(nameof(HopTowardsPlayer), 0.5f, hopInterval);
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < 0.1f && IsGrounded())
        {
            Debug.Log("Monster seems stuck, applying extra force.");
            rb.AddForce(Vector3.up * 2f, ForceMode.Impulse); // Slight lift to unstuck
        }
    }

    private void HopTowardsPlayer()
    {
        if (player == null) return;

        // Ensure we only hop if we're grounded
        if (IsGrounded())
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Vector3 hopDirection = new Vector3(direction.x * moveForce, hopForce, direction.z * moveForce);

            // Apply an impulse force to simulate a natural hop
            rb.AddForce(hopDirection, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        float checkDistance = 1.1f;
        Vector3 origin = transform.position;

        return Physics.Raycast(origin, Vector3.down, checkDistance, groundLayer) ||
               Physics.Raycast(origin + Vector3.right * 0.2f, Vector3.down, checkDistance, groundLayer) ||
               Physics.Raycast(origin + Vector3.left * 0.2f, Vector3.down, checkDistance, groundLayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Lost!"); // Handle Game Over
            Time.timeScale = 0;        // Pause the game
        }
    }
}
