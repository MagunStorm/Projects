using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour {

	public static AudioSource m_SFX;

	void Awake () {
		m_SFX = GetComponent<AudioSource> ();
	}


	public static void SetAudioLoop(bool loop){
		m_SFX.loop = loop;
	}


	public static void SetAudio(AudioClip clip){
		if (clip == null) {
			return;
		}
		m_SFX.clip = clip;
		m_SFX.Play ();
	}


	public static void StopAudio(){
		m_SFX.Stop ();
	}

}
