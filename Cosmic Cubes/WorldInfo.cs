using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldInfo : MonoBehaviour {

	public string m_worldPrefix;
	public Material m_worldSkybox;
	public AudioClip m_worldClip;
	public int m_minStarRequired = 0;
	public GameObject m_padlock;

	private Button m_button;


	void Start(){
		m_button = GetComponent<Button> ();
		if (GameManager.m_Instance.m_stars < m_minStarRequired) {
			m_button.interactable = false;
			m_padlock.SetActive (true);
			m_padlock.GetComponentInChildren<Text> ().text = m_minStarRequired.ToString ();
		}else{
			m_button.interactable = true;
			m_padlock.SetActive (false);
		}
	}


	public void SetWorldInfo(){
		GameManager.m_Instance.m_selectedWorldPrefix = m_worldPrefix;
		GameManager.m_Instance.m_selectedWorldSkybox = m_worldSkybox;
		GameManager.m_Instance.m_selectedWorldClip = m_worldClip;
	}

}
