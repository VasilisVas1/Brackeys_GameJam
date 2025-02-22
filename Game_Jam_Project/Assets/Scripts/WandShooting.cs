using UnityEngine;

public class WandShooting : MonoBehaviour
{
    public GameObject magicBallPrefab;  // Assign the Magic Ball Prefab in the Inspector
    public Transform shootPoint;        // Empty GameObject at wand tip where magic spawns
    public ParticleSystem poofEffect;   // Assign the Poof Effect Prefab
    public float magicSpeed = 20f;      // Speed of the magic balls
    public AudioSource magicShoot;

    private bool canShoot = false;      // Becomes true when the player picks up the wand

    void Update()
    {
        if (canShoot && Input.GetMouseButtonDown(0)) // Left Click to shoot
        {
            ShootMagic();
        }
    }

    public void EnableShooting()
    {
        canShoot = true; // Called when the player picks up the wand
    }

    private void ShootMagic()
    {
        if (magicBallPrefab == null || shootPoint == null)
        {
            Debug.LogWarning("Magic Ball Prefab or Shoot Point is not assigned!");
            return;
        }

        // Create the magic ball at the wand's tip
        GameObject magicBall = Instantiate(magicBallPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = magicBall.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = shootPoint.forward * magicSpeed; // Shoot in the direction the player looks
            magicShoot.Play();
        }
    }
}
