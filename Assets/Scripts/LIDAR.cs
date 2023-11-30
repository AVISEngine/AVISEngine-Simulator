using UnityEngine;

public class LIDAR : MonoBehaviour
{
    public LayerMask detectionLayer;
    public int numberOfRaysVertical = 10;
    public float maxDetectionRange = 10.0f;
    public float verticalFOV = 30.0f;
    public Material pointCloudMaterial;

    private MeshFilter pointCloudMeshFilter;
    private Mesh pointCloudMesh;

    void Start()
    {
        InitializePointCloud();
    }

    void Update()
    {
        GenerateLidarPointCloud();
    }

    void InitializePointCloud()
    {
        // Create a mesh for point cloud visualization
        GameObject pointCloudObject = new GameObject("PointCloudObject");
        pointCloudObject.transform.parent = transform;

        pointCloudMeshFilter = pointCloudObject.AddComponent<MeshFilter>();
        pointCloudMesh = new Mesh();
        pointCloudMeshFilter.mesh = pointCloudMesh;

        MeshRenderer meshRenderer = pointCloudObject.AddComponent<MeshRenderer>();
        meshRenderer.material = pointCloudMaterial;
    }

    void GenerateLidarPointCloud()
    {
        Vector3[] vertices = new Vector3[numberOfRaysVertical];
        int[] indices = new int[numberOfRaysVertical];

        for (int i = 0; i < numberOfRaysVertical; i++)
        {
            float verticalAngle = i * (verticalFOV / (numberOfRaysVertical - 1)) - (verticalFOV / 2);

            // Convert local direction to world direction
            Vector3 localDirection = Quaternion.Euler(verticalAngle, 0, 0) * Vector3.forward;
            Vector3 worldDirection = transform.TransformDirection(localDirection);

            RaycastHit hit;

            if (Physics.Raycast(transform.position, worldDirection, out hit, maxDetectionRange, detectionLayer))
            {
                // Process the hit data and store it in the point cloud.
                float distance = hit.distance;
                Vector3 hitPoint = transform.position + worldDirection * distance;
                vertices[i] = hitPoint;
            }
            else
            {
                // If no hit, set the point to the max detection range.
                vertices[i] = transform.position + worldDirection * maxDetectionRange;
            }

            indices[i] = i;
        }

        // Update the point cloud mesh
        pointCloudMesh.Clear();
        pointCloudMesh.vertices = vertices;
        pointCloudMesh.SetIndices(indices, MeshTopology.Points, 0);

        // Optionally, you can recalculate normals and bounds for better rendering
        pointCloudMesh.RecalculateNormals();
        pointCloudMesh.RecalculateBounds();
    }
}