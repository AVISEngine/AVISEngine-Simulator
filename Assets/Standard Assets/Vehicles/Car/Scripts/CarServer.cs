/*
    Car Server Class
    ________________
    This class is responsible for the connection between simulator and the client
    Two methods to establish a connection:
        1. Using ZMQ Pub-sub pattern
        2. Using AVIS Engine Client-server pattern
 */

using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArabicSupport;
using System.IO.Compression;
using NetMQ;
using NetMQ.Sockets;
using System.Threading;
using AsyncIO;

public class CarServer : MonoBehaviour {
    public float Steering = 0;
    public float Speed = 0;
    public float command = 0;
    string[] responseArray = {"0","0","0","0","0","0"};
    public float sensor = 0;
    public float get_speed = 0;
    public float sensor_angle = 30;
    public int current_speed;
    bool firstMovement = false;

    public InputField serverField;
    public InputField portField;
    public Button startButton;
    public Text logger;

    private bool running;
    private Thread netMQThread;

    // NetMQ Publisher Socket
    private PublisherSocket publisherSocket;
    public string topic = "CarControl";
    public string address = "tcp://localhost:5556";

    public LanguageHandler languageHandler;
    public CameraSensor cameraSens;
    public Sensors sensorsClass;

    private void Start() {
        // Button btn1 = startButton.GetComponent<Button>();
        // btn1.onClick.AddListener(StartServer);

        // Server Start button
        Button btn1 = startButton.GetComponent<Button>();
        btn1.onClick.AddListener(startButtonEvent);

        // Finding UI components needed to show something;
        serverField = GameObject.Find("UIPanel/Scroll View/Viewport/Content/ServerIPField").GetComponent<InputField>();
        portField = GameObject.Find("UIPanel/Scroll View/Viewport/Content/ServerPortField").GetComponent<InputField>();
        logger = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Log").GetComponent<Text>();
        
        // Initialize values into the UI Components
        languageHandler = GameObject.Find("LanguageHandler").GetComponent<LanguageHandler>();
        languageHandler.m_dictionary();
        
        // Clearing the log messages
        logger.text = "";

        if(languageHandler.lang == "fa"){
            btn1.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["StartServer"], false, false);
            portField.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["ServerPort"], false, false);
            serverField.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["ServerIP"], false, false);
        }else{
            btn1.GetComponentInChildren<Text>().text = languageHandler.dict["StartServer"];
            portField.GetComponentInChildren<Text>().text = languageHandler.dict["ServerPort"];
            serverField.GetComponentInChildren<Text>().text = languageHandler.dict["ServerIP"];
        }

      
        cameraSens = GetComponent<CameraSensor>();
        sensorsClass = GameObject.Find("Car Urban Tesla/SensorBox").GetComponent<Sensors>();

        // Initialize NetMQ for Unity
        AsyncIO.ForceDotNet.Force();
        StartServer();
    }

    private void Update()
    {
      
            float.TryParse(responseArray[0], out Speed);
            float.TryParse(responseArray[1], out Steering);
            float.TryParse(responseArray[2], out command);
            float.TryParse(responseArray[3], out sensor);
            float.TryParse(responseArray[4], out get_speed);
            float.TryParse(responseArray[5], out sensor_angle);

            current_speed = Convert.ToInt32(GetComponent<Rigidbody>().velocity.magnitude*3.6);
            if(Convert.ToInt32(current_speed) > 3 && !firstMovement){
                firstMovement = true;
                Debug.Log(current_speed);
                logger.text += "Car Moved";
            }
        
    }

    private void StartServer() {
        running = true;
        netMQThread = new Thread(ServerLoop);
        netMQThread.Start();
    }

    private void ServerLoop() {
        try {
            using (publisherSocket = new PublisherSocket()) {
                publisherSocket.Bind(address);
                
                while (running) {
                    string message = $"{Speed};{Steering};{command};{sensor};{get_speed};{sensor_angle}";
                    publisherSocket.SendMoreFrame(topic).SendFrame(message);
                    Thread.Sleep(100);
                }
            }
        } finally {
            NetMQConfig.Cleanup(); // Important to prevent Unity freeze after one use
        }
    }

    void OnDestroy() {
        running = false;
        netMQThread.Join();
        NetMQConfig.Cleanup();
    }

    void startButtonEvent() {
        if (running) {
            // If server is already running, perhaps we want to provide a way to stop it
            // StopServer();
        } else {
            // Server is not running, so start it
            StartServer();
        }
    }


    
    public static byte[] Compress(byte[] raw)
    {
        using (MemoryStream memory = new MemoryStream())
        {
            using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
            {
                gzip.Write(raw, 0, raw.Length);
            }
            return memory.ToArray();
        }
    }

    public static string[] ParseResponse(string response)
    {
        // Regex : /[a-zA-Z]+:\d+/sg
        // Template : Object1:Value1,Object2:Value2,...
        if(response != ""){
            string[] arr = Regex.Matches(response, @"-?[0-9]\d*(.\d+)?")
                .OfType<Match>()
                .Select(m => m.Groups[0].Value)
                .ToArray();
            return arr;
        }else{
            string[] arr = {"0","0","0","0","0","0"};
            return arr;
        }
        
    }
    public static void CopyTo(Stream src, Stream dest) {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
            dest.Write(bytes, 0, cnt);
        }
    }
    void OnApplicationQuit() {
        running = false;
        if (netMQThread != null && netMQThread.IsAlive) {
            netMQThread.Join();
        }
        Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
