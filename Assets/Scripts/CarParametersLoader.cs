using System.IO;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarParametersLoader : MonoBehaviour
{
    public CarController carController; // Reference to the Car Controller script on the vehicle

    private void Start()
    {
        LoadCarParameters();
    }

    private void LoadCarParameters()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CarParams.json");
        print(path);
        if (File.Exists(path))
        {
            string jsonContent = File.ReadAllText(path);
            CarPhysicsParameters parameters = JsonUtility.FromJson<CarPhysicsParameters>(jsonContent);
            parameters.ApplyToCarController(carController);
        }
        else
        {
            Debug.LogError("Cannot find the Car Physics Parameters JSON file.");
        }
    }
}