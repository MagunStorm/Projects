using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMultiWaypoint : MonoBehaviour {

	public Transform[] m_waypoints;
	public float m_speed = 2f;
	public float m_waitTime = 1f;
	public float m_minDistanceFromWaypoint = 0.15f;
	public int m_waypointSelection;
	public bool m_facingRight=true;
	public bool m_randomMove = false;

	private Transform m_currentWaypoint;
	private Rigidbody2D m_rb2D;
	private Vector3 m_direction;
	private bool m_stop=false;
	private Animator m_anim;


	void Start(){
		m_rb2D = GetComponent<Rigidbody2D> ();
		m_anim = GetComponent<Animator> ();

		m_currentWaypoint = m_waypoints[m_waypointSelection];

		m_direction = m_currentWaypoint.position - this.transform.position;
		m_direction.Normalize ();
	}


	void FixedUpdate () {
		if (m_stop) {
			m_rb2D.velocity = Vector2.zero;
		} else {
			m_rb2D.velocity = m_speed * (Vector2)m_direction;

			if (Vector2.Distance (m_rb2D.position, (Vector2)m_currentWaypoint.position) < m_minDistanceFromWaypoint) {
				if (m_randomMove) {
					m_waypointSelection = Random.Range (0, m_waypoints.Length);
				} else {
					m_waypointSelection++;
					if (m_waypointSelection == m_waypoints.Length) {
						m_waypointSelection = 0;
					}
				}

				if (m_currentWaypoint.position.x > m_waypoints [m_waypointSelection].position.x && m_facingRight) {
					Flip ();
				}
				if (m_currentWaypoint.position.x < m_waypoints [m_waypointSelection].position.x && !m_facingRight) {
					Flip ();
				}
				
				m_currentWaypoint = m_waypoints [m_waypointSelection];
				m_direction = m_currentWaypoint.position - this.transform.position;
				m_direction.Normalize ();
			}
		}

		m_anim.SetFloat ("speed", Mathf.Abs(m_rb2D.velocity.x));
	}


	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("PAUSE")){
			StartCoroutine(Wait(m_waitTime));	
		}

		if (other.CompareTag ("ATTACK")) {
			m_anim.Play ("ATTACK");
		}

		if (other.CompareTag ("FIRE")) {
			m_anim.Play ("Enemy_FIRE");
		}
	}


	IEnumerator Wait(float time){
		m_stop = true;
		m_rb2D.isKinematic = true;
		yield return new WaitForSeconds (time);
		m_stop = false;
		m_rb2D.isKinematic = false;
	}


	public void StopPatrol(){
		m_stop = true;
		m_rb2D.isKinematic = true;
	}


	public void ResumePatrol(float time){
		StartCoroutine(Wait(time));		//zaczekaj przed zmianą kierunku
	}


	void Flip(){
		transform.Rotate (0f, 180f, 0f);
		m_facingRight = !m_facingRight;
	}

}
