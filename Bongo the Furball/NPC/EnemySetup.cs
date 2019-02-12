using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySetup : MonoBehaviour {

	[Header ("ICON")]
	public Sprite m_icon;

	[Header("HEALTH")]
	public bool m_invincible = false;
	public int m_maxHealth = 2;
	[HideInInspector] public int m_currentHealth;

	[Header("COLLISION")]
	public int m_demage = -1;

	[Header("MOVEMENT")]
	public float m_speed = 1f;
	public float m_idleTime = 2f;


	void Awake () {
		m_currentHealth = m_maxHealth;
	}

}
