using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	public static HUDController m_Instance;

	public Image m_DemagePanel;
	public Text m_BonusText;
	public HUD m_enemyHUD;
	public HUD m_playerHUD;
	public Slider m_slingShotSlider;
	public VirtualJoystick m_Vjoystick;
	public GameObject m_vFireObj;
	public GameObject m_pauseButton;
	public GameObject m_StonesPanel;
	public Text m_stonesCountTxt;
	public VirtualArrowContainer m_ArrowContainer;
	public TouchInput m_touchInput;

	[Header("HUD Panels")]
	public GameObject m_joystickPanel;
	public GameObject m_JumpShootPanel;
	public GameObject m_TouchMovePanel;
	public GameObject m_TouchJumpFirePanel;

	[HideInInspector] public GameObject m_Player; // Reference to the player's transform.
	[HideInInspector] public int m_UIElementPressedCount;

	private Animator UIanim;


	void Awake(){
		if (m_Instance == null) {
			m_Instance = this;
		} else {
			Destroy (gameObject);
		}

		UIanim = GetComponent<Animator> ();
	}


	void Start(){
		m_BonusText.enabled = false;
		m_enemyHUD.gameObject.SetActive (false);
		m_playerHUD.gameObject.SetActive (false);
		m_StonesPanel.SetActive (false);
		m_vFireObj.SetActive (false);
		m_slingShotSlider.value = 0;
		m_UIElementPressedCount = 0;
	}


	void Update(){
		if (Input.GetButtonDown ("Cancel")) {
			TogglePauseMenu ();
		}
	}


	public void DisplayBonusText(string txt){
		m_BonusText.enabled = true;
//		m_BonusText.text = txt;
		UIanim.Play ("BonusText");
	}


	public void JumpBtn(){
		if (m_Player == null) {
			m_Player = GameObject.FindGameObjectWithTag ("Player");
		}
		m_Player.GetComponent<PlayerController> ().Jump ();
	}
		

	public void TogglePauseMenu(){
		GameController.m_Instance.TogglePauseMenu ();
		m_pauseButton.SetActive (!m_pauseButton.gameObject.activeInHierarchy);
	}


	public void UpdateStonesCount(){
		if (m_Player == null) {
			m_Player = GameObject.FindGameObjectWithTag ("Player");
		}
		m_stonesCountTxt.text = m_Player.GetComponent<PlayerController> ().m_stones.ToString ();
	}

}
