using UnityEngine;
using UnityEditor;
using System.Collections;
using EasyRoads3Dv3;

public class SnapRoadToTerrain : ScriptableObject {

	[MenuItem( "EasyRoads3D/Snap Roads To Terrain" )]
	public static void  BuildNetwork () 
	{
		ERModularRoad[] scrpts = FindObjectsOfType(typeof(ERModularRoad)) as ERModularRoad[];
		ERModularBase baseScript = FindObjectOfType(typeof(ERModularBase)) as ERModularBase;

		foreach(ERModularRoad scr in scrpts){
			Mesh m = scr.gameObject.GetComponent<MeshFilter>().sharedMesh;

			Vector3[] vecs = m.vertices;

			for(int i = 0; i < vecs.Length; i++){
			//	Vector3 vt = v;
//-->				baseScript.OOQCDCDCDC(ref vecs[i]);

			}
			m.vertices = vecs;

			scr.gameObject.GetComponent<MeshCollider>().sharedMesh = null;
			scr.gameObject.GetComponent<MeshCollider>().sharedMesh = m;
		}

		Debug.Log("done");
	}

}
