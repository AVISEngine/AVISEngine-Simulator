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

public class CarServer : MonoBehaviour
{
    // Sensors and props
    private CameraSensor cameraSens;
    public Sensors sensorsClass;

    // Connection information variables 
    private string connectionIP = "127.0.0.1";
    private int connectionPort = 25001;

    // Car control variables from recieved from client
    public float Steering = 0;
    public float Speed = 0;
    public float command = 0;
    public float sensor = 0;

    public float get_speed = 0;
    public float sensor_angle = 30;

    private float thisDistanceCenter;
    private float thisDistanceRight;
    private float thisDistanceLeft;

    public int current_speed;
    bool firstMovement = false;
    
    // UI Elements
    public InputField serverField;
    public InputField portField;
    public Button startButton;
    public Text logger;

    // Socket and Connenction classes
    TcpListener listener;
    IPAddress localAdd;
    TcpClient client;
    Thread mThread;
    StreamWriter theWriter;

    // Stream data variables
    string data;
    string[] responseArray = {"0","0","0","0","0","0"};
    string imageString;
    private bool running;
    byte[] image;
    string sensors;

    // Classes
    public LanguageHandler languageHandler;

    // ZMQ Variables 
    bool useZMQ = true;

    private void Start()
    {
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

        // Finding Components need to get access
        cameraSens = GetComponent<CameraSensor>();
        sensorsClass = GameObject.Find("Car Urban Tesla/SensorBox").GetComponent<Sensors>();

    }

    private void Update()
    {
        if(useZMQ){
            
        }else{
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
    }

    void startButtonEvent()
    {
        
        if (!(string.IsNullOrEmpty(serverField.text) && string.IsNullOrEmpty(portField.text)))
        {
            string theip = serverField.text;
            int theport = Int32.Parse(portField.text);

            if (theip != null && theport != null)
            {
                Debug.Log(theip);
                Debug.Log(theport);
                connectionIP = theip;
                connectionPort = theport;

                if(useZMQ){
                    //ZMQ
                    // Task task = new Task (async() => Publish());
		            // task.Start ();
                   
                }else{
                    Thread mThread = new Thread(() => GetInfo(connectionIP, connectionPort));
                    mThread.Start();
                }
                logger.text += string.Format(languageHandler.dict["StartedServerMsg"],theip,theport.ToString());
            }  
        }
        else
        {   
            if(useZMQ){
                //ZMQ
                // Task task = new Task (async() => Publish());
		        // task.Start();                
            }else{
                Thread mThread = new Thread(() => GetInfo(connectionIP, connectionPort));
                mThread.Start();
            }
            logger.text += string.Format(languageHandler.dict["StartedServerMsg"],connectionIP,connectionPort.ToString());
        }
    }
    
    void GetInfo(string theIP, int thePort)
    {
        localAdd = IPAddress.Parse(theIP);
        listener = new TcpListener(IPAddress.Any, thePort);
        listener.Start();
        client = listener.AcceptTcpClient();
        
        NetworkStream nwStream = client.GetStream();
        theWriter = new StreamWriter(nwStream);

        running = true;
        while (running)
        {
            Connection(nwStream); 
        }
        listener.Stop();
        Thread mThread = new Thread(() => GetInfo(connectionIP, connectionPort));
        mThread.Start();
    }

    void Connection(NetworkStream nwStream)
    {
        if(client == null){
            return;
        }
        try{
            client.SendBufferSize = 131072;
            client.ReceiveBufferSize = 65536;
            byte[] buffer = new byte[65536];
            
            int bytesRead = nwStream.Read(buffer, 0, 65536);
            string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            
            
            if (dataReceived != "")
            {
                if (dataReceived == "stop")
                {
                    running = false;
                }
                else 
                { 
                    responseArray = ParseResponse(dataReceived);
                    
                    float.TryParse(responseArray[2], out command);
                    float.TryParse(responseArray[3], out sensor);
                    float.TryParse(responseArray[4], out get_speed);
                    float.TryParse(responseArray[5], out sensor_angle);

                    if (command == 1)
                    {   
                        theWriter.Write("<image>" + cameraSens.cameraImageString + "</image><EOF>\n");
                        theWriter.Flush();
                    }
                    if (sensor == 1)
                    {
                        thisDistanceCenter = sensorsClass.distanceCenter;
                        thisDistanceRight = sensorsClass.distanceRight;
                        thisDistanceLeft = sensorsClass.distanceLeft;
                          
                        theWriter.Write("<sensor>" + "L:" + ((int)(thisDistanceLeft * 100f)).ToString() + ",M:" + ((int)(thisDistanceCenter * 100f)).ToString() + ",R:" + ((int)(thisDistanceRight * 100f)).ToString() + "</sensor>\n" + "\n");
                        theWriter.Flush();
                    }
                    if (get_speed == 1)
                    {                        
                        theWriter.Write("<speed>" +current_speed.ToString()+ "</speed><EOF>\n");
                        theWriter.Flush();
                    }
                    sensorsClass.frontSensorAngle = sensor_angle;
                }
            }
        }catch(SocketException socketException){ 
            print("SocketException ->" + socketException.ToString());
        }
    }

    void OnApplicationQuit()
    {
        print("Done");
    }

    void OnDestroy(){
        listener.Stop();
        client.GetStream().Close();
        client.Close();
        print("Destroy");
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

    public void Publish(string topic, string data){
        var pubSocket = new PublisherSocket();
        pubSocket.Options.SendHighWatermark = 1000;
        pubSocket.Bind("tcp://*:25005");
        print("Publisher socket binding...");
        pubSocket.SendMoreFrame("/car/steering").SendFrame("/car/steering" + "20000");
    }

    // All Topics
    public void topics(){
        var topicsDictionary = new Dictionary<string, string>{
            {"/car/steering", "sub"},
            {"/car/velocity", "sub"},
            {"/car/frontsensor/right", "pub"},
            {"/car/frontsensor/middle", "pub"},
            {"/car/frontsensor/left", "pub"},
            {"/car/frontsensor/config/angle", "sub"},
            {"/car/camera/rgb", "pub"},
            {"/car/camera/depth", "pub"},
            {"/car/camera/semantic", "pub"},
            {"/car/camera/gray", "pub"},
            {"/car/camera/config/fov", "sub"},
            {"/car/camera/config/flycam", "sub"},
            {"/car/model/config", "sub"},
            {"/world/lights/sun", "sub"},
            {"/world/traffic/intensity", "sub"},
            {"/core/topics", "pub"}
        };
    }

}


