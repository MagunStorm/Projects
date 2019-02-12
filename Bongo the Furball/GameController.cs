using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fungus;

public class GameController : MonoBehaviour {

	public static bool m_gameOver = false;
	public static GameController m_Instance;
//	public static bool m_CanPlayersMove = true;

	public Transform m_BackgroundPosition;
	public MenuManager m_Menu;
	public LayerMask m_GroundLayer;
	public LayerMask m_EnemyLayer;
	public Transform m_defaultSpawnPoint;
	public Transform m_defaultViewPoint;
	public float m_enableCameraDelay = 0;
	[Header("FUNGUS")]
	public Flowchart m_levelFlowchart;
	public Flowchart m_NarrativeFlowchart;
	public Image m_CharacterImage;
	public View m_view;

	[HideInInspector] public GameObject m_Player;
	[HideInInspector] public View[] views;
	[HideInInspector] public CameraFollowObject m_cam;

	private bool m_GameOverMenuON = false;
	private Transform m_spawnPoint;


	void Awake(){
		if (m_Instance == null) {
			m_Instance = this;
		} else {
			Destroy (gameObject);
		}

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;

		GameObject obj = GameObject.FindGameObjectWithTag ("MainCamera");
		if (obj != null) {
			m_cam = obj.GetComponent<CameraFollowObject> ();
		}

		m_spawnPoint = m_defaultSpawnPoint;
		m_gameOver = false;
		m_Menu.gameObject.SetActive (false);

		views = GameObject.FindObjectsOfType<View> ();
	}


	void Start(){
		SetSpawnPoint (GameDontDestroy.m_Instance.m_SpawnPoint);
		if (m_BackgroundPosition != null) {
			m_BackgroundPosition.position = new Vector3 (m_spawnPoint.position.x, m_BackgroundPosition.position.y);
		}
		m_view.transform.position = m_defaultViewPoint.position;
		m_view.ViewSize = GameDontDestroy.m_Instance.m_cameraSize;

//		Debug.Log ("views.Length: " + views.Length);

		m_levelFlowchart.ExecuteBlock ("ENTER");
	}


	void Update()	{		
//		if(!m_GameOverMenuON && Input.GetButtonDown("Cancel")){
//			m_Menu.gameObject.SetActive (!m_Menu.isActiveAndEnabled);
//			ToggleControls ();	
//		}

		if(!m_GameOverMenuON && m_gameOver){
			m_GameOverMenuON = true;
			m_levelFlowchart.ExecuteBlock ("Player_DEATH");
		}

	}


	public void TogglePauseMenu(){
		m_Menu.gameObject.SetActive (!m_Menu.isActiveAndEnabled);
		ToggleControls ();
	}


	public void ToggleControls(){
		if (m_Player != null) {
			m_cam.enabled = !m_Menu.gameObject.activeInHierarchy;
			m_Player.GetComponent<PlayerController> ().enabled = !m_Menu.gameObject.activeInHierarchy;
		}
	}


	// Utworzenie dowolnego obiektu z Prefab
	public GameObject SpawnObject(GameObject objPrefab, Transform objPos){
		GameObject obj = Instantiate (objPrefab, objPos.position, Quaternion.identity);
		return obj;
	}


	public void SpawnPlayer(GameObject playerPrefab){
		GameObject player = SpawnObject (playerPrefab, m_spawnPoint);
		m_Player = player;
		m_CharacterImage.color = GameDontDestroy.m_Instance.m_playerColor;
		Invoke("EnableCamera",m_enableCameraDelay);
	}


	public void SetSpawnPoint(Vector3 pos){
		if (pos == Vector3.zero) {
			m_spawnPoint = m_defaultSpawnPoint;
		} else {
			m_spawnPoint.position = pos;
			m_defaultViewPoint = m_spawnPoint;
		}
	}


	void EnableCamera(){
		if (!m_cam.enabled)
			m_cam.enabled = true;
		m_cam.m_Player = m_Player.transform;		// wycentrowanie kamery na aktywowanym graczu
	}


	public void FreezePlayers(){
//		m_CanPlayersMove = false;
		m_Player.GetComponent<PlayerController> ().canMove=false;
	}


	public void UnFreezePlayers(){
//		m_CanPlayersMove = true;
		m_Player.GetComponent<PlayerController> ().canMove=true;
	}


	public void StopPlayer(){
		m_Player.GetComponent<PlayerController> ().StopMoving();
	}


	public void SetPlayerAUTOMove(bool val){
		PlayerController pCtrl = m_Player.GetComponent<PlayerController> ();
		if (pCtrl != null) {	
			pCtrl.m_autoMove = val;
		} else {
			Debug.Log ("pCtrl == NULL");
		}
	}


	//włącza automatyczne poruszanie wskazanego Futrzaka
	public void PlayerAutoMoveON(GameObject pl){
		pl.GetComponent<PlayerController> ().m_autoMove = true;
	}


	//wyłącza automatyczne poruszanie wskazanego Futrzaka
	public void PlayerAutoMoveOFF(GameObject pl){
		pl.GetComponent<PlayerController> ().m_autoMove = false;
	}
		

	public void PlayObjectAnimState(GameObject obj, string state){
		Animator anim = obj.GetComponent<Animator> ();
		if (anim != null) {
			anim.Play (state);
		} else {
			Debug.Log ("ObjectANIM == null");
		}
	}


	public void PlayObjectAudio(GameObject obj){
		AudioSource audio = obj.GetComponent<AudioSource> ();
		if (audio != null) {
			audio.Play ();
		} else {
			Debug.Log ("ObjectAUDIO == null");
		}
	}

		
	public void SetTimeScale(float scale){
		Time.timeScale = scale;
	}


	public string GetLanguage(){
//		Debug.Log("Language loaded: "+LocalizationManager.instance.m_language);
		return LocalizationManager.instance.m_language;
	}
		
}
