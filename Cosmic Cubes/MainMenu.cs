using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

	public float m_CameraTransitionSpeed = 3.0f;

	private Transform m_cameraTransform;
	private Transform m_cameraDestTransform;


	void Start(){
		m_cameraTransform = Camera.main.transform;
	}


	void Update(){
		if (m_cameraDestTransform != null) {
			m_cameraTransform.rotation = Quaternion.Slerp (m_cameraTransform.rotation, m_cameraDestTransform.rotation, Time.deltaTime * m_CameraTransitionSpeed);
		}
	}


	public void lookAtMenu(Transform newTransform){
		m_cameraDestTransform = newTransform;
	}

}
