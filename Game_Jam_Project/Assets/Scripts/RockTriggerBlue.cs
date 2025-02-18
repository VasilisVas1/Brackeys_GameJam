using UnityEngine;

public class RockTriggerBlue : MonoBehaviour
{
    public StandingBlueHeadRockController rockController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && rockController != null)
        {
            rockController.TriggerEmerge();
        }
    }
}
