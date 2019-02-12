using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

	[HideInInspector] public GameObject[] m_cubes;

	private float dragDistance;
	private GameObject m_activeCube;
	private Vector3 dir;
	private bool m_readyForNextMove;
	private float m_startTime = 0;
	private Vector2 m_touchPosition;
	private Vector3 fp;   //First touch position
	private Vector3 lp;   //Last touch position

	private bool m_mouseDown = false;


	void Start(){
		m_activeCube = null;
		m_readyForNextMove = true;
		m_startTime = Time.time;

		m_cubes = GameObject.FindGameObjectsWithTag ("Player");
		m_cubes[0].GetComponent<Cube>().Select();
		m_activeCube = GetActibeCube ();

		dragDistance = Screen.height * 10 / 100;
	}


	void Update(){
		if (Time.time - m_startTime < 2f) {
			return;
		}

		if (m_readyForNextMove && GetActibeCube() != null) {
			m_activeCube = GetActibeCube ();
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
				fp = Input.mousePosition;
				lp = Input.mousePosition;
				m_mouseDown = true;
			}

			if (m_mouseDown) {
				if (Mathf.Abs (lp.x - fp.x) > dragDistance || Mathf.Abs (lp.y - fp.y) > dragDistance) {
					Vector3 dir = new Vector3 (0, 0, 1);
					float xDrag = Mathf.Abs (lp.x - fp.x);
					float yDrag = Mathf.Abs (lp.y - fp.y);
					if (xDrag > yDrag) {
						dir = new Vector3 (Mathf.Sign ((lp - fp).x), 0, 0);
					} else {
						dir = new Vector3 (0, 0, Mathf.Sign ((lp - fp).y));
					}

					m_readyForNextMove = false;
					m_activeCube.GetComponent<Cube> ().StartMove (dir);
				} else { 
					//					Debug.Log ("Tap");
				}
			}
		}


		if (m_mouseDown) {
			lp = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1)) {
			MouseUp ();
		}

	}


	void OnEnable () {
		EventManager.StartListening ("Ready", Ready);
		EventManager.StartListening ("MouseUp", MouseUp);
	}

	void OnDisable () {
		EventManager.StopListening ("Ready", Ready);
	}


	public void DeselectAll(){
		EventManager.TriggerEvent ("DeSelect");
	}


	public void SelectCube(int index){
		m_activeCube = m_cubes[index];
		m_activeCube.GetComponent<Cube>().Select();
	}


	public GameObject GetActibeCube(){
		foreach (GameObject cube in m_cubes) {
			if (cube.GetComponent<Cube> ().m_isActive) {
				return cube;
			}
		}
		return null;
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
		m_readyForNextMove = false;
		m_activeCube.GetComponent<Cube> ().StartMove (dir);
	}


	void Ready(){
		m_readyForNextMove = true;
	}


	void MouseUp(){
		m_mouseDown = false;
	}

}
