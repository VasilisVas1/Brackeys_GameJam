using UnityEngine;

public class RockTrigger : MonoBehaviour
{
    public StandingWhiteHeadRockController rockController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && rockController != null)
        {
            rockController.TriggerEmerge();
        }
    }
}
