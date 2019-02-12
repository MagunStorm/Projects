using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour {

	public string m_FurryName;
	[Header("HUD")]
	public Color m_FurryColor = Color.blue;
	public SpriteRenderer m_FurryRend;
	[Header("CONTROLS")]
	public float m_maxXSpeed = 3f;
	public float m_maxYSpeed = 15f;
	public float m_maxSwimSpeed = 5f;
	public float m_jumpForce = 10f;
	public int m_MaxJumps = 1;
	[Header("HEALTH")]
	public int m_maxHealth = 4;
	public int m_maxOxygen = 0;

	[HideInInspector] public int m_currentOxygen = 0;
	[HideInInspector] public int m_currentHealth = 0;
	[HideInInspector] public bool m_isActive = false;


	void Awake(){


		//Pobranie danych Futrzaka z DontDestroy
		m_maxHealth = GameDontDestroy.m_Instance.m_health;
		m_maxOxygen = GameDontDestroy.m_Instance.m_oxy;
		m_MaxJumps = GameDontDestroy.m_Instance.m_jumps;
		m_currentOxygen = m_maxOxygen;
		m_currentHealth = m_maxHealth;
		m_FurryColor = GameDontDestroy.m_Instance.m_playerColor;
	
		m_FurryRend.color = m_FurryColor;
	}


	void Start(){
		HUDController.m_Instance.m_playerHUD.gameObject.SetActive (true);
		UpdateHUD ();
	}


	public void UpdateOxygen(int val){
		m_currentOxygen += val;
		m_currentOxygen = Mathf.Clamp (m_currentOxygen, 0, m_maxOxygen);
//		m_playerHUD.UpdatePlayerOxygen (m_currentOxygen);
		HUDController.m_Instance.m_playerHUD.UpdateOxygen(m_maxOxygen,m_currentOxygen);

		if (m_currentOxygen == 0) {
			PlayerHealth ph = GetComponent<PlayerHealth> ();
			ph.ModifyHealth (val);
		}
	}


	public void UpdateHUD(){
		HUDController.m_Instance.m_joystickPanel.SetActive (!GameDontDestroy.m_Instance.m_hideJoystick);
		HUDController.m_Instance.m_TouchMovePanel.SetActive (GameDontDestroy.m_Instance.m_hideJoystick);
		HUDController.m_Instance.m_JumpShootPanel.SetActive (!GameDontDestroy.m_Instance.m_hideButtons);
		HUDController.m_Instance.m_TouchJumpFirePanel.SetActive (GameDontDestroy.m_Instance.m_hideButtons);

		HUDController.m_Instance.m_playerHUD.m_Icon.color = m_FurryColor;
		HUDController.m_Instance.m_playerHUD.UpdateHealth (m_maxHealth, m_currentHealth);
		HUDController.m_Instance.m_playerHUD.UpdateOxygen (m_maxOxygen, m_currentOxygen);
		HUDController.m_Instance.m_vFireObj.SetActive (GetComponent<PlayerController> ().m_canFire);
		HUDController.m_Instance.m_StonesPanel.SetActive(GetComponent<PlayerController> ().m_canFire);
		HUDController.m_Instance.UpdateStonesCount ();
	}


	public void ResetStats(){
		m_currentHealth = m_maxHealth;
		m_currentOxygen = m_maxOxygen;
	}
		
}
