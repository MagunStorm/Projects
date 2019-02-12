using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ButtonContainerManager : MonoBehaviour {

	public GameObject m_puzzleButtonPrefab;

	private string m_puzzleGroupPrefix;
	private bool m_nextLevelLocked=false;


	void Start(){
		int levelCounter = 0;
		m_puzzleGroupPrefix = GameManager.m_Instance.m_selectedWorldPrefix;

		foreach (SceneInfo scene in GameManager.m_Instance.m_scenes) {
			if (!scene.name.StartsWith (m_puzzleGroupPrefix))
				continue;

			levelCounter++;
			GameObject container = Instantiate (m_puzzleButtonPrefab) as GameObject;
			container.transform.SetParent (this.transform, false);

			if (GameManager.m_Instance.m_stars < scene.stars) {
				m_nextLevelLocked = true;
			}
			container.GetComponent<Button> ().interactable = !m_nextLevelLocked;

			PuzzleData level = new PuzzleData (scene.name);
			string button_text = "";
			if (container.GetComponent<Button> ().interactable) {
				button_text = levelCounter.ToString ();
			}
			container.transform.GetChild (0).GetComponent<Text> ().text = button_text;
			container.transform.GetChild (1).gameObject.SetActive (!container.GetComponent<Button> ().interactable);

			if (container.GetComponent<Button> ().interactable) {
				if (level.BestScore == 0) {
					m_nextLevelLocked = true;
					container.GetComponent<PuzzleButton> ().m_resultPanel.SetActive (false);
				} else if (level.BestScore <= level.GoldScore) {
					container.GetComponent<PuzzleButton> ().SetActiveStars (3);
				} else if (level.BestScore <= level.SilverScore) {
					container.GetComponent<PuzzleButton> ().SetActiveStars (2);
				} else {
					container.GetComponent<PuzzleButton> ().SetActiveStars (1);
				}
				container.GetComponent<Button> ().onClick.AddListener (() => LoadLevel (scene.name));
			} else {
				container.GetComponent<PuzzleButton> ().m_resultPanel.SetActive (false);
			}
		}
	}


	private void LoadLevel(string sceneName){
		GameManager.m_Instance.m_sceneToLoad = sceneName;
//		SceneManager.LoadScene (sceneName);
		SceneManager.LoadScene("Loading");
	}
}
