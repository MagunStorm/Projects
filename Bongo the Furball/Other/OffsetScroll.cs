using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScroll : MonoBehaviour {

	public float m_scrollSpeed;

	private Vector2 m_savedOffset;
	private Renderer m_rend;

	void Start () {
		m_rend = GetComponent<Renderer> ();
		m_savedOffset = m_rend.sharedMaterial.GetTextureOffset ("_MainTex");
	}

	void Update () {
		float x = Mathf.Repeat (Time.time * m_scrollSpeed, 1);
		Vector2 offset = new Vector2 (x, m_savedOffset.y);
		m_rend.sharedMaterial.SetTextureOffset ("_MainTex", offset);
	}

	void OnDisable () {
		m_rend.sharedMaterial.SetTextureOffset ("_MainTex", m_savedOffset);
	}
}
