using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class ObjectPlacementTool : MonoBehaviour
{
    private Vector3 _rotation;
    private bool _isInPlacementMode;
    private Camera _gameCamera;
    public Camera mainCamera;
    public GameObject leftSign;
    public GameObject rightSign;
    public GameObject stopSign;
    public GameObject straightSign;
    public GameObject objectToPlace;
    private string[] rotationStrings = new string[3];
    public class QuaternionConverter : JsonConverter<Quaternion>
    {
        public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
        {
            var jsonObject = new JObject();
            jsonObject.Add("x", value.x);
            jsonObject.Add("y", value.y);
            jsonObject.Add("z", value.z);
            jsonObject.Add("w", value.w);

            jsonObject.WriteTo(writer);
        }

        public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            float x = (float)jsonObject["x"];
            float y = (float)jsonObject["y"];
            float z = (float)jsonObject["z"];
            float w = (float)jsonObject["w"];

            return new Quaternion(x, y, z, w);
        }
    
     
    }
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            var jsonObject = new JObject();
            jsonObject.Add("x", value.x);
            jsonObject.Add("y", value.y);
            jsonObject.Add("z", value.z);
            jsonObject.Add("magnitude", value.magnitude); // Or any other property you want to include

            jsonObject.WriteTo(writer);
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            float x = (float)jsonObject["x"];
            float y = (float)jsonObject["y"];
            float z = (float)jsonObject["z"];

            return new Vector3(x, y, z);
        }
    }

    // [MenuItem("Window/Object Placement Tool")]
    // private static void Init()
    // {
    //     ObjectPlacementTool window = GetWindow<ObjectPlacementTool>();
    //     window.Show();
    // }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        
        // Display a label
        GUILayout.Label("Rotation", GUILayout.Width(50));
        
       rotationStrings[0] = GUILayout.TextField(rotationStrings[0], GUILayout.Width(50));
        rotationStrings[1] = GUILayout.TextField(rotationStrings[1], GUILayout.Width(50));
        rotationStrings[2] = GUILayout.TextField(rotationStrings[2], GUILayout.Width(50));

        GUILayout.EndHorizontal();

        // Use float.TryParse to only accept valid float values
        float newValue;
        if (float.TryParse(rotationStrings[0], out newValue)) {
            _rotation.x = newValue;
        }
        if (float.TryParse(rotationStrings[1], out newValue)) {
            _rotation.y = newValue;
        }
        if (float.TryParse(rotationStrings[2], out newValue)) {
            _rotation.z = newValue;
        }

        if (GUILayout.Button("Switch To AVIS Engine Scene Builder"))
        {
            _isInPlacementMode = true;
            mainCamera.gameObject.SetActive(false);
            _gameCamera = GameObject.Find("GizmoCamera").GetComponentInChildren<Camera>();

            SceneView.lastActiveSceneView.in2DMode = false;
            SceneView.lastActiveSceneView.rotation = Quaternion.Euler(0, 180, 0);
            SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.Euler(0, 180, 0), 1);
        }

        if (_isInPlacementMode)
        {
            if (GUILayout.Button("Add Left Sign"))
            {
                RaycastHit hit;
                Ray ray = _gameCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject newObject = Instantiate(leftSign, hit.point, Quaternion.identity);
                    newObject.tag = "AVISEngine";
                    newObject.layer = LayerMask.NameToLayer("AVISEngine");

                    newObject.transform.Rotate(_rotation);
                    Selection.activeGameObject = newObject;
                }
            }
            if (GUILayout.Button("Add Right Sign"))
            {
                RaycastHit hit;
                Ray ray = _gameCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject newObject = Instantiate(rightSign, hit.point, Quaternion.identity);
                    newObject.tag = "AVISEngine";
                    newObject.layer = LayerMask.NameToLayer("AVISEngine");

                    newObject.transform.Rotate(_rotation);
                    Selection.activeGameObject = newObject;
                }
            }
            if (GUILayout.Button("Add Straight Sign"))
            {
                RaycastHit hit;
                Ray ray = _gameCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject newObject = Instantiate(straightSign, hit.point, Quaternion.identity);
                    newObject.tag = "AVISEngine";
                    newObject.layer = LayerMask.NameToLayer("AVISEngine");

                    newObject.transform.Rotate(_rotation);
                    Selection.activeGameObject = newObject;
                }
            }
            if (GUILayout.Button("Add Stop Sign"))
            {
                RaycastHit hit;
                Ray ray = _gameCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject newObject = Instantiate(stopSign, hit.point, Quaternion.identity);
                    newObject.tag = "AVISEngine";
                    newObject.layer = LayerMask.NameToLayer("AVISEngine");

                    newObject.transform.Rotate(_rotation);
                    Selection.activeGameObject = newObject;
                }
            }
            if (GUILayout.Button("Add Obstacle"))
            {
                RaycastHit hit;
                Ray ray = _gameCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject newObject = Instantiate(objectToPlace, hit.point, Quaternion.identity);
                    newObject.tag = "AVISEngine";
                    newObject.layer = LayerMask.NameToLayer("AVISEngine");

                    newObject.transform.Rotate(_rotation);
                    Selection.activeGameObject = newObject;
                }
            }

            if (GUILayout.Button("Save Transform"))
            {
                string path = Path.Combine(Application.persistentDataPath, "transform.json");

                GameObject[] objects = GameObject.FindGameObjectsWithTag("AVISEngine");

                TransformData[] transforms = new TransformData[objects.Length];

                for (int i = 0; i < objects.Length; i++)
                {
                    string originalName = objects[i].name;

                    // Checking if the name contains "(Clone)" and removing it
                    string instantiatedName = originalName.Contains("(Clone)") ? originalName.Replace("(Clone)", "") : originalName;
                    
                    transforms[i] = new TransformData(objects[i].transform.position, objects[i].transform.rotation, instantiatedName);
                    Debug.Log(objects[i].transform.position);
                }

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    Converters = { new Vector3Converter(), new QuaternionConverter() },
                    Formatting = Formatting.Indented
                };

                string json = JsonConvert.SerializeObject(transforms, settings);
                Debug.Log(json);

                File.WriteAllText(path, json);
                Debug.Log("Written");
            }

            if (GUILayout.Button("Load Transform"))
            {
                string path = Application.persistentDataPath + "/transform.json";

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);

                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        Converters = { new Vector3Converter(), new QuaternionConverter() }
                    };

                    TransformData[] transforms = JsonConvert.DeserializeObject<TransformData[]>(json, settings);

                    foreach (TransformData transformData in transforms)
                    {
                        // GameObject objectToPlacePrefab = Resources.Load<GameObject>(transformData.type);
                        GameObject objectToPlacePrefab = null;

                        // Determine which prefab to instantiate
                        switch (transformData.type)
                        {
                            case "Left":
                                objectToPlacePrefab = leftSign;
                                break;
                            case "Right":
                                objectToPlacePrefab = rightSign;
                                break;
                            case "Stop":
                                objectToPlacePrefab = stopSign;
                                break;
                            case "Straight":
                                objectToPlacePrefab = straightSign;
                                break;
                            case "Barrier":
                                objectToPlacePrefab = objectToPlace;
                                break;
                            default:
                                Debug.LogError("Prefab name not recognized: " + transformData.type);
                                break;
                        }
                        if(objectToPlacePrefab != null)
                        {
                            GameObject newObject = Instantiate(objectToPlacePrefab, transformData.position, transformData.rotation);
                            newObject.name = transformData.type; // Assuming you want to restore the original prefab name
                        }
                        else
                        {
                            Debug.LogError("Prefab not found for: " + transformData.type);
                        }
                    }
                }
                else
                {
                    Debug.LogError("No saved transform data found at path: " + path);
                }
            }

            if (GUILayout.Button("Exit Edit Mode"))
            {
                _isInPlacementMode = false;
                mainCamera.gameObject.SetActive(true);
                // _gameCamera.gameObject.SetActive(false);

            }
        }
    }
  
    [Serializable]
    private class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public string type;

        public TransformData(Vector3 position, Quaternion rotation, string type)
        {
            this.position = position;
            this.rotation = rotation;
            this.type = type;

        }
    }
    
    private class TransformList
    {
        public TransformData[] transforms;

        public TransformList(TransformData[] transforms)
        {
            this.transforms = transforms;
        }
    }
}
