using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

	[HideInInspector] public bool m_isDead;
	public AudioClip m_enemyHurtClip;
	public AudioClip m_enemyDeathClip;
	public Collider2D m_collider;

	private AudioSource m_enemyAudio;
	private EnemySetup m_eSetup;
	private Animator m_anim;

	void Awake(){
		m_enemyAudio = GetComponent<AudioSource> ();
		m_anim = GetComponent<Animator> ();

		m_eSetup = GetComponent<EnemySetup> ();
	}


	void Start () {
		m_isDead = false;
	}


//	void Update(){
//		if (m_isDead) {
//			m_anim.Play ("Enemy_DEAD");
//		}
//	}
		

	void OnTriggerEnter2D(Collider2D collider){
		if (!collider.CompareTag("Player"))
			return;
		HUDController.m_Instance.m_enemyHUD.gameObject.SetActive (true);
		HUDController.m_Instance.m_enemyHUD.m_Icon.sprite = m_eSetup.m_icon;
		HUDController.m_Instance.m_enemyHUD.UpdateHealth (m_eSetup.m_maxHealth, m_eSetup.m_currentHealth);
	}


	void OnTriggerExit2D(Collider2D collider){
		if (!collider.CompareTag("Player"))
			return;
		HUDController.m_Instance.m_enemyHUD.gameObject.SetActive (false);
	}


	public void TakeDemage(int demage){
		m_eSetup.m_currentHealth -= demage;
		HUDController.m_Instance.m_enemyHUD.UpdateHealth(m_eSetup.m_maxHealth, m_eSetup.m_currentHealth);
		if (m_eSetup.m_currentHealth <= 0) {
			m_isDead = true;
			m_collider.enabled = false;
			m_enemyAudio.clip = m_enemyDeathClip;
			m_anim.SetBool ("dead", m_isDead);
//			m_anim.Play ("Enemy_DEAD");
		} else {
			m_enemyAudio.clip = m_enemyHurtClip;
			//m_anim.SetBool ("hurt", true);
			m_anim.Play ("Enemy_HURT");
		} 
		m_enemyAudio.Play ();
	}
		

	void OnDeath(){
		Destroy (gameObject);
	}
}
