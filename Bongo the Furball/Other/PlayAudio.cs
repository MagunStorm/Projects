using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour {

	public AudioClip m_clip;
	public bool m_loop = false;

	void OnTriggerEnter2D(Collider2D other){
		if (!other.CompareTag ("Player"))
			return;

		SFXController.SetAudioLoop (m_loop);
		SFXController.SetAudio (m_clip);
	}
}
