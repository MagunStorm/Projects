using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {

	public string m_AnimState;
	public bool m_playSound;
	public float m_disableDelay = 0f;

	private Animator m_anim;
	private AudioSource m_audio;

	void Awake(){
		gameObject.SetActive (false);
		m_anim = GetComponent<Animator> ();
		m_audio = GetComponent<AudioSource> ();
	}
		
	void OnEnable(){
		if (m_AnimState != null) {
			m_anim.Play (m_AnimState);
		}
		if (m_playSound) {
			m_audio.Play ();
		}
//		Invoke ("DisableEffect", m_disableDelay);
		Destroy(gameObject,m_disableDelay);
	}


	void DisableEffect(){
		gameObject.SetActive (false);
	}

}
