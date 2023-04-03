
/*
 * EasyRoads3D Runtime API example
 * 
 * Important:
 * 
 * It has occured that the terrain was not restored leaving the road shape in the terrain 
 * after exiting Play Mode. This happened after putting focus on the Scene View 
 * window while in Play Mode! In another occasion this consistently happened until the 
 * Scene View window got focus! 
 * 
 * Please check the above using a test terrain and / or backup your terrains before using this code!
 * 
 * You can backup your terrain by simply duplicating the terrain object 
 * in the project folder
 * 
 * Also, check the OnDestroy() code, without this code the shape of the generated roads
 * will certainly remain in the terrain object after you exit Play Mode!
 * 
 * 
 * 
 * 
 * */


using UnityEngine;
using System.Collections;
using EasyRoads3Dv3;

public class runtimeScript : MonoBehaviour {


	public ERRoadNetwork roadNetwork;
	
	public ERRoad road;
	
	public GameObject go;
	public int currentElement = 0;
	public float distance = 0;
	public float speed = 5f;




	void Start () {
	
		Debug.Log("Please read the comments at the top before using the runtime API!");

		// Create Road Network object
		roadNetwork = new ERRoadNetwork();

		// Create road
	//	ERRoad road = roadNetwork.CreateRoad(string name);
	//	ERRoad road = roadNetwork.CreateRoad(string name, Vector3[] markers);
	//	ERRoad road = roadNetwork.CreateRoad(string name, ERRoadType roadType);
	//	ERRoad road = roadNetwork.CreateRoad(string name, ERRoadType roadType, Vector3[] markers);

        // get exisiting road types
    //  ERRoadType[] roadTypes = roadNetwork.GetRoadTypes();
    //  ERRoadType roadType = roadNetwork.GetRoadTypeByName(string name);

        // create a new road type
		ERRoadType roadType = new ERRoadType();
		roadType.roadWidth = 6;
		roadType.roadMaterial = Resources.Load("Materials/roads/single lane") as Material;

        // create a new road
		Vector3[] markers = new Vector3[4];
		markers[0] = new Vector3(200, 5, 200);
		markers[1] = new Vector3(250, 5, 200);
		markers[2] = new Vector3(250, 5, 250);
		markers[3] = new Vector3(300, 5, 250);

		road = roadNetwork.CreateRoad("road 1", roadType, markers);

		// Add Marker: ERRoad.AddMarker(Vector3);
		road.AddMarker(new Vector3(300, 5, 300));

		// Add Marker: ERRoad.InsertMarker(Vector3);
		road.InsertMarker(new Vector3(275, 5, 235));

		// Delete Marker: ERRoad.DeleteMarker(int index);
		road.DeleteMarker(2);

		// Set the road width : ERRoad.SetWidth(float width);
	//	road.SetWidth(10);

		// Set the road material : ERRoad.SetMaterial(Material path);
	//	Material mat = Resources.Load("Materials/roads/single lane") as Material;
	//	road.SetMaterial(mat);

        // find a road
        //  public static function ERRoadNetwork.GetRoadByName(string name) : ERRoad;
        
        // all roads
        //  public static function ERRoadNetwork.GetRoads() : ERRoad[];  

		// Build Road Network 
		roadNetwork.BuildRoadNetwork();

		// Restore Road Network 
	//	roadNetwork.RestoreRoadNetwork();


		// create dummy object
		go = GameObject.CreatePrimitive(PrimitiveType.Cube);

	}
	
	void Update () {
	
		if(roadNetwork != null){
			float deltaT = Time.deltaTime;
			float rSpeed = (deltaT * speed);
		
			distance += rSpeed;

			// pass the current distance to get the position on the road
			Vector3 v = road.GetPosition(distance, ref currentElement);
			v.y += 1;
		
			go.transform.position = v;
		}

        // spline point info center of the road
        //      public function ERRoad.GetSplinePointsCenter() : Vector3[];

        // spline point info center of the road
        //      public function ERRoad.GetSplinePointsRightSide() : Vector3[];

        // spline point info center of the road
        //      public function ERRoad.GetSplinePointsLeftSide() : Vector3[];

        // Get the selected road in the Unity Editor
        //  public static function EREditor.GetSelectedRoad() : ERRoad;   



	}
	
	void OnDestroy(){

		// Restore road networks that are in Build Mode
		// This is very important otherwise the shape of roads will still be visible inside the terrain!

		if(roadNetwork != null){
			if(roadNetwork.isInBuildMode){
				roadNetwork.RestoreRoadNetwork();
			}
		}
	}
}
