using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRepeatBG : MonoBehaviour {

	public float m_tileSize;
	public Transform m_RepeatPoint;
	public float m_scrollSpeed;

	private Transform[] m_layers;
	private int m_leftIndex;
	private int m_rightIndex;

	void OnEnable(){
		m_layers = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			m_layers [i] = transform.GetChild (i);
			m_layers [i].gameObject.GetComponent<AutoScroll> ().m_scrollSpeed = m_scrollSpeed;
			m_layers [i].gameObject.GetComponent<AutoScroll> ().enabled = true;
		}
		m_leftIndex = 0;
		m_rightIndex = m_layers.Length - 1;
	}


	void Update(){
		if (m_scrollSpeed<0 && m_layers [m_leftIndex].transform.position.x <= m_RepeatPoint.position.x) {
			ScrollRight ();
		}
			
		if (m_scrollSpeed>0 && m_layers [m_rightIndex].transform.position.x > m_RepeatPoint.position.x) {
			ScrollLeft ();
		}
	}


	void ScrollLeft(){
		m_layers [m_rightIndex].position = new Vector3(m_layers [m_leftIndex].position.x - m_tileSize, m_layers [m_leftIndex].position.y, m_layers [m_leftIndex].position.z );
		m_leftIndex = m_rightIndex;
		m_rightIndex--;
		if (m_rightIndex < 0) {
			m_rightIndex = m_layers.Length - 1;
		}
	}


	void ScrollRight(){
		m_layers [m_leftIndex].position = new Vector3(m_layers [m_rightIndex].position.x + m_tileSize, m_layers [m_rightIndex].position.y, m_layers [m_rightIndex].position.z );
		m_rightIndex = m_leftIndex;
		m_leftIndex++;
		if (m_leftIndex == m_layers.Length) {
			m_leftIndex = 0;
		}
	}

}
