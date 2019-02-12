using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostFader : MonoBehaviour {

	public float m_fadeSpeed;
	public float m_maxFrost;

	private FrostEffect m_frost;

	void Start () {
		m_frost = GetComponent<FrostEffect> ();
	}
	

	void Update () {
		m_frost.FrostAmount = Mathf.Lerp (m_frost.FrostAmount, m_maxFrost, Time.deltaTime * m_fadeSpeed);
	}
}
