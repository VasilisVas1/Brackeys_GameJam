using UnityEngine;

public class RockTriggerRed : MonoBehaviour
{
    public StandingRedHeadRockController rockController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && rockController != null)
        {
            rockController.TriggerEmerge();
        }
    }
}
