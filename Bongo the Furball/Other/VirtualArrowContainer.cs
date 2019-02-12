using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualArrowContainer : MonoBehaviour {

	public Vector3 m_InputDirection { set; get; }

	[HideInInspector] public Vector3 m_InputX;
	[HideInInspector] public Vector3 m_InputY;

	void Start (){
		m_InputDirection = Vector3.zero;
		m_InputX = Vector3.zero;
		m_InputY = Vector3.zero;
	}


	public void UpdateInputDirection(){
//		Debug.Log ("m_InputX: " + m_InputX);
//		Debug.Log ("m_InputY: " + m_InputY);
		m_InputDirection = m_InputX + m_InputY;
		m_InputDirection = (m_InputDirection.magnitude > 1) ? m_InputDirection.normalized : m_InputDirection;
//		Debug.Log ("m_InputDirection: " + m_InputDirection);
	}
}
