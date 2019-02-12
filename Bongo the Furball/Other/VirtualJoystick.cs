using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler,IPointerUpHandler,IPointerDownHandler {

	public Vector3 m_InputDirection { set; get; }

	private Image m_bgImg;
	private Image m_joystickImg;


	void Start(){
		m_bgImg = GetComponent<Image> ();
		m_joystickImg = transform.GetChild(0).GetComponentInChildren<Image> ();
		m_InputDirection = Vector3.zero;
	}


	public virtual void OnDrag(PointerEventData ped){
		Vector2 pos = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle
			(m_bgImg.rectTransform,
			   ped.position,
			   ped.pressEventCamera,
			   out pos)) 
		{
			pos.x = (pos.x / m_bgImg.rectTransform.sizeDelta.x);
			pos.y = (pos.y / m_bgImg.rectTransform.sizeDelta.y);
//			pos.x = (ped.position.x / m_bgImg.rectTransform.sizeDelta.x);
//			pos.y = (ped.position.y / m_bgImg.rectTransform.sizeDelta.y);

			float x = (m_bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
			float y = (m_bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

			m_InputDirection = new Vector3 (x, 0, y);
			m_InputDirection = (m_InputDirection.magnitude > 1) ? m_InputDirection.normalized : m_InputDirection;

			m_joystickImg.rectTransform.anchoredPosition = 
				new Vector3 (m_InputDirection.x * (m_bgImg.rectTransform.sizeDelta.x / 3),
				m_InputDirection.z * (m_bgImg.rectTransform.sizeDelta.y / 3));
		}

	}


	public virtual void OnPointerDown(PointerEventData ped){
		HUDController.m_Instance.m_UIElementPressedCount++;
		OnDrag (ped);
	}


	public virtual void OnPointerUp(PointerEventData ped){
		m_InputDirection = Vector3.zero;
		m_joystickImg.rectTransform.anchoredPosition = Vector3.zero;
		HUDController.m_Instance.m_UIElementPressedCount--;
	}
}
