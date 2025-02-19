using UnityEngine;

public class FloatingRock : MonoBehaviour
{
    public float floatStrength = 0.5f; // Adjust how high the rock floats
    public float floatSpeed = 2f; // Adjust how fast the rock floats up and down

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Smooth floating effect using sine wave
        float newY = startPosition.y + (Mathf.Sin(Time.time * floatSpeed) * floatStrength);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
