using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneInfo : MonoBehaviour {

	public int m_nextScene;
	[HideInInspector] public int m_sceneID;
	[HideInInspector] public string m_sceneName;

	void Awake () {
		if (m_nextScene < 0) {
			m_nextScene = SceneManager.GetActiveScene ().buildIndex + 1;
		}
		PlayerPrefs.SetInt ("nextScene", m_nextScene);

		m_sceneID = SceneManager.GetActiveScene ().buildIndex;
		m_sceneName = SceneManager.GetActiveScene ().name;
	}

}
