using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public int mushroomIndex; // Set this manually in the inspector
    private GuardianMemoryPuzzle guardian;

    void Start()
    {
        guardian = FindObjectOfType<GuardianMemoryPuzzle>();
    }

    void OnMouseDown()
    {
        guardian.PlayerSelectsMushroom(mushroomIndex);
    }
}
