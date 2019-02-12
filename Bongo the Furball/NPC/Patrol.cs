using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {

	private Rigidbody2D m_rb2D;
	private Animator m_anim;
	private EnemySetup m_eSetup;
	private bool m_stop = false;

	private float m_waitTime;
	private float m_direction;

	void Start(){
		m_rb2D = GetComponent<Rigidbody2D> ();
		m_anim = GetComponent<Animator> ();
		m_eSetup = GetComponent<EnemySetup> ();
		m_direction = transform.localScale.x;
	}


	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Waypoint")){
			StopPatrol();
			ResumePatrol (m_eSetup.m_idleTime);
//			Vector3 theScale = transform.localScale;
//			theScale.x *= -1;
//			transform.localScale = theScale;
			m_direction*=-1;
			transform.Rotate (0f, 180f, 0f);
		}

		if (other.CompareTag ("FireHere")) {
			m_anim.Play ("Enemy_FIRE");
		}
	}


	void FixedUpdate (){
		if (m_stop) {
			m_rb2D.velocity = Vector2.zero;
		} else {
//			m_rb2D.velocity = new Vector2 (m_eSetup.m_speed * transform.localScale.x, m_rb2D.velocity.y);
			m_rb2D.velocity = m_eSetup.m_speed * Vector2.right*m_direction;
		}
		m_anim.SetFloat ("speed", Mathf.Abs(m_rb2D.velocity.x));
	}
		

	IEnumerator Wait(){
		yield return new WaitForSeconds (m_waitTime);
		m_stop = false;
		m_rb2D.isKinematic = false;
	}


	public void StopPatrol(){
		m_stop = true;
		m_rb2D.isKinematic = true;
	}


	public void ResumePatrol(float time){
		m_waitTime = time;
		StartCoroutine(Wait());		//zaczekaj przed zmianą kierunku
	}
}
