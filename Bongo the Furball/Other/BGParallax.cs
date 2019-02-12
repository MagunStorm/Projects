using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGParallax : MonoBehaviour {

	//The camera transform that this object's position is based on
	[SerializeField] Transform m_CameraTransform;

	//How much the transform will be moved by the movement of the camera.
	[SerializeField] Vector2 m_Influence = new Vector2(0.25f, 0.25f);

	//Store the original position to reset to and the last position of the camera
	Vector3 m_OriginalPosition;
	Vector3 m_OriginalCameraPosition;

	//How fast the object moves to match the camera
	const float LERP_SPEED_PRE_DELTA = 20f;

	/// Gets or sets the camera transform that the transform's position is based off of.
	public Transform cameraTransform
	{
		get {
			if (m_CameraTransform == null)
			{
				Camera cam = Camera.main;
				if (cam != null)
				{
					cameraTransform = cam.transform;
				}
				else
				{
					enabled = false;
					Destroy(this);
					#if UNITY_EDITOR
					Debug.LogError(NO_CAMERA);
					#endif
				}
			}
			return m_CameraTransform;
		}
		set {
			m_CameraTransform = value;
			m_OriginalCameraPosition = value.position;
			m_OriginalPosition = transform.position;
		}
	}
		
	/// <value>The influence.</value>
	public virtual Vector2 influence{
		get {	return m_Influence;}
		set {	m_Influence = value; }
	}


	public Vector3 parallaxPosition
	{
		get
		{
			#if GAMEEYE2D_XZ
			return m_OriginalPosition + Vector3.Scale((cameraTransform.position - m_OriginalCameraPosition), new Vector3(influence.x, 0f, influence.y));
			#else
			return m_OriginalPosition + Vector3.Scale((cameraTransform.position - m_OriginalCameraPosition), new Vector3(influence.x, influence.y, 0f));
			#endif
		}
	}


	void Awake ()
	{
		if (cameraTransform != null)
		{
			m_OriginalPosition = transform.position;
			m_OriginalCameraPosition = cameraTransform.position;
		}
	}

	//When this component enabled set its position to where the object would be based on the camera
	void OnEnable()
	{
		if (cameraTransform != null)
		{
			m_OriginalPosition = transform.position;
			m_OriginalCameraPosition = Camera.main.transform.position;
			transform.position = parallaxPosition;
		}
		else
		{
			enabled = false;
		}
	}


	//On each frame if the camera has moved, move the camera according to how influenced it is by the camera's position.
	void Update ()
	{			
		transform.position = Vector3.Lerp(  transform.position,
			parallaxPosition,
			Mathf.Min(LERP_SPEED_PRE_DELTA * Time.deltaTime, 1f));

	}


	#if UNITY_EDITOR

	//Whether to draw debug data or not
	[SerializeField] bool m_DrawDebug = false;

	//Error messages
	const string NO_CAMERA = "Parralaxing error. No camera in the scene.";

	//In the scene window draw the line between the original position and the current position
	void OnDrawGizmos ()
	{
		if (m_DrawDebug && Application.isPlaying)
		{
			Gizmos.color = Color.grey;
			Gizmos.DrawLine(m_OriginalPosition, transform.position);
		}
	}

	/// <summary>
	/// When first put into the editor or the reset button is hit, the tool sets the camera to the main camera.
	/// Only works in the editor.
	/// </summary>
	public void Reset ()
	{
		Camera mainCamera = Camera.main;
		if (mainCamera != null)
		{
			cameraTransform = mainCamera.transform;
		}
	}

	#endif
}
