using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Magnet : MonoBehaviour {

	public float m_Force;
	public Transform m_magnetPoint;

	[Header("Tile type")]
	public bool m_targetTile = false;
//	public bool m_bridgeTile = false;

	private GameObject m_cube;
	private TileColor m_tileColor;
	private bool m_scored;
	private bool m_isMagnetON;

	void Start(){
		m_tileColor = GetComponent<TileColor> ();
		m_scored = false;
		m_isMagnetON = true;
	}


	void Update(){
		if (m_cube != null) {
			m_cube.GetComponent<Cube> ().m_nearestMagnet = m_cube.transform.position;
			if (m_isMagnetON) {
				m_cube.transform.position = Vector3.MoveTowards (m_cube.transform.position, m_magnetPoint.position, m_Force * Time.deltaTime);
				if (Vector3.Distance(m_cube.transform.position,m_magnetPoint.position)<0.05f) {
					EventManager.TriggerEvent ("UpdateMovesCount");
					GameObject topSide = m_cube.GetComponentInParent<Cube> ().GetTopSide ();
					m_cube.GetComponentInParent<Cube> ().m_outlineRenderer.enabled = false;
					if (m_targetTile) {
						if (m_tileColor.color == topSide.GetComponent<TileColor> ().color) {
							Color newcolor = topSide.GetComponent<TileColor> ().m_colorPick;
							newcolor.a = 100f;
							m_cube.GetComponentInParent<Cube> ().m_outlineRenderer.material.color = newcolor;
							m_cube.GetComponentInParent<Cube> ().m_outlineRenderer.enabled = true;
							EventManager.TriggerEvent ("Score");
							m_scored = true;
						}
					} 

//					if (m_holeTile) {
//						GetComponent<BoxCollider> ().isTrigger = true;
//						GetComponent<Magnet> ().enabled = false;
//					}
					m_cube.GetComponent<Cube> ().Stop ();
					m_cube = null;
				}
			}
		}
	}


	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			m_cube = other.gameObject;
		}
	}


	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			m_cube = null;
			if (m_scored) {
				EventManager.TriggerEvent ("UnScore");
				m_scored = false;
			}
		}
	}


	void OnEnable () {
		EventManager.StartListening ("MagnetON", MagnetON);
		EventManager.StartListening ("MagnetOFF", MagnetOFF);
	}


	void OnDisable () {
		EventManager.StopListening ("MagnetON", MagnetON);
		EventManager.StopListening ("MagnetOFF", MagnetOFF);
	}
		

	void MagnetON(){
		m_isMagnetON = true;
	}


	void MagnetOFF(){
		m_isMagnetON = false;
	}
}
