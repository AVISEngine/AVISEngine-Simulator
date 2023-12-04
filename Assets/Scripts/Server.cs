using UnityEngine;
using NetMQ;
using NetMQ.Sockets;
using System.Text;

public class Server : MonoBehaviour
{
    private PublisherSocket publisher;
    public Camera captureCamera;  
        private Texture2D texture2D;

    private Rect rect;
    void Start()
    {
        publisher = new PublisherSocket();
        publisher.Bind("tcp://*:12345"); // Replace with the appropriate IP and port

        // Start publishing messages
        PublishScanData();
        PublishSteeringData();
        PublishVelocityData();
        PublishFrontImage();
        PublishSegmentData();
        PublishDepthData();
    }

    void PublishMessage(string topic, string message)
    {
        publisher.SendMoreFrame(topic).SendFrame(message);
    }

    void PublishScanData()
    {
        // Assuming scan data is being collected and stored in a variable named "scanData"
        string scanData = "example scan data";
        PublishMessage("/car/scan", scanData);
    }
    
    

    void PublishSteeringData()
    {
        // Assuming steering data is being collected and stored in a variable named "steeringData"
        string steeringData = "example steering data";
        PublishMessage("/car/steering", steeringData);
    }

    void PublishVelocityData()
    {
        // Assuming velocity data is being collected and stored in a variable named "velocityData"
        string velocityData = "example velocity data";
        PublishMessage("/car/vel", velocityData);
    }


    void PublishFrontImage()
    {
        // Assuming the front camera image is represented as a byte array named "frontImageData"
        // byte[] frontImageData = GetFrontImageData();
        // publisher.SendMoreFrame("/car/image_front").SendFrame(frontImageData);
        // Capture the frame
        captureCamera.Render();
        rect = new Rect(0, 0, captureCamera.pixelWidth, captureCamera.pixelHeight);
        texture2D = new Texture2D(captureCamera.pixelWidth, captureCamera.pixelHeight, TextureFormat.RGB24, false);

        RenderTexture.active = captureCamera.targetTexture;
        texture2D.ReadPixels(rect, 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;

        // Encode the frame to JPEG format
        byte[] frameBytes = texture2D.EncodeToJPG();

        // Send the topic frame
        publisher.SendMoreFrame("/car/image_front").SendFrame(frameBytes);
    }

    void PublishSegmentData()
    {
        // Assuming segment data is being collected and stored in a variable named "segmentData"
        string segmentData = "example segment data";
        PublishMessage("/car/segment", segmentData);
    }

    void PublishDepthData()
    {
        // Assuming depth data is being collected and stored in a variable named "depthData"
        string depthData = "example depth data";
        PublishMessage("/car/depth", depthData);
    }

    byte[] GetFrontImageData()
    {
        // Placeholder method to return an example front camera image as a byte array
        // Replace this with your actual image capture and conversion logic
        rect = new Rect(0, 0, captureCamera.pixelWidth, captureCamera.pixelHeight);
        texture2D = new Texture2D(captureCamera.pixelWidth, captureCamera.pixelHeight, TextureFormat.RGB24, false);
        // Load or capture the image and convert it to a byte array
        // captureCamera.targetTexture = new RenderTexture(captureCamera.pixelWidth, captureCamera.pixelHeight, 24);

        byte[] imageData = texture2D.EncodeToPNG(); // Use the appropriate encoding for your images
        return imageData;
    }

    void OnDestroy()
    {
        if (publisher != null)
        {
            publisher.Close();
            publisher.Dispose();
        }
    }
}