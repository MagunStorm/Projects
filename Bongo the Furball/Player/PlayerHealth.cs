using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	public AudioClip m_playerHurtClip;
	public AudioClip m_playerDeathClip;
	public GameObject m_DestroyPrefab;

	[HideInInspector] public bool m_demaged = false;

	private AudioSource m_playerAudio;
	private PlayerSetup m_playerSetup;
	private HUDController m_HUD;


	void Awake(){
		m_playerAudio = GetComponent<AudioSource> ();
		m_playerSetup = GetComponent<PlayerSetup> ();

		GameObject obj = GameObject.FindGameObjectWithTag ("HUD");
		m_HUD = obj.GetComponent<HUDController> ();
	}
	

	void Update () {
		if(GameController.m_gameOver){
			return;
		}

		if(m_demaged){
			m_HUD.m_DemagePanel.color = flashColour;
		}
		else{
			m_HUD.m_DemagePanel.color = Color.Lerp (m_HUD.m_DemagePanel.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		m_demaged = false;

	}


	public void ModifyHealth(int mod){
		if (mod < 0) {
			m_demaged = true;
			m_playerAudio.clip = m_playerHurtClip;
			m_playerAudio.Play ();
		}
		m_playerSetup.m_currentHealth = Mathf.Clamp (m_playerSetup.m_currentHealth + mod, 0, m_playerSetup.m_maxHealth);
		HUDController.m_Instance.m_playerHUD.UpdateHealth(m_playerSetup.m_maxHealth,m_playerSetup.m_currentHealth);
		if (m_playerSetup.m_currentHealth <= 0) {
			OnPlayerDeath();
		}
	}
		

	void OnPlayerDeath(){
		HUDController.m_Instance.m_playerHUD.gameObject.SetActive(false);
		SFXController.SetAudio(m_playerDeathClip);				//odtworzenie odgłosu śmierci

		GameObject obj = Instantiate(m_DestroyPrefab,this.transform.position,Quaternion.identity);
		obj.SetActive (true);
		Destroy (gameObject);

		GameController.m_gameOver = true;
	}

}
