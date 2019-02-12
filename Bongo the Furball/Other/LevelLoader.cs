using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fungus;

public class LevelLoader : MonoBehaviour {

	public Flowchart m_flowchart;
	public Slider m_progressBar;
	public Text m_progressText;

	private AsyncOperation m_ao;


	void Start(){
		int sceneID = 0;
		m_progressBar.value = 0;
		m_progressText.text = "0%";
		if (PlayerPrefs.HasKey ("nextScene"))
			sceneID = PlayerPrefs.GetInt ("nextScene");
		StartCoroutine ("AsynchLoad", sceneID);
	}


	IEnumerator AsynchLoad (int sceneID)	{
		yield return null;

		m_ao = SceneManager.LoadSceneAsync(sceneID);
		m_ao.allowSceneActivation = false;

		while (! m_ao.isDone)	{
			// [0, 0.9] > [0, 1]
			float progress = Mathf.Clamp01(m_ao.progress / 0.9f);
			m_progressBar.value = progress;
			if (progress > 0f) {
				m_progressText.text = "" + (progress * 100).ToString ("00") + "%";
			}
			if (m_ao.progress == 0.9f){
				m_progressBar.value = 1;
				m_progressText.text = "100%";
				m_flowchart.ExecuteBlock("EXIT");
				break;
			}
			yield return null;
		}
		yield return null;
	}


	public void SetAllowSceneActivation(bool val){
		m_ao.allowSceneActivation = val;
	}
}
