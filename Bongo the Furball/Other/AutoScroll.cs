using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroll : MonoBehaviour {

	[HideInInspector] public float m_scrollSpeed=0f;

	private Rigidbody2D rb2d;

	void OnEnable ()	{
		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.velocity = new Vector2 (m_scrollSpeed, 0);
	}

}

