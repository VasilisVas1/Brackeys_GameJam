using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(FlowerPlacement))]
public class FlowerPlacementEditor : MonoBehaviour
{
    void OnValidate()
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
