using System.IO;
using UnityEditor;
using UnityEngine;

public class ObjectPlacementTool : MonoBehaviour
{
    private Vector3 _rotation;
    private bool _isInPlacementMode;
    private Camera _gameCamera;
    public GameObject objectToPlace;
    private string[] rotationStrings = new string[3];


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

        if (GUILayout.Button("Place Objects"))
        {
            _isInPlacementMode = true;
            _gameCamera = GameObject.Find("Camera").GetComponentInChildren<Camera>();
            // _gameCamera.gameObject.SetActive(false);
            SceneView.lastActiveSceneView.in2DMode = false;
            SceneView.lastActiveSceneView.rotation = Quaternion.Euler(0, 180, 0);
            SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.Euler(0, 180, 0), 1);
        }

        if (_isInPlacementMode)
        {
            if (GUILayout.Button("Add Object 1"))
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

            if (GUILayout.Button("Add Object 2"))
            {
                RaycastHit hit;
                Ray ray = _gameCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject newObject = Instantiate(Resources.Load<GameObject>("Object2"), hit.point, Quaternion.identity);
                    newObject.transform.Rotate(_rotation);
                    Selection.activeGameObject = newObject;
                }
            }

            if (GUILayout.Button("List Objects"))
            {
                foreach (GameObject obj in Selection.gameObjects)
                {
                    Debug.Log(obj.name);
                }
            }

            if (GUILayout.Button("Save Transform"))
            {
                string path = Application.persistentDataPath + "/transform.json";

                GameObject[] objects = GameObject.FindGameObjectsWithTag("AVISEngine");
                TransformData[] transforms = new TransformData[objects.Length];

                for (int i = 0; i < objects.Length; i++)
                {
                    transforms[i] = new TransformData(objects[i].transform.position, objects[i].transform.rotation);
                }

                string json = JsonUtility.ToJson(new TransformList(transforms));
                File.WriteAllText(path, json);
            }

            if (GUILayout.Button("Load Transform"))
            {
                string path = Application.persistentDataPath + "/transform.json";

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    TransformList transformList = JsonUtility.FromJson<TransformList>(json);

                    foreach (TransformData transformData in transformList.transforms)
                    {
                        GameObject newObject = Instantiate(objectToPlace, transformData.position, transformData.rotation);
                    }
                }
            }

            if (GUILayout.Button("Exit Placement Mode"))
            {
                _isInPlacementMode = false;
                _gameCamera.gameObject.SetActive(true);
            }
        }
    }

    private class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;

        public TransformData(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
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
