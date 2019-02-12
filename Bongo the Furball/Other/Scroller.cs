using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour {

	public float scrollSpeed;
	public float tileSize;
	public Vector3 m_directionVector = Vector3.down;

	private Vector3 startPosition;

	void Start () {
		startPosition = transform.position;
	}


	void Update () {
		float newPosition = Mathf.Repeat (Time.time * scrollSpeed, tileSize);
		Vector3 pos = new Vector3 (transform.position.x, startPosition.y, transform.position.z);
		transform.position = pos + m_directionVector * newPosition;
	}
}
