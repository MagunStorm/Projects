using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Image m_Icon;
	[Header("HEALTH")]
	public Slider m_HealthSlider;
	public RectTransform m_HealthBackgroundRect;
	public RectTransform m_HealthFillAreaRect;
	[Header("OXYGEN")]
	public Slider m_OxygenSlider;


	public void UpdateHealth(int maxval, int val){
		m_HealthSlider.maxValue = maxval;
		m_HealthSlider.value = val;
		m_HealthBackgroundRect.sizeDelta = new Vector2 (maxval * 54,m_HealthBackgroundRect.rect.height);
		m_HealthFillAreaRect.sizeDelta = new Vector2 (maxval * 54,m_HealthFillAreaRect.rect.height);
	}


	public void UpdateOxygen(int maxval, int val){
		m_OxygenSlider.maxValue = maxval;
		m_OxygenSlider.value = val;
	}
		
}
