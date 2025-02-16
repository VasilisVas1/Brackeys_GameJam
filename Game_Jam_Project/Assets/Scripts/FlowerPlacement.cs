using UnityEngine;

public class FlowerPlacement : MonoBehaviour
{
    void Start()
    {
        AdjustToGround();
    }

    void AdjustToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, Mathf.Infinity))
        {
            transform.position = hit.point;
        }
    }
}
