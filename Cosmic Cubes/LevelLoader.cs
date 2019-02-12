using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public Slider m_progressBar;
	public Text m_progressText;

	private AsyncOperation m_ao;


	void Start(){
		m_progressBar.value = 0;
		m_progressText.text = "0%";

		StartCoroutine ("AsynchLoad", GameManager.m_Instance.m_sceneToLoad);
	}


	IEnumerator AsynchLoad (string sceneID)	{
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
				m_ao.allowSceneActivation = true;
				break;
			}
			yield return null;
		}
		yield return null;
	}

}
