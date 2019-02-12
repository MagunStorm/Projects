using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAudio : MonoBehaviour
{
	public AudioClip m_MusicClip;

    void Start(){
		if (m_MusicClip != null) {	
			// jeśli aktualny clip jest inny, niż podany, ustaw nowy i włącz
			if (AudioController.m_Instance.m_audioSource.clip != m_MusicClip) {
				AudioController.m_Instance.m_audioSource.clip = m_MusicClip;
				AudioController.m_Instance.m_audioSource.Play ();
			}
		} else {
			// jeśli nie podano clipu
			if (AudioController.m_Instance.m_audioSource.clip != GameManager.m_Instance.m_selectedWorldClip && GameManager.m_Instance.m_selectedWorldClip!= null) {
				// jeśli aktualny clip jest inny, niż zapisany w GameManager, ustaw nowy i włącz
				AudioController.m_Instance.m_audioSource.clip = GameManager.m_Instance.m_selectedWorldClip;
				AudioController.m_Instance.m_audioSource.Play ();
			}
		}
    }
		
}
