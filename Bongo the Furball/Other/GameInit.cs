using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInit : MonoBehaviour {

	void Start () {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;

		SceneManager.LoadScene ("Loading");
	}
		
}
