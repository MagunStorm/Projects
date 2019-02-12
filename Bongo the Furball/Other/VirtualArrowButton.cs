using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualArrowButton : MonoBehaviour,IPointerUpHandler,IPointerDownHandler {

	public enum Action {LEFT,RIGHT,UP,DOWN, JUMP, FIRE};
	public Action m_action;

	private Image m_bgImg;
	private Image m_arrowImg;
	private Vector3 m_DirectionVect;


	void Start(){
		m_bgImg = GetComponent<Image> ();
		m_arrowImg = GetComponentInChildren<Image> ();

		switch (m_action) {
		case Action.LEFT:
				m_DirectionVect = new Vector3 (-1, 0, 0);
				break;
		case Action.RIGHT:
				m_DirectionVect = new Vector3 (1, 0, 0);
				break;
		case Action.UP:
				m_DirectionVect = new Vector3 (0, 1, 0);
				break;
		case Action.DOWN:
				m_DirectionVect = new Vector3 (0, -1, 0);
				break;
		default:
				m_DirectionVect = Vector3.zero;
				break;
		}
	}


	public virtual void OnPointerDown(PointerEventData ped){
		HUDController.m_Instance.m_UIElementPressedCount++;
		Vector2 pos = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle
			(m_bgImg.rectTransform,
				ped.position,
				ped.pressEventCamera,
				out pos)) 
		{
			if (m_action == Action.LEFT || m_action == Action.RIGHT) {
				GetComponentInParent<VirtualArrowContainer> ().m_InputX = m_DirectionVect;
			} else {
				GetComponentInParent<VirtualArrowContainer> ().m_InputY = m_DirectionVect;
			}
			GetComponentInParent<VirtualArrowContainer> ().UpdateInputDirection ();
		}else{
			OnPointerUp (ped);
		}

		m_bgImg.color = Color.gray;
		m_arrowImg.color = Color.gray;
	}
		

	public virtual void OnPointerUp(PointerEventData ped){
		if (m_action == Action.LEFT || m_action == Action.RIGHT) {
			GetComponentInParent<VirtualArrowContainer> ().m_InputX = Vector3.zero;
		} else {
			GetComponentInParent<VirtualArrowContainer> ().m_InputY = Vector3.zero;
		}
		GetComponentInParent<VirtualArrowContainer> ().UpdateInputDirection();
		HUDController.m_Instance.m_UIElementPressedCount--;
		m_bgImg.color = Color.white;
		m_arrowImg.color = Color.white;
	}
		
}
