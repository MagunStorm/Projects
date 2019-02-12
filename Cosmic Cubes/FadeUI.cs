using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUI : MonoBehaviour {

	public float m_fadeSpeed = 1.2f;

	private CanvasGroup m_canvasGroup;
	private bool m_fadeIN;
	private bool m_fadeOUT;

	void Start () {
		m_fadeIN = false;
		m_fadeOUT = false;
		m_canvasGroup = GetComponent<CanvasGroup> ();
		m_canvasGroup.alpha = 0f;
	}
	

	void Update () {
		if (m_fadeOUT) {
			m_canvasGroup.alpha = Mathf.Lerp (m_canvasGroup.alpha, 1f, m_fadeSpeed * Time.deltaTime);
			if (m_canvasGroup.alpha > 0.9f) {
				m_canvasGroup.alpha = 1f;
				m_fadeOUT = false;
			}
		}

		if (m_fadeIN) {
			m_canvasGroup.alpha = Mathf.Lerp (m_canvasGroup.alpha, 0f, m_fadeSpeed *2* Time.deltaTime);
			if (m_canvasGroup.alpha < 0.05f) {
				m_canvasGroup.alpha = 0f;
				m_fadeIN = false;
			}
		}
	}


	public void FadeIN(){
		m_fadeIN = true;
	}


	public void FadeOUT(){
		m_fadeOUT = true;
	}


}
