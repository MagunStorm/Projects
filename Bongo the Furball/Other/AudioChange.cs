using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChange : MonoBehaviour {

	public AudioSource m_audio;
	public AudioClip m_clip;
	[Range(0f,1f)] public float m_volume=0.5f;

	void OnTriggerEnter2D(Collider2D collider){
		if (!collider.CompareTag ("Player")) {
			return;
		}

		m_audio.Stop ();
		m_audio.clip = m_clip;
		m_audio.volume = m_volume;
		m_audio.Play ();
	}

}
