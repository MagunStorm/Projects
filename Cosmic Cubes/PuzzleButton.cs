using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour {

	public GameObject m_resultPanel;
	public GameObject[] m_rewardImgs;


	void Awake () {
		foreach (GameObject star in m_rewardImgs) {
			star.SetActive (false);
		}
	}
	

	public void SetActiveStars(int no){
		for (int i = 0; i < no; i++) {
			m_rewardImgs [i].SetActive (true);
		}
	}

}
