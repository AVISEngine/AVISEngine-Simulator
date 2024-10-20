using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SQLite4Unity3d;
using Newtonsoft.Json;

public class SimpleWebSocketServer : MonoBehaviour
{
    private WebSocketServer wss;
    private Dictionary<string, List<string>> subscribers = new Dictionary<string, List<string>>();

    // Car components
    private CameraSensor cameraSens;
    public Sensors sensorsClass;

    // Car control variables
    public float Steering = 0;
    public float Speed = 0;
    public float command = 0;
    public float sensor = 0;
    public float get_speed = 0;
    public float sensor_angle = 30;
    public int current_speed;

    // GPS simulation variables
    private Vector3 gpsPosition;
    private float gpsUpdateInterval = 1f; // Update GPS every 1 second
    private float lastGpsUpdateTime = 0f;

    // Scene camera
    public Camera sceneCamera;
    private float sceneCameraPublishInterval = 1f / 15f; // 15 FPS for scene camera
    private float lastSceneCameraPublishTime = 0f;
    private RenderTexture sceneRenderTexture;
    private Texture2D sceneTexture;

    // Add reference to kartLap script
    private kartLap kartLapScript;

    private SQLiteConnection _connection;
    private float checkpointDataPublishInterval = 1f; // Publish checkpoint data every 1 second
    private float lastCheckpointDataPublishTime = 0f;

    void Start()
    {
        wss = new WebSocketServer(4567);
        wss.AddWebSocketService<PubSubService>("/pubsub");
        wss.Start();
        Debug.Log("WebSocket server started on ws://localhost:4567/pubsub");

        // Initialize car components
        cameraSens = GetComponent<CameraSensor>();
        sensorsClass = GameObject.Find("Car Urban Tesla/SensorBox").GetComponent<Sensors>();

        // Initialize GPS position
        gpsPosition = transform.position;

        // Initialize scene camera
        if (sceneCamera == null)
        {
            sceneCamera = Camera.main;
        }
        
        if (sceneCamera != null)
        {
            sceneRenderTexture = new RenderTexture(512, 512, 8, RenderTextureFormat.ARGB4444);
            sceneCamera.targetTexture = sceneRenderTexture;
            sceneTexture = new Texture2D(512, 512, TextureFormat.RGB24, false);
        }
        else
        {
            Debug.LogError("Scene camera is not assigned and main camera not found!");
        }

        // Get reference to kartLap script
        kartLapScript = GetComponent<kartLap>();
        if (kartLapScript == null)
        {
            Debug.LogError("kartLap script not found on this GameObject!");
        }

        InitializeDatabase();
    }

    private float publishInterval = 0.05f; // Publish every 0.05 seconds (20 Hz)
    private float lastPublishTime = 0f;
    private float cameraPublishInterval = 1f / 30f; // 30 FPS
    private float lastCameraPublishTime = 0f;

    void Update()
    {
        if (Time.time - lastPublishTime >= publishInterval)
        {
            PublishSpeed();
            PublishSensorData();
            PublishLapAndCheckpointData();
            lastPublishTime = Time.time;
        }

        if (Time.time - lastCameraPublishTime >= cameraPublishInterval)
        {
            PublishCameraImage();
            lastCameraPublishTime = Time.time;
        }

        if (Time.time - lastGpsUpdateTime >= gpsUpdateInterval)
        {
            UpdateAndPublishGPS();
           
            lastGpsUpdateTime = Time.time;
        }

        if (Time.time - lastSceneCameraPublishTime >= sceneCameraPublishInterval)
        {
            PublishSceneCameraImage();
            lastSceneCameraPublishTime = Time.time;
        }

        if (Time.time - lastCheckpointDataPublishTime >= checkpointDataPublishInterval)
        {
            PublishCheckpointData();
            lastCheckpointDataPublishTime = Time.time;
        }
    }

    void OnDestroy()
    {
        wss.Stop();
        Debug.Log("WebSocket server stopped");

        if (sceneRenderTexture != null)
        {
            sceneRenderTexture.Release();
            Destroy(sceneRenderTexture);
        }

        if (sceneTexture != null)
        {
            Destroy(sceneTexture);
        }

        if (_connection != null)
        {
            _connection.Close();
        }
    }

    public class PubSubService : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            try
            {
                var message = e.Data.Split(':');
                if (message.Length < 2) return;

                var action = message[0];
                var topic = message[1];

