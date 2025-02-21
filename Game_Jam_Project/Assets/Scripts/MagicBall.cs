using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public ParticleSystem poofEffectPrefab; // Assign the poof effect prefab in the inspector

    void OnTriggerEnter(Collider other)
    {
        // Instantiate poof effect at impact point
        if (poofEffectPrefab != null)
        {
            Instantiate(poofEffectPrefab, transform.position, Quaternion.identity);
        }

        if (other.CompareTag("Monster")) // Destroy monster when hit
    {
        Destroy(other.gameObject);
    }

        // Destroy the magic ball
        Destroy(gameObject);
    }
}
