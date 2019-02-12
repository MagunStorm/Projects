using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public ObjectPooler m_ProjectilePool;
//	public string m_PoolTag;
	public Transform m_firePoint;
	public bool m_isAutoFire=false;
	public float m_fireRate = 0.5f;

	[HideInInspector] public float m_fireDelay;


	void Start(){
		m_fireDelay = 0f;

		if (m_isAutoFire) {
			StartCoroutine(AutoFire());
		}
	}


	public void Fire(){
		m_ProjectilePool.SpawnAtPoint(m_firePoint);
	}


	public void FireWithForce(float force){
		m_ProjectilePool.SpawnAtPoint(m_firePoint, force);
	}


	IEnumerator AutoFire(){
		while (true) {
			if (this.gameObject.CompareTag ("Enemy")) {
				GetComponent<Animator> ().Play ("Enemy_FIRE");
			} else {
				Fire ();
			}
			yield return new WaitForSeconds (m_fireRate);
		}
	}


//	void OnDisable(){
//		StopCoroutine (AutoFire ());
//	}
		
}

