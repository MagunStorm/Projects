using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float m_gravityScale = 1.0f;
	public float m_speed = 150.0f;
	public int m_demage = 2;
	public float m_destroyTime = 10f;
	public AudioClip m_fireClip;
	public AudioClip m_destroyClip;
	public GameObject m_DestroyPrefab;

	private Rigidbody2D m_rigid;
	private AudioSource m_audio;

	void Awake(){
		m_rigid = GetComponent<Rigidbody2D> ();
		m_audio = GetComponent<AudioSource> ();
	}


	void Start(){
		m_rigid.gravityScale = m_gravityScale;
	}


	void OnEnable(){
		m_rigid.isKinematic = false;
		m_rigid.AddForce(transform.right*m_speed);
//		m_rigid.AddForce(transform.localScale*m_speed);
		if(m_fireClip!=null){
			m_audio.clip = m_fireClip;
			m_audio.Play ();
		}
		GetComponent<Collider2D> ().isTrigger = false;
		Invoke ("Destroy", m_destroyTime);
	}


	public void Destroy(){
		m_rigid.velocity = Vector2.zero;
		m_rigid.isKinematic = true;
		GetComponent<Collider2D> ().isTrigger = true;
//		GetComponent<Animator> ().Play ("Projectile_DESTROY");
		if(m_DestroyPrefab!=null){
			GameObject obj = Instantiate(m_DestroyPrefab,this.transform.position,Quaternion.identity);
			obj.SetActive (true);
		}
		if(m_destroyClip!=null){
			m_audio.clip = m_destroyClip;
			m_audio.Play ();
		}
//		StartCoroutine(ReturnToPool());
		gameObject.SetActive (false);
	}


	IEnumerator ReturnToPool(){
		yield return new WaitForSeconds (0.4f);
		gameObject.SetActive (false);
	}


	void OnDisable(){
		CancelInvoke ();
	}
		
}