                switch (action)
                {
                    case "subscribe":
                        Subscribe(topic);
                        break;
                    case "unsubscribe":
                        Unsubscribe(topic);
                        break;
                    case "publish":
                        if (message.Length < 3) return;
                        var data = message[2];
                        Publish(topic, data);
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error processing message: {ex.Message}");
            }
        }

        private void Subscribe(string topic)
        {
            if (!SimpleWebSocketServer.Instance.subscribers.ContainsKey(topic))
            {
                SimpleWebSocketServer.Instance.subscribers[topic] = new List<string>();
            }
            if (!SimpleWebSocketServer.Instance.subscribers[topic].Contains(ID))
            {
                SimpleWebSocketServer.Instance.subscribers[topic].Add(ID);
                Debug.Log($"Client {ID} subscribed to {topic}");
            }
        }

        private void Unsubscribe(string topic)
        {
            if (SimpleWebSocketServer.Instance.subscribers.ContainsKey(topic))
            {
                SimpleWebSocketServer.Instance.subscribers[topic].Remove(ID);
                Debug.Log($"Client {ID} unsubscribed from {topic}");
            }
        }

        public static void Publish(string topic, string data)
        {
            if (SimpleWebSocketServer.Instance.subscribers.ContainsKey(topic))
            {
                var service = SimpleWebSocketServer.Instance.wss.WebSocketServices["/pubsub"];
                foreach (var clientId in SimpleWebSocketServer.Instance.subscribers[topic])
                {
                    try
                    {
                        service.Sessions.SendTo($"{topic}:{data}", clientId);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"Failed to send message to client {clientId}: {ex.Message}");
                    }
                }
                // Debug.Log($"Published to {topic}: {data}");
            }

