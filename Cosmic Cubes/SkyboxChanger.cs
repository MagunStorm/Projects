using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour {

	void Start () {
		if (GameManager.m_Instance.m_selectedWorldSkybox != null) {
			RenderSettings.skybox = GameManager.m_Instance.m_selectedWorldSkybox;
		} else {
			Debug.Log("Skybox ERROR");
		}
	}
	

}
