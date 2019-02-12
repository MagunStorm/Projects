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
			
//		if(Input.GetButtonUp("Action")){
//			if (m_InteractAction != null) {
//				m_InteractAction.Invoke ();
//			}
//		}
	}


	void OnTriggerEnter(Collider other){
		if (!CheckTag (other))
			return;

		m_other = other.gameObject;
		if (other.CompareTag ("Player")) {
			m_Player = other.gameObject;
		}

//		SFXController.SetAudioLoop (false);
//		SFXController.SetAudio(m_EnterClip);

		if (m_Action != null) {
			m_Action.Invoke ();
		}

		if (m_destroyOnContact) {
			Destroy (gameObject);
		}
			
	}


	void OnTriggerStay(Collider other){
		if (!CheckTag (other))
			return;
		if (Time.time > m_nextActionTime) {
			m_nextActionTime = Time.time + m_ActionTimeRate;
			if (m_LoopAction != null) {
				m_LoopAction.Invoke ();
			}
		}
	}


	void OnTriggerExit(Collider other){
		if (!CheckTag (other))
			return;

		if (m_ExitAction != null) {
			m_ExitAction.Invoke ();
		}
		
		m_Player = null;
//		m_other = null;

//		SFXController.SetAudioLoop (false);
//		SFXController.SetAudio (m_ExitClip);
	}


	bool CheckTag(Collider col){
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

	public void RestartLevel(){
		EventManager.TriggerEvent ("FadeAndRestart");

//		if (m_Player.GetComponent<Cube> ().m_cubeMaterial == Cube.CubeMaterial.DUMMY) {
//			Debug.Log ("DUMMY");
//		} else {
//			EventManager.TriggerEvent ("FadeAndRestart");
//		}
	}
		

	public void ToggleBridge(GameObject tile){
		tile.GetComponent<MeshRenderer> ().enabled = !tile.GetComponent<MeshRenderer> ().enabled;
		tile.transform.GetChild (1).gameObject.SetActive (!tile.transform.GetChild (1).gameObject.activeInHierarchy);
	}


	public void ToggleObstacle(GameObject obs){
		obs.SetActive (!obs.activeInHierarchy);
	}


	public void DeselectCube(){
		if (!this.gameObject.transform.GetChild (1).gameObject.activeInHierarchy) {
			EventManager.TriggerEvent ("DeSelect");
		}
	}

}
