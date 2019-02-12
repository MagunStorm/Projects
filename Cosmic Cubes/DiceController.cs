using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour {

	public float m_slideSpeed = 1f;
	public float m_rollSpeed = 15f;

	[HideInInspector] public bool m_isMoving;

	private Rigidbody m_controller;
//	private Transform m_camTransform;
	private Vector3 dir;
	private bool m_roll;
	private bool m_readyForNextMove;
	private TileColor[] m_cubeSides;
	private Vector3 m_currentPos;

	private Transform m_camTransform;
	private Vector3 rotatedDir;

	void Start(){
		m_currentPos = transform.position;
		m_readyForNextMove = true;
		m_isMoving = false;
		m_roll = false;

		m_controller = GetComponent<Rigidbody> ();
		m_cubeSides = GetComponentsInChildren<TileColor> ();

		m_camTransform = Camera.main.transform;
	}


	void Update(){
		if (m_readyForNextMove) {
			dir = Vector3.zero;
			dir.x = Input.GetAxis ("Horizontal");
			dir.z = Input.GetAxis ("Vertical");
			dir.Normalize ();
			if (dir != Vector3.zero) {
				StartMove ();
			}
		}
			
	}


	void FixedUpdate(){
		if (!m_isMoving) //jeśli FALSE, nic nie rób, tylko czekaj na input
			return;
		
		if (m_roll) {
			m_controller.AddForce (rotatedDir * m_rollSpeed);
		} else{
			transform.position += rotatedDir * m_slideSpeed * Time.deltaTime;
		}
	}


	void OnEnable () {
		EventManager.StartListening ("StopMoving", Stop);
	}

	void OnDisable () {
		EventManager.StopListening ("StopMoving", Stop);
	}



	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Obstacle")){
			m_roll = true;
		}
	}


	void OnTriggerExit(Collider other){
		if(other.CompareTag("Obstacle")){
			EventManager.TriggerEvent ("StopMoving");
		}
	}


	void OnCollisionEnter(Collision collision){
		switch (collision.gameObject.tag) {
		case "Wall":
		case "HeavyCube":
			transform.position = m_currentPos;
			EventManager.TriggerEvent ("StopMoving");
			break;
		default:
			break;
		}

//		if (collision.gameObject.CompareTag ("Wall")) {
//			transform.position = m_currentPos;
//			EventManager.TriggerEvent ("StopMoving");
//		}
	}


	public void SetDir(string d){
		switch(d){
		case "001":
			dir = new Vector3 (0, 0, 1);
			break;
		case "00-1":
			dir = new Vector3 (0, 0, -1);
			break;
		case "100":
			dir = new Vector3 (1, 0, 0);
			break;
		case "-100":
			dir = new Vector3 (-1, 0, 0);
			break;
		default:
			dir = new Vector3 (0, 0, 1);
			break;
		}
	}


	void StartMove(){
		m_isMoving = true;
		m_readyForNextMove = false;

		//Rotate direction vector with camera
		rotatedDir = m_camTransform.TransformDirection (dir);
		rotatedDir = new Vector3 (Mathf.Round(rotatedDir.x), 0, Mathf.Round(rotatedDir.z));
		rotatedDir = rotatedDir.normalized * dir.magnitude;
	}


	void Stop(){
		m_currentPos = transform.position;
		m_roll = false;
		m_controller.velocity = Vector3.zero;
		m_readyForNextMove = true;
		m_isMoving = false;
		Vector3 rot = transform.rotation.eulerAngles;
		Vector3 targetRotation = new Vector3 (Mathf.Round (rot.x / 90) * 90, Mathf.Round (rot.y / 90) * 90, Mathf.Round (rot.z / 90) * 90);
		transform.rotation = Quaternion.Euler(targetRotation);
	}


	public TileColor.Col CheckCubeSideUp(){
		GameObject obj = m_cubeSides [0].gameObject;
		foreach (TileColor side in m_cubeSides) {
			if (side.gameObject.transform.position.y > obj.transform.position.y) {
				obj = side.gameObject;
			}
		}
		return obj.GetComponent<TileColor> ().color;
	}

}
