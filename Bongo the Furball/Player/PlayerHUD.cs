using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

	public Image m_playerIcon;
	[Header("HEALTH")]
	public Slider m_playerHealthSlider;
	[Header("OXYGEN")]
	public Slider m_playerOxygenSlider;


	public void ShowPlayerHUD(){
		m_playerHealthSlider.gameObject.SetActive (true);
	}


	public void HidePlayerHUD(){
		m_playerHealthSlider.gameObject.SetActive (false);
	}


	public void UpdatePlayerHealth(int val){
		m_playerHealthSlider.value = val;
	}


	public void UpdatePlayerOxygen(int val){
		m_playerOxygenSlider.value = val;
	}


	public void SetHUDSize (float size){
		RectTransform rect = GetComponent<RectTransform> ();
		if (rect != null) {
			rect.localScale = new Vector3 (size, size, 1f);
		}
	}
		

	public void RepositionHUD(Vector2 newpos){
		RectTransform rect = GetComponent<RectTransform> ();
		rect.anchoredPosition = newpos;
	}


}
