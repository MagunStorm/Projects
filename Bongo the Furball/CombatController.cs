using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour {

	public GameObject m_attackColliderObj;


	void Start () {
		m_attackColliderObj.SetActive (false);
	}


	public void ToggleAttackCollider(){
		m_attackColliderObj.SetActive (!m_attackColliderObj.activeInHierarchy);
	}
		
}
