using UnityEngine;
using System.Collections.Generic;

public class LIDAR : MonoBehaviour
{
    public int numHorizontalRays = 360;
    public int numVerticalRays = 180;
    public float maxDistance = 10f;

    private List<Vector3> pointCloud = new List<Vector3>();

    void Update()
    {
        ClearLines();
        SimulateLidar();
        CreatePointCloud();
    }

    void SimulateLidar()
    {
        for (int i = 0; i < numHorizontalRays; i++)
        {
            for (int j = 0; j < numVerticalRays; j++)
            {
                float horizontalAngle = i * 360f / numHorizontalRays;
                float verticalAngle = j * 180f / (numVerticalRays - 1) - 90f; // Distribute the vertical rays from -90 to 90 degrees

                Vector3 direction = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * transform.forward;

                RaycastHit hit;

                if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    pointCloud.Add(hit.point);
                }
                else
                {
                    Vector3 endPoint = transform.position + direction * maxDistance;
                    Debug.DrawLine(transform.position, endPoint, Color.green);
                }
            }
        }
    }

    void CreatePointCloud()
    {
        // Create a point cloud using a primitive Unity GameObject (e.g., empty GameObject or particle system)
        GameObject pointCloudObject = new GameObject("PointCloud");
        pointCloudObject.transform.position = Vector3.zero;

        // Add points to the point cloud GameObject
        foreach (Vector3 point in pointCloud)
        {
            GameObject pointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointObj.transform.position = point;
            pointObj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f); // Adjust the size of the points
            pointObj.transform.SetParent(pointCloudObject.transform);
        }
    }

    void ClearLines()
    {
        Debug.ClearDeveloperConsole();
    }
}