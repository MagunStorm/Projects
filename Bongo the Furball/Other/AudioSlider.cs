using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour {

	public enum AudioType {MUSIC,SFX};
	public AudioType m_audioType;

	private Slider m_slider;


	void Awake(){
		m_slider = GetComponent<Slider> ();
	}


	void OnEnable(){
		switch (m_audioType) {
		case AudioType.MUSIC:
			m_slider.value = AudioController.m_Instance.m_currentMusicVol;
			break;
		case AudioType.SFX:
			m_slider.value = AudioController.m_Instance.m_currentSfxVol;
			break;
		default:
			Debug.Log ("Audio ERROR");
			break;
		}
	}


	public void UpdateAudio(){
		switch (m_audioType) {
		case AudioType.MUSIC:
			AudioController.m_Instance.SetMusicVol (m_slider.value);
//			Debug.Log ("MUSIC Update");
			break;
		case AudioType.SFX:
			AudioController.m_Instance.SetSfxVol (m_slider.value);
//			Debug.Log ("SFX Update");
			break;
		default:
			Debug.Log ("Audio Update ERROR");
			break;
		}
	}
}
