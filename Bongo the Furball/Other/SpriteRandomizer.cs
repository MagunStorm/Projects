using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour {

	public Sprite[] m_Sprites;

	private SpriteRenderer m_rend;


	void Awake(){
		m_rend = GetComponentInChildren<SpriteRenderer> ();
	}


	void Start () {
		RandomizeSprite ();
	}


	public void RandomizeSprite(){
		if (m_rend = null) {
			Debug.Log ("SpriteController: brak komponentu SpriteRenderer");		
			return;
		}
		int r = Random.Range (0, m_Sprites.Length);
		Debug.Log ("rand: " + r);
		Debug.Log ("m_Sprites[r]: " + m_Sprites [r]);
		m_rend.sprite = m_Sprites [r];
	}

}
