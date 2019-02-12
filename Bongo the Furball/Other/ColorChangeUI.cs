using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangeUI : MonoBehaviour {

	public Texture2D m_colorTexture;
	public Image m_PlayerImage;
	public Transform m_pointer;
	public float m_pointerSpeed;

	private RectTransform m_Rect;
	private Vector2 m_leftMin;
	private Vector2 m_rightMax;
	private Slider m_colorSlider;

	void Awake(){
		m_Rect = this.GetComponent<RectTransform> ();
		m_PlayerImage.color = Color.blue;
		m_colorSlider = GetComponent<Slider> ();
	}


	void Start(){
		Vector3[] v = new Vector3[4];
		m_Rect.GetWorldCorners (v);
		m_leftMin = new Vector2 (v [0].x, v [1].y);
		m_rightMax = new Vector2 (v [2].x, v [3].y);

		SetColorFromHandle ();
	}


	void Update(){
		float hor = Input.GetAxis ("Horizontal");
		if (Mathf.Abs (hor) > 0.1f) {
			m_colorSlider.value += hor * Time.deltaTime * m_pointerSpeed;
			SetColorFromHandle ();
		}
	}


	public void SetColorFromHandle(){
		Vector3 pos = m_pointer.position;
		float texPosU = (pos.x - m_leftMin.x) / (m_rightMax.x-m_leftMin.x);
		float texPosV = 0.5f;
		Color textureColor = m_colorTexture.GetPixelBilinear (texPosU, texPosV);

		GameDontDestroy.m_Instance.m_playerColor = textureColor;
		m_PlayerImage.color = textureColor;
	}


	public void PrintPosition(GameObject obj){
		Debug.Log ("handle pos: " + obj.transform.position);
	}


}
