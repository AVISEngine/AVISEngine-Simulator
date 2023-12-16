using UnityEngine;
using NetMQ;
using NetMQ.Sockets;
using System.Collections;
using System.Text;
using AsyncIO;

public class Server : MonoBehaviour
{
    private PublisherSocket publisher;
    public Camera captureCamera;
    private Texture2D texture2D;
    private bool isRunning = true;
    public int width = 512;
    public int height = 512;
    

    private Rect rect;

    void OnEnable()
    {
        ForceDotNet.Force();
        publisher = new PublisherSocket();
        publisher.Options.SendHighWatermark = 1;
        publisher.Bind("tcp://*:12345");

        StartCoroutines();
    }

    void OnDisable()
    {
        StopAllCoroutines();
        isRunning = false;
        publisher.Close();
        publisher.Dispose();
        NetMQConfig.Cleanup();
    }

    void Start()
    {
        rect = new Rect(0, 0, width, height);
        texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);
        captureCamera.targetTexture = new RenderTexture(width, height, 24);
    }

    void StartCoroutines()
    {
        StartCoroutine(PublishFrontImage());
        StartCoroutine(PublishSegmentData());
    }

    private void PublishMessage(string topic, byte[] message)
    {
        if (isRunning)
        {
            publisher.SendMoreFrame(topic).SendFrame(message);
        }
    }

    private void PublishStringMessage(string topic, string message)
    {
        if (isRunning)
        {
            publisher.SendMoreFrame(topic).SendFrame(Encoding.UTF8.GetBytes(message));
        }
    }

    IEnumerator PublishSegmentData()
    {
        while (isRunning)
        {
            yield return new WaitForEndOfFrame();

            captureCamera.Render();

            RenderTexture.active = captureCamera.targetTexture;
            texture2D.ReadPixels(rect, 0, 0);
            texture2D.Apply();
            RenderTexture.active = null;

            byte[] frameBytes = texture2D.EncodeToJPG(50);

            // Send the topic frame
            publisher.SendMoreFrame("/car/segment").SendFrame(frameBytes);

            // Wait some time before capturing the next frame
            yield return new WaitForSeconds(0.033f); // Approx 30 FPS
        }
    }
 

    IEnumerator PublishFrontImage()
    {
        while (isRunning)
        {
            yield return new WaitForEndOfFrame();

            captureCamera.Render();

            RenderTexture.active = captureCamera.targetTexture;
            texture2D.ReadPixels(rect, 0, 0);
            texture2D.Apply();
            RenderTexture.active = null;

            byte[] frameBytes = texture2D.EncodeToJPG(50);

            // Send the topic frame
            publisher.SendMoreFrame("/car/image_front").SendFrame(frameBytes);

            // Wait some time before capturing the next frame
            yield return new WaitForSeconds(0.016f); // Approx 30 FPS
        }
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