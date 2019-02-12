using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualFireButton : MonoBehaviour,IPointerUpHandler,IPointerDownHandler {

	public Slider m_SlingshotSlider;
	public float m_MinLaunchForce = 150f;
	public float m_MaxLaunchForce = 350f; 
	public float m_MaxChargeTime = 0.75f;
	public AudioSource m_audio;

	[HideInInspector] public float m_CurrentLaunchForce;  

	private float m_ChargeSpeed;         
	private bool m_buttonDown;
	private Image m_bgImg;
	private Image m_sliderBGImage;

	void Start(){
		m_bgImg = GetComponent<Image> ();

		if (m_SlingshotSlider != null) {
			m_sliderBGImage = m_SlingshotSlider.gameObject.transform.GetChild (0).GetComponent<Image> ();
			m_sliderBGImage.enabled = false;
			m_SlingshotSlider.minValue = m_MinLaunchForce;
			m_SlingshotSlider.maxValue = m_MaxLaunchForce;
			m_buttonDown = false;
			m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
		}
	}


	void Update(){
		if (m_buttonDown) {
			m_sliderBGImage.enabled = true;
			m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
			m_SlingshotSlider.value = m_CurrentLaunchForce;
		} else {
			m_CurrentLaunchForce = m_MinLaunchForce;
			m_SlingshotSlider.value = m_CurrentLaunchForce;
		}
	}


	public virtual void OnPointerDown(PointerEventData ped){
		Vector2 pos = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle
			(m_bgImg.rectTransform,
				ped.position,
				ped.pressEventCamera,
				out pos) && GameController.m_Instance.m_Player.GetComponent<PlayerController> ().m_stones > 0) {

//			m_audio.Play ();
			m_buttonDown = true;
			HUDController.m_Instance.m_UIElementPressedCount++;
		}
	}


	public virtual void OnPointerUp(PointerEventData ped){
//		m_audio.Stop();
		m_buttonDown = false;
		HUDController.m_Instance.m_UIElementPressedCount--;
		GameController.m_Instance.m_Player.GetComponent<PlayerController> ().Fire (m_CurrentLaunchForce);
		HUDController.m_Instance.UpdateStonesCount ();
		m_sliderBGImage.enabled = false;
	}
}
