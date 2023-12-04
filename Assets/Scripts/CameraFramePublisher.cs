using UnityEngine;
using NetMQ;
using NetMQ.Sockets;
using AsyncIO;

public class CameraFramePublisher : MonoBehaviour
{
    public Camera captureCamera;  // Assign this in the inspector or find it dynamically
    private Texture2D texture2D;
    private Rect rect;
    private PublisherSocket publisherSocket;
    private bool isRunning = true;

    // Ensure that the AsyncIO operation is being forced before any NetMQ code is executed
    void OnEnable()
    {
        ForceDotNet.Force();
        // NetMQConfig.AutoNotify = false;
        publisherSocket = new PublisherSocket();
        publisherSocket.Bind("tcp://*:12345");
    }

    void OnDisable()
    {
        isRunning = false;
        publisherSocket.Close();
        publisherSocket.Dispose();
        NetMQConfig.Cleanup();
    }

    void Start()
    {
        rect = new Rect(0, 0, captureCamera.pixelWidth, captureCamera.pixelHeight);
        texture2D = new Texture2D(captureCamera.pixelWidth, captureCamera.pixelHeight, TextureFormat.RGB24, false);
        if (captureCamera.targetTexture == null)
        {
            captureCamera.targetTexture = new RenderTexture(captureCamera.pixelWidth, captureCamera.pixelHeight, 24);
        }
    }

    void LateUpdate()
    {
        if (isRunning)
        {
            SendFrame();
        }
    }

    private void SendFrame()
    {
        // Capture the frame
        captureCamera.Render();

        RenderTexture.active = captureCamera.targetTexture;
        texture2D.ReadPixels(rect, 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;

        // Encode the frame to JPEG format
        byte[] frameBytes = texture2D.EncodeToJPG();

        // Send the topic frame
        publisherSocket.SendMoreFrame("/car/image_front").SendFrame(frameBytes);
    }
}