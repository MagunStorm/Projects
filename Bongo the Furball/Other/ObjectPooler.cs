using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

	public GameObject m_poolObjectPrefab;
	public int m_poolAmt=20;
	public bool m_willGrow = true;

	private List<GameObject> m_poolObjects;
	private Transform m_CommonPool;


	void Awake(){
		GameObject obj = GameObject.FindGameObjectWithTag ("CommonPool");
		m_CommonPool = obj != null ? obj.transform : null;

		InitiatePool ();
	}


	void Start () {
	}


	void InitiatePool(){
		m_poolObjects = new List<GameObject> ();
		for (int i = 0; i < m_poolAmt; i++) {
			GameObject obj = Instantiate (m_poolObjectPrefab);
			//			obj.transform.parent = this.transform;
			obj.transform.parent = m_CommonPool;
			obj.SetActive (false);
			m_poolObjects.Add (obj);
		}
	}


	public GameObject GetPooledObject(){
		//znajdź nieaktywny obiekt na liście
		for (int i = 0; i < m_poolObjects.Count; i++) {
			if (!m_poolObjects [i].activeInHierarchy) {
				return m_poolObjects [i];
			}
		}

		//..jeśli nie znaleziono wolnego obiektu sprawdź, czy lista może rosnąć
		if (m_willGrow) {
			//...i jeśli tak, dodaj nowy obiekt do listy i zwróć
			GameObject obj = Instantiate (m_poolObjectPrefab);
			m_poolObjects.Add (obj);
			return obj;
		}

		//jeśli nie ma wolnego i lista nie może rosnąć - zwróć null
		return null;
	}


	public void SpawnAtPoint(Transform point, float force = -100f){
		GameObject obj = GetPooledObject ();

		obj.transform.position = point.position;
		obj.transform.rotation = point.rotation;
//		obj.transform.parent = null;
		if (force != -100f) {
			obj.GetComponent<Projectile> ().m_speed = force;
		}
		obj.SetActive (true);
	}

}
