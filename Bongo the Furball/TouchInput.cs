using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class TouchInput : MonoBehaviour , IDragHandler,IPointerUpHandler,IPointerDownHandler {

	public enum InputType {MOVE,JUMP,FIRE};

	public InputType m_input;
	public Image m_Img;
	public Vector3 m_InputDirection { set; get; }
	public float m_dragFactor=0.03f;

	private Vector3 fp;   //First touch position
	private Vector3 lp;   //Last touch position
	private Vector3 prev;	//Temp touch position
	private float dragDistance;  //minimum distance for a swipe to be registered

	[Header("Slingshot")]
	public Slider m_SlingshotSlider;
	public float m_MinLaunchForce;
	public float m_MaxLaunchForce; 
	[HideInInspector] public float m_CurrentLaunchForce;  

	public float m_ChargeSpeed;         
	private Image m_sliderBGImage;


	void Start(){
		m_InputDirection = Vector3.zero;
		dragDistance = Screen.height * m_dragFactor; 

		if (m_SlingshotSlider != null) {
			m_sliderBGImage = m_SlingshotSlider.gameObject.transform.GetChild (0).GetComponent<Image> ();
			m_sliderBGImage.enabled = false;
			m_SlingshotSlider.minValue = m_MinLaunchForce;
			m_SlingshotSlider.maxValue = m_MaxLaunchForce;
			m_CurrentLaunchForce = m_MinLaunchForce;
//			m_ChargeSpeed=0.05f;
		}
	}


	public virtual void OnPointerDown(PointerEventData ped){
		Vector2 pos = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle
			(m_Img.rectTransform,
				ped.position,
				ped.pressEventCamera,
				out pos)) 
		{
			fp = ped.position;
			prev = ped.position;
			lp = ped.position;
			OnDrag (ped);
		}
	}


	public virtual void OnDrag(PointerEventData ped){
		Vector2 pos = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle
			(m_Img.rectTransform,
				ped.position,
				ped.pressEventCamera,
				out pos)) 
		{
			prev = lp;
			lp = ped.position;
			switch (m_input) {
			case InputType.MOVE:
				MoveDrag ();
				break;
			case InputType.JUMP:
				break;
			case InputType.FIRE:
				FireDrag ();
				break;
			default:
				break;	
			}
		}
	}


	public virtual void OnPointerUp(PointerEventData ped){
		Vector2 pos = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle
			(m_Img.rectTransform,
				ped.position,
				ped.pressEventCamera,
				out pos)) 
		{
			lp = ped.position;
			switch (m_input) {
			case InputType.MOVE:
				MoveUp ();
				break;
			case InputType.JUMP:
				JumpUp ();
				break;
			case InputType.FIRE:
				FireUp ();
				break;
			default:
				break;	
			}
		}
	}


	public void MoveDrag(){
		if ((lp - fp).magnitude > dragDistance) {
			m_InputDirection = Vector3.zero + (lp - fp);
			m_InputDirection = m_InputDirection.normalized;
			if ((prev - fp).magnitude > (lp - fp).magnitude && prev != fp) {
				fp = lp;
			}
		}
	}


	public void MoveUp(){
		m_InputDirection = Vector3.zero;
	}


	public void JumpUp(){
		if ((lp - fp).magnitude <= dragDistance) {
			HUDController.m_Instance.JumpBtn ();
		}
	}


	public void FireDrag(){
		if (GameController.m_Instance.m_Player.GetComponent<PlayerController> ().m_stones > 0) 
		{
			if ((lp - fp).magnitude > dragDistance) {
				m_sliderBGImage.enabled = true;
				m_CurrentLaunchForce += (lp - fp).magnitude * m_ChargeSpeed;
				m_CurrentLaunchForce = Mathf.Clamp (m_CurrentLaunchForce, m_MinLaunchForce, m_MaxLaunchForce);
				m_SlingshotSlider.value = m_CurrentLaunchForce;
			}
		}		
	}


	public void FireUp(){
		if ((lp - fp).magnitude > dragDistance) {
			GameController.m_Instance.m_Player.GetComponent<PlayerController> ().Fire (m_CurrentLaunchForce);
			HUDController.m_Instance.UpdateStonesCount ();
			m_sliderBGImage.enabled = false;
			m_CurrentLaunchForce = m_MinLaunchForce;
			m_SlingshotSlider.value = m_CurrentLaunchForce;
		}
	}
}