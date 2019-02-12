using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionController : MonoBehaviour {

//	public bool m_CollisionByTag = true;
	public string[] m_CollisionTags;
	public bool m_destroyOnContact = false;
	public float m_impactForce;
	[Header("ENTER Action")]
	public UnityEvent m_Action;
	[Header("AUDIO")]
	public AudioClip m_EnterClip;
	public AudioClip m_ExitClip;
//	public AudioClip m_StayClip;

	private Collision2D m_collider;


	void OnCollisionEnter2D(Collision2D other){
		if (!CheckTag (other) || other.relativeVelocity.magnitude < m_impactForce)
			return;

		m_collider = other;

		SFXController.SetAudioLoop (false);
		SFXController.SetAudio(m_EnterClip);

		if (m_Action != null) {
			m_Action.Invoke ();
		}

		if (m_destroyOnContact) {
			Destroy (gameObject);
		}
	}


	void OnCollisionExit2D(Collision2D other){
		SFXController.SetAudioLoop (false);
		SFXController.SetAudio (m_ExitClip);
	}


	bool CheckTag(Collision2D col){
		bool result = false;

		if (m_CollisionTags.Length == 0) {
			result = true;
		}
		if (m_CollisionTags.Length>0) {
			foreach (string s in m_CollisionTags) {
				if (col.gameObject.CompareTag (s)) {
					result = true;
				}
			}
		}
		return result;
	}


	/////////////////////////////////////////////////
	//	Poniżej różne funkcje wywoływane w kolizji
	/// ////////////////////////////////////////////


	public void ProjectileImpact(){
		if (m_collider.gameObject.CompareTag ("Enemy")) {
			EnemyHealth eH = m_collider.gameObject.GetComponent<EnemyHealth> ();
			int demage = this.GetComponent<Projectile> ().m_demage;
			eH.TakeDemage (demage);
		}

		if (m_collider.gameObject.CompareTag ("Player")) {
			m_collider.gameObject.GetComponent<PlayerHealth> ().ModifyHealth (-this.GetComponent<Projectile> ().m_demage);
		}

		this.GetComponent<Projectile> ().Destroy ();
	}
		

	public void PlayerEnemyCollision(){
		bool onTop = Physics2D.OverlapCapsule (new Vector2 (this.GetComponent<PlayerController> ().m_GroundPoint.position.x, this.GetComponent<PlayerController> ().m_GroundPoint.position.y + 0.15f),
			         new Vector2 (1.5f, 0.4f),
			         CapsuleDirection2D.Horizontal,
			         0f,
			         GameController.m_Instance.m_EnemyLayer);

		if (onTop && !m_collider.gameObject.GetComponent<EnemySetup>().m_invincible) {
			m_collider.gameObject.GetComponent<EnemyHealth> ().TakeDemage (1);
		} else {
//			if (m_collider.gameObject.GetComponent<Patrol> () != null) {
//				m_collider.gameObject.GetComponent<Patrol> ().StopPatrol ();
//				m_collider.gameObject.GetComponent<Patrol> ().ResumePatrol (0.5f);
//			}
			this.GetComponent<PlayerHealth> ().ModifyHealth (m_collider.gameObject.GetComponent<EnemySetup> ().m_demage);
			bool collisionSide = m_collider.gameObject.transform.position.x < gameObject.transform.position.x ? true : false;
			this.GetComponent<PlayerController> ().BounceOff (collisionSide);
		}
	}


	public void DemageEnemy(){
		Debug.Log ("SoftSpot");
		m_collider.gameObject.GetComponent<EnemyHealth> ().TakeDemage (1);
	}


	public void Explode(GameObject prefab){
		GameObject obj = Instantiate(prefab,this.transform.position,Quaternion.identity);
		obj.SetActive (true);
	}


	public void ImpactSound(AudioClip clip){
		SFXController.SetAudio (clip);
	}


	public void DemagePlayer(int demage){
		m_collider.gameObject.GetComponent<PlayerHealth> ().ModifyHealth (demage);
		bool collisionSide = m_collider.gameObject.transform.position.x < gameObject.transform.position.x ? true : false;
		m_collider.gameObject.GetComponent<PlayerController> ().BounceOff (collisionSide);
	}


}
