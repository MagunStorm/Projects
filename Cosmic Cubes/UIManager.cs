using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;


public class UIManager : MonoBehaviour {

	public Text m_movesCountText;
	public Text m_starCounterText;
	public GameObject m_victoryPanel;
	public GameObject m_hudPanel;
	public GameObject[] m_starsReward;
	public Button m_nextLevelButton;
	public GameObject m_padlock;
	public GameObject m_adButton;
	public GameObject m_fadePanel;

	private CubeController m_cubeController;
	private LevelManager m_levelManager;
	private int m_nextSceneMinStars = 0;
	private string m_nextSceneName = "";
	private Camera m_mainCam;

	void Start(){
		if(m_movesCountText!=null)
			m_movesCountText.text = "0";
		if(m_victoryPanel)
			m_victoryPanel.SetActive (false);
		if(m_adButton)
			m_adButton.SetActive (false);
		if (m_starCounterText)
			m_starCounterText.text = GameManager.m_Instance.m_stars.ToString ();
		if (m_starsReward!=null) {
			foreach (GameObject star in m_starsReward) {
				star.SetActive (false);
			}
		}
		m_mainCam = Camera.main;

		if(m_fadePanel)
			m_fadePanel.SetActive (true);

		m_cubeController = FindObjectOfType<CubeController> ();
		m_levelManager = FindObjectOfType<LevelManager> ();
	}


	void OnEnable () {
		EventManager.StartListening ("UpdateMovesUICount", UpdateMovesUI);
		EventManager.StartListening ("Victory", Victory);
		EventManager.StartListening ("FadeAndRestart", FadeAndRestart);
	}


	void OnDisable () {
		EventManager.StopListening ("UpdateMovesUICount", UpdateMovesUI);
		EventManager.StopListening ("Victory", Victory);
		EventManager.StopListening ("FadeAndRestart", FadeAndRestart);
	}


	public void UpdateMovesUI(){
		m_movesCountText.text = m_levelManager.m_moves.ToString ();
	}


	public void Victory(){
		m_starCounterText.text = GameManager.m_Instance.m_stars.ToString ();

		int reward = m_levelManager.m_currentPerformance;
		for (int i = 0; i < reward; i++) {
			m_starsReward [i].SetActive (true);
		}

		string[] allData = SceneManager.GetActiveScene().name.Split ('_');
		string sceneprefix = allData [0];
		int scenenum = int.Parse(allData[1]);
		string nextscene = sceneprefix + "_" +(scenenum+1).ToString ("D3");

		m_nextSceneName = "LevelSelection";
		foreach (SceneInfo scene in GameManager.m_Instance.m_scenes) {
			if (scene.name == nextscene) {
				m_nextSceneName = scene.name;
				m_nextSceneMinStars = scene.stars;
			}
		}
		GameManager.m_Instance.m_sceneToLoad = m_nextSceneName;

		if (GameManager.m_Instance.m_stars < m_nextSceneMinStars) {
			m_nextLevelButton.interactable = false;
			m_padlock.GetComponentInChildren<Text> ().text = m_nextSceneMinStars.ToString ();
			m_adButton.SetActive (true);
		} else {
			m_nextLevelButton.interactable = true;
			m_adButton.SetActive (false);
			m_padlock.SetActive (false);
		}

		m_hudPanel.SetActive (false);
		m_victoryPanel.SetActive (true);
	}


	public void LoadScene(string name){
		SceneManager.LoadScene (name);
	}


	public void Restart(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}


	public void FadeAndLoad(string action){
		m_fadePanel.GetComponent<Animator> ().Play ("Fade IN");
		StartCoroutine ("WaitAndExecute", action);
	}


	public void FadeAndRestart(){
		m_fadePanel.GetComponent<Animator> ().Play ("Fade IN");
		StartCoroutine ("WaitAndExecute", "Restart");
	}


	IEnumerator WaitAndExecute(string action){
		yield return new WaitForSeconds (1f);
		switch (action) {
		case "Restart":
			Restart ();
			break;
		default:
			LoadScene (action);
			break;
		}
	}


	public void Quit(){
		#if UNITY_EDITOR
		EditorApplication.isPlaying =false;
		#else
		Application.Quit();
		#endif
	}


	public void ClearProgress(){
		PlayerPrefs.DeleteAll ();
		GameManager.m_Instance.m_stars = 0;
	}


	public void LoadNextLevel(){
//			SceneManager.LoadScene (m_nextSceneName);
		SceneManager.LoadScene("Loading");
	}


	public void SetDirection(string dir){
		if (m_cubeController) {
			m_cubeController.SetDir (dir);
		} else {
			Debug.Log ("UIManager: can't assign direction");
		}
	}


	public void TogglePanel(GameObject panel){
		panel.SetActive (!panel.activeInHierarchy);
	}
		

	public void SwipeCamera(bool val){
		m_mainCam.GetComponent<CameraMotor> ().Slidem_camera (val);
	}


	public void ToggleCameraMove(){
		m_mainCam.GetComponent<CameraMotor> ().m_canMoveCamera = !m_mainCam.GetComponent<CameraMotor> ().m_canMoveCamera;
		CubeController cubeController = GameObject.FindObjectOfType<CubeController> ();
		cubeController.enabled = !cubeController.enabled;
	}
		
}
