using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Fungus;
using UnityEngine.UI;
using System.IO;

public class ActionController : MonoBehaviour {

	[Header("AWAKE action")]
	public UnityEvent m_AwakeAction;
	[Header("OnEnable action")]
	public UnityEvent m_EnableAction;
	[Header("START action")]
	public UnityEvent m_StartAction;
	[Header("UPDATE action")]
	public float m_ActionTimeRate = 2f;
	public UnityEvent m_UpdateAction;
	[Header("OnDisable action")]
	public UnityEvent m_DisableAction;
	[Header("OnDestroy action")]
	public UnityEvent m_DestroyAction;

	private float m_nextActionTime = 0f;

	void Awake () {
		if (m_AwakeAction != null) {
			m_AwakeAction.Invoke ();
		}
	}


	void OnEnable(){
		if (m_EnableAction != null) {
			m_EnableAction.Invoke ();
		}
	}


	void Start () {
		if (m_StartAction != null) {
			m_StartAction.Invoke ();
		}
	}


	void Update(){
		if (Time.time > m_nextActionTime) {
			m_nextActionTime = Time.time + m_ActionTimeRate;
			if (m_UpdateAction != null) {
				m_UpdateAction.Invoke ();
			}
		}
	}


	void OnDisable(){
		if (m_DisableAction != null) {
			m_DisableAction.Invoke ();
		}
	}


	void OnDestroy(){
		if (m_DestroyAction != null) {
			m_DestroyAction.Invoke ();
		}
	}


//////////////////////////////////////
//	Funkcje uruchamiane w ActionController
/////////////////////////////////////////


	public void Skip(Flowchart flow){
			flow.ExecuteBlock ("EXIT");
	}


	public void ButtonInteractable(Button but){
//		bool checkpoint = File.Exists (Application.dataPath + "/SaveData/Checkpoint.dat");
		bool checkpoint = File.Exists (Application.persistentDataPath+"/Checkpoint.dat");
		bool startlevel = File.Exists (Application.persistentDataPath+"/StartLevel.dat");
		but.interactable = checkpoint || startlevel ? true : false;
	}


	public void ToggleCollider(Collider2D col){
		col.enabled = !col.enabled;
	}


	public void ToggleAudioSource(AudioSource au){
		au.enabled = !au.enabled;
	}


	public void ToggleFlag(Animator anim){
		anim.SetBool ("flagUP", !anim.GetBool("flagUP"));
	}


	public void TestAction(string t){
//		t.text = Application.dataPath;
		Debug.Log(t);
	}


	public void SetFungusImageColor(Image img){
		img.color = GameDontDestroy.m_Instance.m_playerColor;
	}


	public void ToggleIsActive(GameObject obj){
		obj.SetActive (!obj.activeInHierarchy);
	}


	public void SetCursor(bool val){
		Cursor.visible = val;
		Cursor.lockState = CursorLockMode.Confined;
	}


	public string GetLanguage(){
		return LocalizationManager.instance.m_language;
	}


	public void NewGameSave(int sceneID){
		GameDontDestroy.m_Instance.Save ("StartLevel.dat", sceneID, Vector3.zero);
	}


	public void ResizeView(View v){
		v.ViewSize = GameDontDestroy.m_Instance.m_cameraSize;
	}

}
