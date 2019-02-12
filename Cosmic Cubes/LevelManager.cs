using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

//	public int m_minStars;
	public int m_goldScore;
	public int m_silverScore;

	[HideInInspector] public int m_moves;
	[HideInInspector] public int m_levelReward;
	[HideInInspector] public int m_currentPerformance;

	private CubeController m_cubeController;
	private GameObject[] m_targetTiles;
	private int m_victoryCount;

	void Start(){
		m_cubeController = FindObjectOfType<CubeController> ();
		m_targetTiles = GameObject.FindGameObjectsWithTag ("TargetTile");
		m_victoryCount = 0;

		m_moves = m_cubeController.m_cubes.Length * -1;
	}


	void OnEnable () {
		EventManager.StartListening ("UpdateMovesCount", UpdateMoves);
		EventManager.StartListening ("Score", Score);
		EventManager.StartListening ("UnScore", UnScore);
	}


	void OnDisable () {
		EventManager.StopListening ("UpdateMovesCount", UpdateMoves);
		EventManager.StopListening ("Score", Score);
		EventManager.StopListening ("UnScore", UnScore);
	}


	public void UpdateMoves(){
		m_moves++;
		EventManager.TriggerEvent("UpdateMovesUICount");
	}


	public void CheckVictory(){
		if (m_victoryCount == m_targetTiles.Length) {
			m_cubeController.DeselectAll ();

			m_currentPerformance = 0;
			if (m_moves <= m_goldScore) {
				m_currentPerformance = 3;
			} else if (m_moves <= m_silverScore) {
				m_currentPerformance = 2;
			} else{
				m_currentPerformance = 1;
			}

			PuzzleData puzzle = new PuzzleData (SceneManager.GetActiveScene ().name);

			m_levelReward = m_currentPerformance - puzzle.stars;
			GameManager.m_Instance.m_stars += m_levelReward;
			GameManager.m_Instance.Save ();

			string saveString="";
			saveString += (puzzle.BestScore > m_moves || puzzle.BestScore==0) ? m_moves.ToString () : puzzle.BestScore.ToString ();
			saveString += '&';
			saveString += m_silverScore.ToString ();
			saveString += '&';
			saveString += m_goldScore.ToString ();
			saveString += '&';
			saveString += m_currentPerformance.ToString ();
			PlayerPrefs.SetString (SceneManager.GetActiveScene ().name, saveString);

//			m_victoryPanel.SetActive (true);
			EventManager.TriggerEvent("Victory");
		}
	}


	public void Score(){
		m_victoryCount++;
		m_victoryCount = Mathf.Clamp (m_victoryCount, 0, m_targetTiles.Length);
		CheckVictory ();
	}


	public void UnScore(){
		m_victoryCount--;
		m_victoryCount = Mathf.Clamp (m_victoryCount, 0, m_targetTiles.Length);
	}

}
