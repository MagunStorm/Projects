using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour {

	public static AudioController m_Instance;

	[HideInInspector] public float m_currentMusicVol;
	[HideInInspector] public float m_currentSfxVol;
	[HideInInspector] public bool m_musicOn;
	[HideInInspector] public bool m_sfxOn;
	[HideInInspector] public AudioSource m_audioSource;

	private AudioMixer m_audioMixer;


	void Awake(){
		if (m_Instance == null) {
			m_Instance = this;
		} else {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);

		m_audioSource = GetComponent<AudioSource> ();
	}


	void Start () {
		m_musicOn = true;
		m_sfxOn = true;
		LoadMixer ();
	}


	public void LoadMixer(){
		m_audioMixer = Resources.Load ("MainAudioMixer") as AudioMixer;
		m_audioMixer.GetFloat ("MusicVol", out m_currentMusicVol);
		m_audioMixer.GetFloat ("SfxVol", out m_currentSfxVol);
	}


	public void SetAudio(){
		if (m_musicOn) {
			m_audioMixer.SetFloat ("MusicVol", m_currentMusicVol);
		} else {
			m_audioMixer.SetFloat("MusicVol",-80f);
		}

		if (m_sfxOn) {
			m_audioMixer.SetFloat("SfxVol",m_currentSfxVol);
		} else {
			m_audioMixer.SetFloat ("SfxVol", -80f);
		}
	}


	public void SetMusicVol(float vol){
		m_audioMixer.SetFloat ("MusicVol", vol);
		m_currentMusicVol = vol;
	}


	public void SetSfxVol(float vol){
		m_audioMixer.SetFloat ("SfxVol", vol);
		m_currentSfxVol = vol;
	}


	public void ToggleMuteMusic(){
		if (m_musicOn) {
			m_musicOn = false;
			m_audioMixer.SetFloat("MusicVol",-80f);
		} else {
			m_musicOn = true;
			m_audioMixer.SetFloat("MusicVol",m_currentMusicVol);
		}
	}


	public void ToggleMuteSfx(){
		if (m_sfxOn) {
			m_sfxOn = false;
			m_audioMixer.SetFloat("SfxVol",-80f);
		} else {
			m_sfxOn = true;
			m_audioMixer.SetFloat("SfxVol",m_currentSfxVol);
		}
	}
}
