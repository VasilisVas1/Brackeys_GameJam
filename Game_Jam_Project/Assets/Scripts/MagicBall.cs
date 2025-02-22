using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public ParticleSystem poofEffectPrefab; // Assign the poof effect prefab in the inspector
    public AudioSource monsterDead;

    void OnTriggerEnter(Collider other)
    {
        // Instantiate poof effect at impact point
        if (poofEffectPrefab != null)
        {
            Instantiate(poofEffectPrefab, transform.position, Quaternion.identity);
        }

    if (other.CompareTag("Monster")) 
{
    if (monsterDead != null)
    {
        AudioSource.PlayClipAtPoint(monsterDead.clip, other.transform.position);
    }
    Destroy(other.gameObject);
}


        Destroy(gameObject);
    }
}
