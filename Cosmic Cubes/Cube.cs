using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	public enum CubeMaterial {LIGHT,HEAVY,ICE,DUMMY};

	public CubeMaterial m_cubeMaterial;
	public float m_slideSpeed = 1f;
	public float m_rollSpeed = 15f;
	public GameObject m_outline;
	public AudioClip m_moveClip;
	public AudioClip m_rollClip;

	[HideInInspector] public bool m_isMoving;
	[HideInInspector] public Vector3 m_nearestMagnet;
	[HideInInspector] public bool m_isActive;
	[HideInInspector] public MeshRenderer m_outlineRenderer;

	private TileColor[] m_cubeSides;
	private bool m_roll;
	private Rigidbody m_controller;
	private Vector3 rotatedDir;
	private Transform m_camTransform;
	private AudioSource m_audio;


	void Awake(){
		m_audio = GetComponent<AudioSource> ();
		m_isMoving = false;
		m_roll = false;
		rotatedDir = Vector3.zero;
		m_isActive = false;
	}

	void Start(){

		m_cubeSides = GetComponentsInChildren<TileColor> ();
		m_controller = GetComponent<Rigidbody> ();

		m_camTransform = Camera.main.transform;
		m_outlineRenderer = transform.GetChild (0).GetComponent<MeshRenderer> ();
	}


	void OnEnable () {
		EventManager.StartListening ("Select", Select);
		EventManager.StartListening ("DeSelect", DeSelect);
	}

	void OnDisable () {
		EventManager.StopListening ("Select", Select);
		EventManager.StopListening ("DeSelect", DeSelect);
	}


	void FixedUpdate(){
		if (!m_isMoving) //jeśli FALSE, nic nie rób, tylko czekaj na input
			return;

		if (m_roll) {
			m_controller.AddForce (rotatedDir * m_rollSpeed);
			if (m_rollClip != null) {
				m_audio.clip = m_rollClip;
				m_audio.Play ();
			}
		} else{
			transform.position += rotatedDir * m_slideSpeed * Time.deltaTime;
			if (m_moveClip != null) {
				m_audio.clip = m_moveClip;
				m_audio.Play ();
			}
		}
	}


	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Obstacle")) {
			switch (m_cubeMaterial) {
			case CubeMaterial.DUMMY:
			case CubeMaterial.HEAVY:
			case CubeMaterial.ICE:
				EventManager.TriggerEvent ("MagnetON");
				transform.position = m_nearestMagnet;
				Stop ();
				break;
			case CubeMaterial.LIGHT:
				m_roll = true;
				break;
			default:
				Debug.Log ("Obstacle ERROR");
				break;
			}
		}
	}


	void OnTriggerExit(Collider other){
		if(other.CompareTag("Obstacle")){
			Stop();
		}
	}


	void OnCollisionEnter(Collision collision){
		switch (collision.gameObject.tag) {
		case "Wall":
		case "Player":
			EventManager.TriggerEvent ("MouseUp");
			EventManager.TriggerEvent ("MagnetON");
			if (m_isMoving) {
				transform.position = m_nearestMagnet;
				Stop ();
			} 
//			else {
//				if (collision.gameObject.GetComponent<Cube> ().m_cubeMaterial == CubeMaterial.HEAVY) {
//					rotatedDir = collision.gameObject.GetComponent<Cube> ().rotatedDir;
//					if (m_cubeMaterial == CubeMaterial.ICE) {
//						EventManager.TriggerEvent ("MagnetOFF");
//					}
//					m_isMoving = true;
//				}
//			}
			break;
		default:
			break;
		}
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


	public GameObject GetTopSide(){
		GameObject obj = m_cubeSides [0].gameObject;
		foreach (TileColor side in m_cubeSides) {
			if (side.gameObject.transform.position.y > obj.transform.position.y) {
				obj = side.gameObject;
			}
		}
		return obj;
	}


	public void StartMove(Vector3 dir){
		//Rotate direction vector with camera
		rotatedDir = m_camTransform.TransformDirection (dir);
		rotatedDir = new Vector3 (Mathf.Round(rotatedDir.x), 0, Mathf.Round(rotatedDir.z));
		rotatedDir = rotatedDir.normalized * dir.magnitude;

		if (m_cubeMaterial == CubeMaterial.ICE) {
			EventManager.TriggerEvent ("MagnetOFF");
		}
		m_isMoving = true;
	}


	public void Stop(){
		m_nearestMagnet = transform.position;
		m_roll = false;
		m_controller.velocity = Vector3.zero;
		m_isMoving = false;
		Vector3 rot = transform.rotation.eulerAngles;
		Vector3 targetRotation = new Vector3 (Mathf.Round (rot.x / 90) * 90, Mathf.Round (rot.y / 90) * 90, Mathf.Round (rot.z / 90) * 90);
		transform.rotation = Quaternion.Euler(targetRotation);

		EventManager.TriggerEvent ("Ready");
	}


	void OnMouseDown(){
		if(!m_outline.activeInHierarchy){
			EventManager.TriggerEvent ("DeSelect");
			Select ();
		}
	}


	public void Select(){
		m_isActive = true;
		m_outline.SetActive (true);

//		GameObject m_cam = Camera.main.gameObject;
//		m_cam.GetComponent<CameraMotor> ().m_lookAt = this.transform;
	}


	public void DeSelect(){
		m_isActive = false;
		m_outline.SetActive (false);
	}
}
