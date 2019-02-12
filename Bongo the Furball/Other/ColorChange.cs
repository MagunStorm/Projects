using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChange : MonoBehaviour {

	public Texture2D m_colorTexture;
	public Image m_Image;
	public Transform m_pointer;
	public float m_pointerSpeed;

	private RectTransform m_Rect;
	private Vector2 m_leftMin;
	private Vector2 m_rightMax;

	void Awake(){
		m_Rect = this.GetComponent<RectTransform> ();
		m_Image.color = Color.blue;
	}


	void Start(){
		Vector3[] v = new Vector3[4];
		m_Rect.GetWorldCorners (v);
		m_leftMin = new Vector2 (v [0].x, v [1].y);
		m_rightMax = new Vector2 (v [2].x, v [3].y);

		SetColorFromPosition((Vector2)m_pointer.position);
	}


	void OnGUI(){
		if (Event.current.type == EventType.MouseUp) {
			Vector2 MousePos = Event.current.mousePosition;
			Vector2 invertMousePos = new Vector2 (MousePos.x, Screen.height-MousePos.y);
			if (MousePos.x < m_leftMin.x || 
				MousePos.x > m_rightMax.x ||
				invertMousePos.y > m_leftMin.y ||
				invertMousePos.y < m_rightMax.y){
				return;
			} 		
			m_pointer.position = new Vector2 (MousePos.x, m_pointer.position.y);
			SetColorFromPosition((Vector2)m_pointer.position);
		}
	}


	void Update(){
		float hor = Input.GetAxis ("Horizontal");
		if (Mathf.Abs (hor) > 0.1f) {
			Vector2 newpos = Vector2.right * hor * Time.deltaTime * m_pointerSpeed;
			m_pointer.position += (Vector3)newpos;
			m_pointer.position = new Vector2(Mathf.Clamp(m_pointer.position.x,m_leftMin.x,m_rightMax.x),m_pointer.position.y);
			SetColorFromPosition((Vector2)m_pointer.position);
		}
	}


	void SetColorFromPosition(Vector2 pos){
		float texPosU = (pos.x - m_leftMin.x) / (m_rightMax.x-m_leftMin.x);
		float texPosV = 0.5f;
		Color textureColor = m_colorTexture.GetPixelBilinear (texPosU, texPosV);

		GameDontDestroy.m_Instance.m_playerColor = textureColor;
		m_Image.color = textureColor;
	}
		
}
