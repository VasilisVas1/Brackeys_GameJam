using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public float hopForce = 8f;    // Vertical force applied when hopping
    public float moveForce = 4f;   // Forward movement force
    public float hopInterval = 1f; // Time between hops
    public float unstuckBoost = 2f; // Extra push to get unstuck

    public GameObject player;
    private Rigidbody rb;
    private AudioSource audioSource; // Audio source for jump sound


    private RespawnManager respawnManager; // Reference to RespawnManager

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        respawnManager = FindObjectOfType<RespawnManager>(); // Find the RespawnManager in the scene

        InvokeRepeating(nameof(HopTowardsPlayer), 0.5f, hopInterval);
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < 0.1f && IsGrounded())
        {
            Debug.Log("Monster seems stuck, applying unstuck force.");
            
            Vector3 unstuckForce = Vector3.up * (hopForce / 2) + transform.forward * unstuckBoost;
            rb.AddForce(unstuckForce, ForceMode.Impulse);
        }
    }

    private void HopTowardsPlayer()
    {
        if (player == null) return;

        if (IsGrounded())
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Vector3 hopDirection = new Vector3(direction.x * moveForce, hopForce, direction.z * moveForce);

            rb.AddForce(hopDirection, ForceMode.Impulse);

            // Play jump sound
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
    }

    private bool IsGrounded()
    {
        float checkDistance = 3.3f;

        return Physics.Raycast(transform.position, Vector3.down, checkDistance) ||
               Physics.Raycast(transform.position + Vector3.right * 0.3f, Vector3.down, checkDistance) ||
               Physics.Raycast(transform.position + Vector3.left * 0.3f, Vector3.down, checkDistance) ||
               Physics.Raycast(transform.position + Vector3.forward * 0.3f, Vector3.down, checkDistance) ||
               Physics.Raycast(transform.position + Vector3.back * 0.3f, Vector3.down, checkDistance);
    }

/*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Lost!"); 
            
            // Call Game Over from RespawnManager
            if (respawnManager != null)
            {
                respawnManager.RespawnPlayer();
            }
            else
            {
                Debug.LogError("RespawnManager not found in the scene!");
            }
        }
    }
*/
private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        Debug.Log("Player hit by a monster! Respawning...");

        // Call RespawnPlayer instead of handling game over
        if (respawnManager != null)
        {
            respawnManager.RespawnPlayer();
        }
        else
        {
            Debug.LogError("RespawnManager not found in the scene!");
        }
    }
}

}
