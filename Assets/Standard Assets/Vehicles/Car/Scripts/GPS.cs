using UnityEngine;

public class GPS : MonoBehaviour
{
    public float speed = 5f; // Movement speed of the object
    public float updateInterval = 0.5f; // Update interval in seconds for generating new GPS coordinates

    private float nextUpdate;
    private Vector3 lastPosition;

    // Simulated GPS coordinates
    private double latitude;
    private double longitude;

    void Start()
    {
        lastPosition = transform.position;
        nextUpdate = Time.time + updateInterval;
    }

    void Update()
    {
        // Simulate object movement
        // transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Update GPS coordinates at regular intervals
        if (Time.time > nextUpdate)
        {
            UpdateGPS();
            nextUpdate = Time.time + updateInterval;
        }
    }

    void UpdateGPS()
    {
        // Calculate displacement from the last position
        Vector3 displacement = transform.position - lastPosition;

        // Update latitude and longitude based on the displacement
        latitude += displacement.z * 0.00001; // Adjust the scale factor as needed
        longitude += displacement.x * 0.00001; // Adjust the scale factor as needed

        // Print or use the simulated GPS coordinates
        Debug.Log($"Latitude: {latitude}, Longitude: {longitude}");

        // Update the last position
        lastPosition = transform.position;
    }
}