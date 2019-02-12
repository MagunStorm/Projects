using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {

	public Transform m_lookAt;
	public Vector3 m_offset;
	public float m_smooth = 7.5f;
	public float m_swipeResistance = 200.0f;
	public float m_defaultFieldOfView=35f;
	public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.1f;        // The rate of change of the orthographic size in orthographic mode.
	public float m_startDelay = 0.5f;
	public bool m_canMoveCamera = false;
	public float m_cameraMoveSpeed = 5f;

	private Vector3 m_desiredPos;
	private Vector2 m_touchPosition;
	private float m_startTime = 0;
	private Camera m_camera;
	private float m_currentFieldOfView;

	private Vector3 fp;   //First touch position
	private Vector3 lp;   //Last touch position
	private bool m_mouseDown=false;

	void Start(){
		m_startTime = Time.time;
//		transform.LookAt (m_lookAt.position);
		m_camera = Camera.main;
		m_currentFieldOfView = m_defaultFieldOfView;
	}


	void Update(){

		if (Time.time - m_startTime < m_startDelay) {
			return;
		}

		m_desiredPos = m_lookAt.position + m_offset;
		transform.position = Vector3.Lerp (transform.position, m_desiredPos, m_smooth * Time.deltaTime);
		m_camera.fieldOfView = Mathf.Lerp (m_camera.fieldOfView, m_currentFieldOfView, m_smooth * Time.deltaTime);
		transform.LookAt (m_lookAt.position);

		// If there are two touches on the device...
		if (Input.touchCount == 2)
		{
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			// If the m_camera is orthographic...
			if (m_camera.orthographic)
			{
				// ... change the orthographic size based on the change in distance between the touches.
				m_camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

				// Make sure the orthographic size never drops below zero.
				m_camera.orthographicSize = Mathf.Max(m_camera.orthographicSize, 0.1f);
			}
			else
			{
				// Otherwise change the field of view based on the change in distance between the touches.
				m_currentFieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

				// Clamp the field of view to make sure it's between 0 and 180.
				m_currentFieldOfView = Mathf.Clamp(m_currentFieldOfView,20f,50f);
			}
		}

		if (Input.GetKeyDown ("[")) {
			m_currentFieldOfView += 5;
			m_currentFieldOfView = Mathf.Clamp(m_currentFieldOfView,20f,50f);
		}
		if (Input.GetKeyDown ("]")) {
			m_currentFieldOfView -= 5;
			m_currentFieldOfView = Mathf.Clamp(m_currentFieldOfView,20f,50f);
		}


		if (m_canMoveCamera) {
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
				fp = Input.mousePosition;
				lp = Input.mousePosition;
				m_mouseDown = true;
			}

			if (m_mouseDown) {
				lp = Input.mousePosition;
				Vector3 dragPos = (lp - fp).normalized;
				dragPos = new Vector3 (dragPos.x, 0f, dragPos.y);
				m_lookAt.position = Vector3.Lerp (m_lookAt.position, m_lookAt.position + dragPos, Time.deltaTime * m_cameraMoveSpeed);
			}

			if (Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1)) {
				m_mouseDown = false;
			}
		}

	}


	public void Slidem_camera(bool left){
		if (left) {
			m_offset = Quaternion.Euler (0, 90, 0) * m_offset;
		} else {
			m_offset = Quaternion.Euler (0, -90, 0) * m_offset;
		}
	}
}

