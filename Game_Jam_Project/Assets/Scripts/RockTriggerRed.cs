using UnityEngine;

public class RockTriggerRed : MonoBehaviour
{
    public StandingRedHeadRockController rockController;

    //TEST TEST TEST TEST
    public GameObject blueTrigger, whiteTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && rockController != null)
        {
            //TEST TEST TEST TEST
            blueTrigger.SetActive(false);
            whiteTrigger.SetActive(false);
            //TEST TEST TEST TEST

            rockController.TriggerEmerge();
        }
    }
}
