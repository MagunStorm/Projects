using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour {

	public float m_bgSize;
	public float m_viewZone = 10;

	private Transform m_camTransform;
	private Transform[] m_layers;
	private int m_leftIndex;
	private int m_rightIndex;

	void OnEnable(){
		m_camTransform = Camera.main.transform;
		m_layers = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			m_layers [i] = transform.GetChild (i);
		}
		m_leftIndex = 0;
		m_rightIndex = m_layers.Length - 1;
	}


	void Update(){
		if (m_camTransform.position.x < (m_layers [m_leftIndex].transform.position.x + m_viewZone)) {
			ScrollLeft ();
		}

		if (m_camTransform.position.x > (m_layers [m_rightIndex].transform.position.x - m_viewZone)) {
			ScrollRight ();
		}
	}


	void ScrollLeft(){
		m_layers [m_rightIndex].position = new Vector3(m_layers [m_leftIndex].position.x - m_bgSize, m_layers [m_leftIndex].position.y, m_layers [m_leftIndex].position.z );
		m_leftIndex = m_rightIndex;
		m_rightIndex--;
		if (m_rightIndex < 0) {
			m_rightIndex = m_layers.Length - 1;
		}
	}


	void ScrollRight(){
		m_layers [m_leftIndex].position = new Vector3(m_layers [m_rightIndex].position.x + m_bgSize, m_layers [m_rightIndex].position.y, m_layers [m_rightIndex].position.z );
		m_rightIndex = m_leftIndex;
		m_leftIndex++;
		if (m_leftIndex == m_layers.Length) {
			m_leftIndex = 0;
		}
	}

}
