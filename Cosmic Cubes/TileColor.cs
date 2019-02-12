using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColor : MonoBehaviour {

	public bool m_applyColor=true;
	public enum Col {White,Black,Red,Blue,Green,Yellow, Dummy};
	public Col color;

	[HideInInspector] public Color m_colorPick;

	private MeshRenderer m_renderer;


	void Start(){
		switch (color) {
		case Col.White:
			m_colorPick = Color.white;
			break;
		case Col.Black:
			m_colorPick = Color.black;
			break;
		case Col.Blue:
			m_colorPick = Color.blue;
			break;
		case Col.Green:
			m_colorPick = Color.green;
			break;
		case Col.Red:
			m_colorPick = Color.red;
			break;
		case Col.Yellow:
			m_colorPick = Color.yellow;
			break;
		case Col.Dummy:
			m_colorPick = Color.clear;
			break;
		default:
			m_colorPick = Color.clear;
			break;
		}

		if (m_applyColor) {
			m_renderer = GetComponent<MeshRenderer> ();
			m_renderer.material.color = m_colorPick;
		}
	}


//	void OnDrawGizmosSelected (){
//		Gizmos.color = m_colorPick;
//		Gizmos.DrawWireSphere (transform.position + m_Position, m_Radius);
//	}

}
