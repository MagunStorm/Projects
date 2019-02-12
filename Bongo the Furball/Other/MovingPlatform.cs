using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public GameObject m_platform;
	public float m_speed = 2f;
	public Transform[] m_waypoints;
	public int m_waypointSelection;

	private Transform m_currentWaypoint;

	void Start(){
		m_currentWaypoint = m_waypoints[m_waypointSelection];
	}


	void Update(){
		m_platform.transform.position = Vector3.MoveTowards (m_platform.transform.position, m_currentWaypoint.position, m_speed * Time.deltaTime);
		if (m_platform.transform.position == m_currentWaypoint.position) {
			m_waypointSelection++;
			if (m_waypointSelection == m_waypoints.Length) {
				m_waypointSelection = 0;
			}
			m_currentWaypoint = m_waypoints [m_waypointSelection];
		}
	}

}
