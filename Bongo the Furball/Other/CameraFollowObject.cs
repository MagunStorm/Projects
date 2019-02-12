using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;

public class CameraFollowObject : MonoBehaviour {

	[Header("BOUNDRY")]
	public float xMargin = 1f; // Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f; // Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f; // How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f; // How smoothly the camera catches up with it's target movement in the y axis.
	[HideInInspector] public Transform m_Player; // Reference to the player's transform.
	public float m_XcameraOffset = 0f;
	public float m_YcameraOffset = 0f;
	public BoxCollider2D m_CameraBoundry;
	public float m_verticalSensitivity = 0.5f;
	public float m_minZoom = 2f;
	public float m_maxZoom = 5f;

	private float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
	private float orthoZoomSpeed = 0.02f;        // The rate of change of the orthographic size in orthographic mode.
	private float m_currentZoom;
	private float m_offsetX = 0f;
	private Camera m_cam;
	private Vector3 m_borderOffset;
	private Vector3 m_CamLowerLeftLimit;
	private Vector3 m_CamUpperRightLimit;


	private void OnEnable()
	{
		// Setting up the reference.
		if (m_Player == null) {
			m_Player = GameObject.FindGameObjectWithTag ("Player").transform;
		}
		m_cam = GetComponent<Camera> ();
	}


	void Start(){
		if (PlayerPrefs.HasKey ("Zoom")) {
			m_cam.orthographicSize = PlayerPrefs.GetFloat ("Zoom");
			foreach (View v in GameController.m_Instance.views) {
				v.ViewSize = m_cam.orthographicSize;
			}
		} 
		SetCamBoundries ();
		MoveCamera ();
	}


	public void SetCamBoundries(){
		Vector3 minPoint = m_cam.ScreenToWorldPoint (Vector3.zero);
		m_borderOffset = transform.position - minPoint;

		float leftdownX = m_CameraBoundry.gameObject.transform.position.x - m_CameraBoundry.size.x / 2 + m_CameraBoundry.offset.x;
		float leftdownY = m_CameraBoundry.gameObject.transform.position.y - m_CameraBoundry.size.y / 2 + m_CameraBoundry.offset.y;
		float rightupX = m_CameraBoundry.gameObject.transform.position.x + m_CameraBoundry.size.x / 2 + m_CameraBoundry.offset.x;
		float rightupY = m_CameraBoundry.gameObject.transform.position.y + m_CameraBoundry.size.y / 2 + m_CameraBoundry.offset.y;

		m_CamLowerLeftLimit = new Vector3 (leftdownX, leftdownY, 0f);
		m_CamUpperRightLimit = new Vector3 (rightupX, rightupY, 0f);
	}


	private bool CheckXMargin()	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - m_Player.position.x) > xMargin;
	}


	private bool CheckYMargin()	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - m_Player.position.y) > yMargin;
	}


	private void CalculateOffsetX(){
		if (m_Player.GetComponent<PlayerController> ().lookingRight) {
			m_offsetX = m_XcameraOffset;
		} else {
			m_offsetX = -m_XcameraOffset;
		}
	}


	void Update(){
		float ver = Input.GetAxis ("Vertical");
//		if (HUDController.m_Instance.m_Vjoystick.m_InputDirection != Vector3.zero) {
//			ver = HUDController.m_Instance.m_Vjoystick.m_InputDirection.z;
//		}
			
		float targetY = transform.position.y;
		if (Mathf.Abs (ver) > m_verticalSensitivity && m_Player.GetComponent<PlayerController>().isGrounded) {
			targetY = Mathf.Lerp (transform.position.y, transform.position.y + (Mathf.Sign (ver) * m_YcameraOffset), ySmooth * Time.deltaTime);
		}
		transform.position = new Vector3(transform.position.x, targetY, transform.position.z);


		// If there are two touches on the device...
//		if (Input.touchCount == 2 && HUDController.m_Instance.m_UIElementPressedCount==0)
//		{
//			// Store both touches.
//			Touch touchZero = Input.GetTouch(0);
//			Touch touchOne = Input.GetTouch(1);
//
//			// Find the position in the previous frame of each touch.
//			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
//			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
//
//			// Find the magnitude of the vector (the distance) between the touches in each frame.
//			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
//			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
//
//			// Find the difference in the distances between each frame.
//			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
//
//			// If the camera is orthographic...
//			if (m_cam.orthographic){
//				// ... change the orthographic size based on the change in distance between the touches.
//				m_cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
//
//				// Make sure the orthographic size never drops below zero.
////				m_cam.orthographicSize = Mathf.Max(m_cam.orthographicSize, 0.1f);
//				m_cam.orthographicSize = Mathf.Clamp (m_cam.orthographicSize, m_minZoom, m_maxZoom);
//				PlayerPrefs.SetFloat ("Zoom", m_cam.orthographicSize);
//			}else{
//				// Otherwise change the field of view based on the change in distance between the touches.
//				m_cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
//
//				// Clamp the field of view to make sure it's between 0 and 180.
//				m_cam.fieldOfView = Mathf.Clamp(m_cam.fieldOfView, 0.1f, 179.9f);
//			}
//			SetCamBoundries ();
//			MoveCamera ();
//		}

	}


	private void LateUpdate(){
		if (!GameController.m_gameOver && m_Player!= null) {
			CalculateOffsetX ();
			MoveCamera ();
		}
	}


	public void MoveCamera(){
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		// If the player has moved beyond the x margin...
		if (CheckXMargin())		{
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, m_Player.position.x + m_offsetX, xSmooth*Time.deltaTime);
		}

		// If the player has moved beyond the y margin...
		if (CheckYMargin())		{
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp(transform.position.y, m_Player.position.y, ySmooth*Time.deltaTime);
		}
		targetX = Mathf.Clamp(targetX, m_CamLowerLeftLimit.x + m_borderOffset.x, m_CamUpperRightLimit.x - m_borderOffset.x);
		targetY = Mathf.Clamp(targetY, m_CamLowerLeftLimit.y + m_borderOffset.y, m_CamUpperRightLimit.y - m_borderOffset.y);

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}


}