            // Handle incoming car control data
            if (topic == "/car/control")
            {
                SimpleWebSocketServer.Instance.HandleCarControlData(data);
            }
        }
    }

    // Singleton instance
    public static SimpleWebSocketServer Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        sceneTexture = new Texture2D(512, 512, TextureFormat.RGB24, false);
        sceneRenderTexture = new RenderTexture(512, 512, 8, RenderTextureFormat.ARGB4444);
    }

    private void HandleCarControlData(string data)
    {
        string[] responseArray = ParseResponse(data);
        if (responseArray.Length >= 6)
        {
            float.TryParse(responseArray[0], out Speed);
            float.TryParse(responseArray[1], out Steering);
            float.TryParse(responseArray[2], out command);
            float.TryParse(responseArray[3], out sensor);
            float.TryParse(responseArray[4], out get_speed);
            float.TryParse(responseArray[5], out sensor_angle);

            sensorsClass.frontSensorAngle = sensor_angle;

            // Handle other car control logic here
        }
    }

    private void PublishSpeed()
    {
        string speedData = $"<speed>{current_speed}</speed>";
        PubSubService.Publish("/car/speed", speedData);
    }

    private void PublishSensorData()
    {
        float thisDistanceCenter = sensorsClass.distanceCenter;
        float thisDistanceRight = sensorsClass.distanceRight;
        float thisDistanceLeft = sensorsClass.distanceLeft;

        string sensorData = $"<sensor>L:{(int)(thisDistanceLeft * 100f)},M:{(int)(thisDistanceCenter * 100f)},R:{(int)(thisDistanceRight * 100f)}</sensor>";
        PubSubService.Publish("/car/sensors", sensorData);
    }

    public void PublishCameraImage()
    {
        string imageData = $"<image>{cameraSens.cameraImageString}</image>";
        PubSubService.Publish("/car/camera", imageData);
    }

    public void PublishSceneCameraImage()
    {
        if (sceneCamera == null || sceneRenderTexture == null || sceneTexture == null)
        {
            Debug.LogError("Scene camera or textures are not properly initialized!");
            return;
        }

        RenderTexture.active = sceneRenderTexture;
        sceneCamera.Render();
        sceneTexture.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
        sceneTexture.Apply();
        RenderTexture.active = null;

        byte[] bytes = sceneTexture.EncodeToJPG();
        string encodedImage = Convert.ToBase64String(bytes);

        string imageData = $"<sceneImage>{encodedImage}</sceneImage>";
        PubSubService.Publish("/car/sceneCamera", imageData);
    }

    private static string[] ParseResponse(string response)
    {
        if (response != "")
        {
            string[] arr = Regex.Matches(response, @"-?[0-9]\d*(.\d+)?")
                .OfType<Match>()
                .Select(m => m.Groups[0].Value)
                .ToArray();
            return arr;
        }
        else
        {
            return new string[] { "0", "0", "0", "0", "0", "0" };
        }
    }

    private void UpdateAndPublishGPS()
    {
        // Simulate GPS movement (you can adjust this based on your needs)
        gpsPosition = transform.position;
        // Convert to latitude and longitude (this is a simple approximation)
        // You might want to use a more accurate conversion based on your game world
        float latitude = gpsPosition.z / 111000f; // Rough approximation: 1 degree = 111 km
        float longitude = gpsPosition.x / (111000f * Mathf.Cos(latitude * Mathf.Deg2Rad));

        string gpsData = $"<gps>lat:{latitude:F6},lon:{longitude:F6}</gps>";
        PubSubService.Publish("/car/gps", gpsData);
        Debug.Log(gpsData );
    }

    private void PublishLapAndCheckpointData()
    {
        if (kartLapScript != null)
        {
            string raceStatus = true ? "started" : "not_started";
            string lapData = $"<lapData>status:{raceStatus},team:{kartLapScript.teamName},stage:{kartLapScript.stageName},laps:{kartLapScript.lapNumber},checkpoints:{kartLapScript.checkpointIndex}/{kartLapScript.totalCheckpoints}</lapData>";
            PubSubService.Publish("/car/lapData", lapData);
        }
    }

    void InitializeDatabase()
    {
        string DatabaseName = "Scores.db";
        string filepath = Application.persistentDataPath + "/" + DatabaseName;
        _connection = new SQLiteConnection(filepath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Database initialized at: " + filepath);
    }

    public class CheckpointDataItem
    {
        public string teamName { get; set; }
        public string stageName { get; set; }
        public int lap { get; set; }
        public int checkpoint { get; set; }
        public float time { get; set; }
    }

    void PublishCheckpointData()
    {
        try
        {
            Debug.Log("Starting PublishCheckpointData method");

            if (_connection == null)
            {
                Debug.LogError("Database connection is null. Attempting to reinitialize.");
                InitializeDatabase();
                if (_connection == null)
                {
                    Debug.LogError("Failed to reinitialize database connection.");
                    return;
                }
            }

            var checkpointData = new List<CheckpointDataItem>();
            
            var tableInfo = _connection.GetTableInfo("TeamScore");
            Debug.Log($"Table info count: {tableInfo.Count}");

            if (tableInfo.Count == 0)
            {
                Debug.LogWarning("TeamScore table does not exist in the database.");
            }
            else
            {
                var results = _connection.Query<TeamScore>("SELECT * FROM TeamScore ORDER BY TeamName, StageName, LapNumber, CheckpointIndex, Timestamp");
                
                var bestTimes = new Dictionary<(string, string, int, int), CheckpointDataItem>();

                foreach (var result in results)
                {
                    var key = (result.TeamName, result.StageName, result.LapNumber, result.CheckpointIndex);
                    if (!bestTimes.ContainsKey(key) || result.CheckpointTime < bestTimes[key].time)
                    {
                        bestTimes[key] = new CheckpointDataItem
                        {
                            teamName = result.TeamName,
                            stageName = result.StageName,
                            lap = result.LapNumber,
                            checkpoint = result.CheckpointIndex,
                            time = result.CheckpointTime
                        };
                    }
                }

                checkpointData = bestTimes.Values.OrderBy(c => c.teamName)
                                             .ThenBy(c => c.stageName)
                                             .ThenBy(c => c.lap)
                                             .ThenBy(c => c.checkpoint)
                                             .ToList();
            }

            Debug.Log($"Checkpoint data count: {checkpointData.Count}");

            if (checkpointData.Count == 0)
            {
                Debug.Log("No checkpoint data found in the database.");
            }

            string jsonData = JsonConvert.SerializeObject(checkpointData);
            Debug.Log("Checkpoint Data: " + jsonData);
            PubSubService.Publish("/car/checkpointData", jsonData);

            Debug.Log("Finished PublishCheckpointData method");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in PublishCheckpointData: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }
    }
}

// Make sure this class is defined somewhere in your project
public class TeamScore
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string TeamName { get; set; }
    public string StageName { get; set; }
    public int LapNumber { get; set; }
    public int CheckpointIndex { get; set; }
    public float CheckpointTime { get; set; }
    public DateTime Timestamp { get; set; }
}
