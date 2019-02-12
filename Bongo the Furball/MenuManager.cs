using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using System.IO;
using Fungus;

public class MenuManager : MonoBehaviour {

	public static MenuManager m_Instance;

	[Header("MENU PANELS")]
	public GameObject m_menuPanel;
//	public GameObject m_optionsPanel;
//	public GameObject m_quitPanel;
	public Slider m_zoomSlider;

	[Header("MENU PARAMS")]
	public bool m_freezeTime = true;
	public bool m_enableOnStart = false;
	public Selectable m_FirstSelected;

	private Canvas m_menuCanvas;
	private Animator m_anim;

	void Awake(){
//		if (m_Instance == null) {
//			m_Instance = this;
//		} else {
//			Destroy (gameObject);
//		}
		//DontDestroyOnLoad (gameObject);

		m_menuCanvas = GetComponent<Canvas> ();
		m_anim = GetComponent<Animator> ();
	}


	void Start () {
		Cursor.lockState = CursorLockMode.Confined;
		m_menuCanvas.enabled = m_enableOnStart;

		if (m_zoomSlider != null) {
			m_zoomSlider.value = Camera.main.orthographicSize;
		}
	}


	void OnEnable(){
		if(m_FirstSelected != null){
			m_FirstSelected.interactable = true;
			m_FirstSelected.Select ();
		}

		Cursor.visible = true;
		if (m_freezeTime) {
			Time.timeScale = 0;
		}
			
	}


	void OnDisable(){
		Cursor.visible = false;
		if (m_freezeTime) {
			Time.timeScale = 1;
		}
	}


	void Update(){
		if (Cursor.lockState != CursorLockMode.Confined) {
			Cursor.lockState = CursorLockMode.Confined;
		}
	}


	public void Resume(){
//		gameObject.SetActive (false);
//		GameController.m_Instance.ToggleControls ();
		HUDController.m_Instance.TogglePauseMenu();
	}


	public void Pause()	{
		m_menuCanvas.enabled = !m_menuCanvas.enabled;
		if (m_FirstSelected != null) {
			m_FirstSelected.Select ();
		}
		Cursor.visible = !Cursor.visible;
		if (m_freezeTime) {
			Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		}
		if (m_anim != null) {
			m_anim.SetBool ("MenuON", m_menuCanvas.enabled);
		}
	}
		

	public void Quit()	{
		#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
		#else 
		Application.Quit();
		#endif
	}


	public void Restart(){
		if (GameDontDestroy.m_Instance.Load ("StartLevel.dat")) {
			SceneManager.LoadScene (GameDontDestroy.m_Instance.m_SceneNo);
		} else {
			Debug.Log ("ERROR: RESTART");
		}
	}


	public void NewGame(){
//		GameDontDestroy.m_Instance.Save ("StartLevel.dat", SceneManager.GetActiveScene ().buildIndex+1, Vector3.zero);
//		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex+1);
		GameDontDestroy.m_Instance.DeleteFile("Checkpoint.dat");
		GameDontDestroy.m_Instance.DeleteFile("StartLevel.dat");

		PlayerPrefs.DeleteKey ("JOYSTICK");
		PlayerPrefs.DeleteKey ("BUTTONS");
		PlayerPrefs.DeleteKey ("Zoom");
		GameDontDestroy.m_Instance.m_hideButtons = false;
		GameDontDestroy.m_Instance.m_hideJoystick = false;

		SceneManager.LoadScene("Loading");
	}


	public void MainMenu(){
//		SceneManager.LoadScene ("MainMenu");
		int i = GameDontDestroy.m_Instance.m_hideJoystick ? 1 : 0;
		PlayerPrefs.SetInt ("JOYSTICK", i);
		i = GameDontDestroy.m_Instance.m_hideButtons ? 1 : 0;
		PlayerPrefs.SetInt ("BUTTONS", i);

		PlayerPrefs.SetInt ("nextScene", 1);
		SceneManager.LoadScene("Loading");
	}


	public void Continue(){
		GameDontDestroy.m_Instance.LoadLast();
		PlayerPrefs.SetInt ("nextScene", GameDontDestroy.m_Instance.m_SceneNo);
//		SceneManager.LoadScene(GameDontDestroy.m_Instance.m_SceneNo);
		SceneManager.LoadScene("Loading");
	}


	public void LoadCheckpoint(){
		if (GameDontDestroy.m_Instance.Load ("Checkpoint.dat")) {
			PlayerPrefs.SetInt ("nextScene", GameDontDestroy.m_Instance.m_SceneNo);
			//	SceneManager.LoadScene(GameDontDestroy.m_Instance.m_SceneNo);
			SceneManager.LoadScene("Loading");
		} else {
			Debug.Log ("ERROR: LoadCheckpoint");
		}
	}


	public void LoadScene(int i){
		SceneManager.LoadScene (i);
	}


	public void SetMusicVol(Slider musicSlider){
		AudioController.m_Instance.SetMusicVol (musicSlider.value);
	}


	public void SetMusicVol(float vol){
		AudioController.m_Instance.SetMusicVol (vol);
	}


	public void MuteMusic(){
		AudioController.m_Instance.ToggleMuteMusic ();
	}


	public void SetSfxVol(Slider sfxSlider){
		AudioController.m_Instance.SetSfxVol (sfxSlider.value);
	}


	public void SetSfxVol(float vol){
		AudioController.m_Instance.SetSfxVol (vol);
	}


	public void MuteSfx(){
		AudioController.m_Instance.ToggleMuteSfx ();
	}


//	public void ToggleOptionsPanel(){
//		m_menuPanel.SetActive (!m_menuPanel.activeInHierarchy);
//		m_optionsPanel.SetActive (!m_optionsPanel.activeInHierarchy);
//	}
//
//
//	public void ToggleQuitPanel(){
//		m_menuPanel.SetActive (!m_menuPanel.activeInHierarchy);
//		m_quitPanel.SetActive (!m_quitPanel.activeInHierarchy);
//	}


	public void TogglePanels(GameObject panel){
		m_menuPanel.SetActive (!m_menuPanel.activeInHierarchy);
		panel.SetActive (!panel.activeInHierarchy);
	}


	public void SetLanguageFile(string file){
		LocalizationManager.instance.LoadLocalizedText (file);
	}


	public void SetLanguage(string lang){
		LocalizationManager.instance.SetLanguage (lang);
	}


	public void ResizeView(Slider s){
		Camera.main.orthographicSize = s.value;
		PlayerPrefs.SetFloat ("Zoom", Camera.main.orthographicSize);
		foreach (View v in GameController.m_Instance.views) {
			v.ViewSize = s.value;
			GameController.m_Instance.m_cam.SetCamBoundries ();
			GameController.m_Instance.m_cam.MoveCamera ();
			GameDontDestroy.m_Instance.m_cameraSize = s.value;
		}
	}
}