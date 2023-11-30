using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;
using UnityEngine;
using System.IO;
using System;

public class CameraSensor : MonoBehaviour
{

	public Camera sensorCam;
	public int width = 512;
	public int height = 512;
	
	public int counter = 0;
	Texture2D img;
	Texture2D tex;
	private CarServer m_server;
	private Config config;
	RenderTexture ren;

	[HideInInspector]
	[System.NonSerialized]
	public byte[] cameraImage;
	[HideInInspector]
	[System.NonSerialized]
	public string cameraImageString;

	void Awake()
	{
		tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		ren = new RenderTexture(width, height, 8, RenderTextureFormat.ARGB32);
		sensorCam.targetTexture = ren;
	}

	Texture2D RTImage(Camera cam)
	{
		RenderTexture currentRT = RenderTexture.active;
		RenderTexture.active = cam.targetTexture;
		cam.Render();
		tex.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
		tex.Apply();
		RenderTexture.active = currentRT;
		return tex;
	}

	public Texture2D GetImage()
	{
		return RTImage(sensorCam);
	}

	void Start()
    {	
		string path = Application.dataPath + "config.json";
		if (!File.Exists(path))
        {
			StreamWriter sw = File.CreateText(path);
		}else{
			string jsonString;
			jsonString = File.ReadAllText(path);
			JSONNode data = JSON.Parse(jsonString); 
			
			// foreach(JSONNode con in data["Config"])
			// {
			// 	Debug.Log(con["CameraWidth"]);
			// 	Debug.Log(con["CameraHeight"]);
			// }
		}

	}
	void Update()
	{	
		cameraImageString = Texture2DToBase64(GetImage());
	}

	public static string Texture2DToBase64(Texture2D texture)
	{	
		byte[] imageData = texture.EncodeToJPG();
		return Convert.ToBase64String(imageData);
	}
	
	public string getFrame()
    {
		return Texture2DToBase64(GetImage());
	}
	
}

