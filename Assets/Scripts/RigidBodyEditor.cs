using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidBodyEditor : MonoBehaviour
{
    private Rigidbody rb;
    private float width = 200; // Width of the IMGUI window
    private float height = 100; // Height of the IMGUI window
    public bool showGUI = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnGUI()
    {
        if (!showGUI) return;

        // Calculate the bottom right corner
        float x = Screen.width - width - 20;
        float y = Screen.height - height - 20;

        // Create a GUI group on the screen at the bottom right corner
        GUILayout.BeginArea(new Rect(x, y, width, height));
        GUILayout.BeginVertical("box");

        // Output the current mass
        GUILayout.Label("Current Mass: " + rb.mass);

        // Mass Slider
        float mass = rb.mass;
        mass = GUILayout.HorizontalSlider(mass, 500.0f, 3000.0f);
        if (mass != rb.mass)
        {
            rb.mass = mass;
        }

        // Output the current drag
        GUILayout.Label("Current Drag: " + rb.drag);

        // Drag Slider
        float drag = rb.drag;
        drag = GUILayout.HorizontalSlider(drag, 0f, 10f);
        if (drag != rb.drag)
        {
            rb.drag = drag;
        }

        // Close the groups
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}