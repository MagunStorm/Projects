using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButtons : MonoBehaviour {

	public GameObject m_musicON;
	public GameObject m_musicOFF;
	public GameObject m_sfxON;
	public GameObject m_sfxOFF;

	void Start () {
		if (AudioController.m_Instance.m_currentMusicVol == -80f) {
			m_musicON.SetActive (false);
			m_musicOFF.SetActive (true);
		} else {
			m_musicON.SetActive (true);
			m_musicOFF.SetActive (false);
		}

		if (AudioController.m_Instance.m_currentSfxVol == -80f) {
			m_sfxON.SetActive (false);
			m_sfxOFF.SetActive (true);
		} else {
			m_sfxON.SetActive (true);
			m_sfxOFF.SetActive (false);
		}
	}

}
