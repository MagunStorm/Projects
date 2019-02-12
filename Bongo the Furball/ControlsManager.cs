using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour {

	public Toggle m_joystickToggle;
	public Toggle m_jumpShootToggle;


	void OnEnable(){
		m_joystickToggle.isOn = GameDontDestroy.m_Instance.m_hideJoystick;
		m_jumpShootToggle.isOn = GameDontDestroy.m_Instance.m_hideButtons;
	}


	public void ToggleMoveUI(){
		HUDController.m_Instance.m_joystickPanel.SetActive (!m_joystickToggle.isOn);
		HUDController.m_Instance.m_TouchMovePanel.SetActive (m_joystickToggle.isOn);
		GameDontDestroy.m_Instance.m_hideJoystick = m_joystickToggle.isOn;
	}


	public void ToggleJumpShootUI(){
		HUDController.m_Instance.m_JumpShootPanel.SetActive (!m_jumpShootToggle.isOn);
		HUDController.m_Instance.m_TouchJumpFirePanel.SetActive (m_jumpShootToggle.isOn);
		GameDontDestroy.m_Instance.m_hideButtons = m_jumpShootToggle.isOn;
	}
}
