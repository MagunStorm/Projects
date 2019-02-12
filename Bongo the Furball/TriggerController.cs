using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class TriggerController : MonoBehaviour {
	
	public string[] m_CollisionTags;
	public bool m_destroyOnContact = false;
//	public bool m_interactable = false;
	[Header("ENTER action")]
	public UnityEvent m_Action;
	[Header("LOOP action")]
	public float m_ActionTimeRate = 1f;
	public UnityEvent m_LoopAction;
	[Header("EXIT action")]
	public UnityEvent m_ExitAction;
	[Header("INTERACT action")]
	public UnityEvent m_InteractAction;
	[Header("AUDIO")]
	public AudioClip m_EnterClip;
	public AudioClip m_LoopClip;
	public AudioClip m_ExitClip;

	private GameObject m_Player;
	private GameObject m_other;
	private float m_nextActionTime = 0f;


	void Update(){
		if (m_InteractAction == null || m_Player == null) {
			return;
		}
			
		if(Input.GetButtonUp("Action")){
			if (m_InteractAction != null) {
				m_InteractAction.Invoke ();
			}
		}
	}


	void OnTriggerEnter2D(Collider2D other){
		if (!CheckTag (other))
			return;

		m_other = other.gameObject;
		if (other.CompareTag ("Player")) {
			m_Player = other.gameObject;
		}

		SFXController.SetAudioLoop (false);
		SFXController.SetAudio(m_EnterClip);

		if (m_Action != null) {
			m_Action.Invoke ();
		}

		if (m_destroyOnContact) {
			Destroy (gameObject);
		}
			
	}


	void OnTriggerStay2D(Collider2D other){
		if (!CheckTag (other))
			return;
		if (Time.time > m_nextActionTime) {
			m_nextActionTime = Time.time + m_ActionTimeRate;
			if (m_LoopAction != null) {
				m_LoopAction.Invoke ();
			}
		}
	}


	void OnTriggerExit2D(Collider2D other){
		if (!CheckTag (other))
			return;

		if (m_ExitAction != null) {
			m_ExitAction.Invoke ();
		}
		
		m_Player = null;
		m_other = null;

		SFXController.SetAudioLoop (false);
		SFXController.SetAudio (m_ExitClip);
	}


	bool CheckTag(Collider2D col){
		bool result = false;

		if (m_CollisionTags.Length == 0) {
			result = true;
		}
		if (m_CollisionTags.Length>0) {
			foreach (string s in m_CollisionTags) {
				if (col.CompareTag (s)) {
					result = true;
				}
			}
		}
		return result;
	}


/////////////////////////////////////////////////
//	Poniżej różne funkcje wywoływane w triggerze
/// ////////////////////////////////////////////

	public void ModHP(int hp){
		PlayerHealth ph = m_Player.GetComponent<PlayerHealth> ();
		ph.ModifyHealth (hp);
	}


	public void PlayerBounceOff(){
		bool collisionSide = m_Player.transform.position.x < gameObject.transform.position.x ? true : false;
		m_Player.GetComponent<PlayerController> ().BounceOff (collisionSide);
	}


	public void ChangeMaxHealth(int val){
		PlayerSetup pc = m_Player.GetComponent<PlayerSetup> ();
		pc.m_maxHealth += val;
		ModHP (100);
//		PlayerHealth ph = m_Player.GetComponent<PlayerHealth> ();
//		ph.ModifyHealth (100);
	}
		

	public void DoubleJump(){
		PlayerSetup pc = m_Player.GetComponent<PlayerSetup> ();
		pc.m_MaxJumps = 2;
	}


	public void Slingshot(){
		PlayerController pc = m_Player.GetComponent<PlayerController> ();
		pc.m_canFire = true;
		HUDController.m_Instance.m_vFireObj.SetActive (true);
		HUDController.m_Instance.m_StonesPanel.SetActive (true);
		Stones ();
	}


	public void BonusText(string block){
//		HUDController.m_Instance.DisplayBonusText (txt);
		GameController.m_Instance.m_levelFlowchart.ExecuteBlock(block);
	}


	public void Water(float dist){
		PlayerSetup pSetup;

		if (m_Player.GetComponent<PlayerController> ().isWater) 	//tylko dla graczy, którzy są w wodzie
		{
			pSetup = m_Player.GetComponent<PlayerSetup> ();
			float surfaceY = gameObject.transform.localPosition.y + GetComponent<BuoyancyEffector2D> ().surfaceLevel;
			float dist_from_sourface = m_Player.transform.position.y - surfaceY;
			if (m_Player.transform.position.y < surfaceY && Mathf.Abs (dist_from_sourface) > dist) {
				pSetup.UpdateOxygen (-1);
				SFXController.SetAudioLoop (false);
				SFXController.SetAudio (m_LoopClip);
			} else {
				pSetup.UpdateOxygen (5);
			}
		}

	}


	public void ClampWaterMovement(){
		float surfaceY = gameObject.transform.position.y + GetComponent<BuoyancyEffector2D> ().surfaceLevel;
		float posY = m_Player.transform.position.y >= surfaceY ? surfaceY : m_Player.transform.position.y;
		m_Player.transform.position = new Vector3 (m_Player.transform.position.x, posY, m_Player.transform.position.z);
	}


	public void SaveCheckpoint(){
		GameDontDestroy.m_Instance.Save ("Checkpoint.dat", SceneManager.GetActiveScene ().buildIndex, this.transform.position);
		Animator anim = GetComponent<Animator> ();
		anim.Play ("Checkpoint");
//		GameController.m_Instance.m_levelFlowchart.ExecuteBlock ("CHECKPOINT");
	}


	public void SaveStartLevel(){
		GameDontDestroy.m_Instance.Save ("StartLevel.dat", SceneManager.GetActiveScene ().buildIndex, Vector3.zero);
	}


	public void Exit(){
		GameController.m_Instance.m_levelFlowchart.ExecuteBlock ("EXIT");
	}


	public void ChangeParent(){
//		m_Player.transform.parent = this.transform;
		m_other.transform.parent = this.transform;
	}


	public void Stones(){
		m_Player.GetComponent <PlayerController> ().m_stones = m_Player.GetComponent <PlayerController> ().m_maxStones;
		HUDController.m_Instance.UpdateStonesCount ();
		this.gameObject.SetActive (false);
	}


	public void PlayerRotationZ(bool val){
		float rotationY = m_Player.GetComponent<PlayerController> ().lookingRight ? 0 : 180f;
		Quaternion newRotarion = new Quaternion (m_Player.transform.rotation.x, rotationY, 0, m_Player.transform.rotation.w);
		m_Player.transform.rotation = newRotarion;
		m_Player.GetComponent<Rigidbody2D> ().freezeRotation = val;
	}
		


}
