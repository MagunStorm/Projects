using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJumpButton : MonoBehaviour,IPointerUpHandler,IPointerDownHandler {

	private Image m_bgImg;


	void Start(){
		m_bgImg = GetComponent<Image> ();
	}


	public virtual void OnPointerDown(PointerEventData ped){
		Vector2 pos = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle
			(m_bgImg.rectTransform,
				ped.position,
				ped.pressEventCamera,
				out pos)) {
			HUDController.m_Instance.m_UIElementPressedCount++;
			HUDController.m_Instance.JumpBtn ();
			m_bgImg.color = Color.gray;
		}
	}


	public virtual void OnPointerUp(PointerEventData ped){
		HUDController.m_Instance.m_UIElementPressedCount--;
		m_bgImg.color = Color.white;
	}
}
