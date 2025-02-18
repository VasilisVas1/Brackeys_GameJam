using UnityEngine;

public class RockTriggerYellow : MonoBehaviour
{
    public StandingYellowHeadRockController rockController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && rockController != null)
        {
            rockController.TriggerEmerge();
        }
    }
}
