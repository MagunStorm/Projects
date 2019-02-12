using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToPlatform : MonoBehaviour {

	public string[] m_CollisionTags;

	void OnCollisionEnter2D(Collision2D other){
		foreach (string s in m_CollisionTags) {
			if (other.gameObject.CompareTag(s)) {
				other.transform.parent = this.transform;
			}
		}
	}
		

	void OnCollisionExit2D(Collision2D other){
		foreach (string s in m_CollisionTags) {
			if (other.gameObject.CompareTag(s)) {
				other.transform.parent = null;
			}
		}
	}


}

