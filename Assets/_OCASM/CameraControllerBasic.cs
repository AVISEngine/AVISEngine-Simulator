using UnityEngine;
using System.Collections;

namespace OCASM {

	public class CameraControllerBasic : MonoBehaviour {
		#region Variables
		[Header("Camera")]
		public Camera cam;
		[Header("Rotation")]
		public float rotationSpeed = 200.0f;
		[Range (0f,0.99f)]
		public float smoothingFactorRotation = 0.9f;
	
		[Header("Movement")]
		public float movementSpeed = 10.0f;
		[Range (0f,0.99f)]
		public float smoothingFactorMovement = 0.9f;
		[Range (0f, 1000f)]
		[Space(10)]
		public float speedBoost = 10.0f;
		private float speedBoostH = 1.0f;
	
		[Header("FOV")]
		[Range (0.1f, 179.0f)]
		public float initialFOV = 60.0f;
		[Tooltip ("Should be lower than the initial FOV.")]
		[Range (0.1f, 179.0f)]
		public float zoomInFOV = 10.0f;
		[Space(10)]
		public float changeSpeedFOV = 60.0f;
		[Range (0f, 0.99f)]
		public float smoothingFactorFOV = 0.9f;
	
		private float FOVCurrent;
		private float FOVTarget;
	
		private Vector3 rotationCurrent;
		private Vector3 rotationTarget;
	
		private Vector3 movementChangeCurrent;
		private Vector3 movementChangeTarget;
		#endregion
	
		// Use this for initialization
		void Start () {
			movementChangeCurrent = new Vector3 (0,0,0);
			movementChangeTarget = new Vector3 (0,0,0);
	
			rotationCurrent = transform.eulerAngles;
			rotationTarget = rotationCurrent;
	
			FOVCurrent = initialFOV;
			FOVTarget = FOVCurrent;
		}
	
		// Update is called once per frame
		void Update () {
	
			// Rotation
			rotationTarget.y = rotationTarget.y + ((Input.GetAxis ("Mouse X")) * rotationSpeed * Time.deltaTime);
			rotationTarget.x = rotationTarget.x + ((Input.GetAxis ("Mouse Y") * -1) * rotationSpeed * Time.deltaTime);
	
			rotationCurrent = Vector3.Lerp (rotationCurrent, rotationTarget, 1 - smoothingFactorRotation);
			transform.eulerAngles = rotationCurrent;
	
			// Movement
			speedBoostH = Input.GetKey (KeyCode.LeftShift) ? speedBoost : 1.0f;
	
			movementChangeTarget.x = Input.GetAxis ("Horizontal") * movementSpeed * speedBoostH * Time.deltaTime;
			movementChangeTarget.y = Input.GetAxis ("Vertical") * movementSpeed * speedBoostH * Time.deltaTime;
	
			movementChangeCurrent = Vector3.Lerp (movementChangeCurrent, movementChangeTarget, 1 - smoothingFactorMovement);
			transform.Translate (Vector3.right * movementChangeCurrent.x, Space.Self);
			transform.Translate (Vector3.forward * movementChangeCurrent.y, Space.Self);
	
			// FOV
			if (Input.GetKey (KeyCode.Mouse0)) {
				FOVTarget -= changeSpeedFOV * Time.deltaTime;
			}
			if (!Input.GetKey (KeyCode.Mouse0)) {
				FOVTarget += changeSpeedFOV * Time.deltaTime;
			}
	
			FOVTarget = Mathf.Clamp (FOVTarget, zoomInFOV, initialFOV);
			FOVCurrent = Mathf.Lerp (FOVCurrent, FOVTarget, 1 - smoothingFactorFOV);
	
			cam.fieldOfView = FOVCurrent;
		}
	}
}