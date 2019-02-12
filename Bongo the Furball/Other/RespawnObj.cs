using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObj : MonoBehaviour {

	public float m_RespawnDelay;

	private float m_nextActionTime;
	private bool m_respawn;
	private GameObject m_RespawnObj;

	void Start () {
		m_nextActionTime = 0f;
		m_respawn = false;
	}
	

	void Update () {
		if (m_respawn && Time.time > m_nextActionTime) {
			m_RespawnObj.SetActive (true);
			m_respawn = false;
		}
	}


	public void SetRespawnObj(GameObject obj){
		m_nextActionTime = Time.time + m_RespawnDelay;
		m_respawn = true;
		m_RespawnObj = obj;
	}


}
