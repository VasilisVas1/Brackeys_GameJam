using UnityEngine;

[ExecuteInEditMode]
public class FlowerPlacementEditor : MonoBehaviour
{
    void Update()
    {
        if (!Application.isPlaying) // Only run in Edit Mode
        {
            AdjustToGround();
        }
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
