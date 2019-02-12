using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampRectTransform : MonoBehaviour {

	public float m_padding = 10f;
	public float m_elementSize = 128f;
	public float m_viewSize = 250f;

	private RectTransform m_rt;
	private int m_amountEleemnts;
	private float m_contentSize;


	void Start(){
		m_rt = GetComponent<RectTransform> ();
	}


	void Update(){
		// Clamp rect transform
		m_amountEleemnts = m_rt.childCount;
		m_contentSize = (m_amountEleemnts * (m_elementSize + m_padding) - m_viewSize) * m_rt.localScale.x;
		if (m_rt.localPosition.x > m_padding) {
			m_rt.localPosition = new Vector3 (m_padding, m_rt.localPosition.y, m_rt.localPosition.z);
		} else if (m_rt.localPosition.x < -m_contentSize) {
			m_rt.localPosition = new Vector3 (-m_contentSize, m_rt.localPosition.y, m_rt.localPosition.z);
		}
	}

}
