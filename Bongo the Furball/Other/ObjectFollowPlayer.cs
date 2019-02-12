using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowPlayer : MonoBehaviour {

	public float m_SkySpeed=0.5f;
	public Transform m_camPos;

	private GameObject m_player;

	void Update () {
//		if (m_player != null) {
////			float newx = Mathf.Lerp (this.transform.position.x, m_player.transform.position.x, Time.deltaTime * m_SkySpeed);
//			float newx = m_player.transform.position.x;
//			this.transform.position = new Vector3 (newx, this.transform.position.y, this.transform.position.z);
//		} else {
//			m_player = GameObject.FindGameObjectWithTag ("Player");
//		}

		Vector3 pos = new Vector3 (m_camPos.position.x, this.transform.position.y, this.transform.position.z);
		this.transform.position = pos;

	}
}
