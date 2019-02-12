using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;


public class GameDontDestroy : MonoBehaviour {

	public static GameDontDestroy m_Instance;

	[HideInInspector] 
	public int m_SceneNo;
	[HideInInspector] 
	public Vector3 m_SpawnPoint;
	[HideInInspector] 
	public int m_health;
	[HideInInspector] 
	public int m_jumps;
	[HideInInspector] 
	public int m_oxy;
	[HideInInspector] 
	public bool m_canfire;
	[HideInInspector] 
	public Color m_playerColor;
	[HideInInspector] 
	public int m_stones;

	public bool m_initDeafaultPlayer = false;
	public float m_cameraSize;

	private string m_savePath;
	[HideInInspector] public bool m_hideJoystick;
	[HideInInspector] public bool m_hideButtons;


	void Awake(){
		if (m_Instance == null) {
			m_Instance = this;
		} else {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}


	void Start(){
		if (m_initDeafaultPlayer) {
			m_SceneNo = SceneManager.GetActiveScene ().buildIndex;
			m_SpawnPoint = Vector3.zero;
			m_health = 5;
			m_jumps = 1;
			m_oxy = 10;
			m_canfire = false;
			m_playerColor = Color.blue;
			m_stones = 0;
		}

		int i = PlayerPrefs.HasKey ("JOYSTICK") ? PlayerPrefs.GetInt("JOYSTICK") : 0;
		m_hideJoystick = i == 1 ? true : false;
		i = PlayerPrefs.HasKey ("BUTTONS") ? PlayerPrefs.GetInt("BUTTONS") : 0;
		m_hideButtons = i == 1 ? true : false;

//		m_savePath = Application.dataPath + "/SaveData";
		m_savePath = Application.persistentDataPath;
		if (!Directory.Exists (m_savePath)) {
			Directory.CreateDirectory (m_savePath);
		} 
	}


	public void Save(string name, int sceneno, Vector3 spawnpoint){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (m_savePath + "/" + name);
		SaveData data = new SaveData ();
		data.sceneNo = sceneno;
		data.XspawnPoint = spawnpoint.x;
		data.YspawnPoint = spawnpoint.y;
		GameObject obj = GameObject.FindGameObjectWithTag ("Player");
		if (obj != null) {
			PlayerSetup ps = obj.GetComponent<PlayerSetup> ();
			PlayerController pc = obj.GetComponent<PlayerController> ();
			data.maxHealth = ps.m_maxHealth;
			data.maxJump = ps.m_MaxJumps;
			data.maxOxy = ps.m_maxOxygen;
			data.canFire = pc.m_canFire;
			data.stones = pc.m_stones;
			data.Rcolor = ps.m_FurryColor.r;
			data.Gcolor = ps.m_FurryColor.g;
			data.Bcolor = ps.m_FurryColor.b;
		} else //zapisz wartości domyślne
		{
			data.maxHealth = 5;
			data.maxJump = 1;
			data.maxOxy = 10;
			data.stones = 0;
			data.canFire = false;
			data.Rcolor = m_playerColor.r;
			data.Gcolor = m_playerColor.g;
			data.Bcolor = m_playerColor.b;
		}
		bf.Serialize (file, data);
		file.Close ();

		m_SceneNo = sceneno;
		m_SpawnPoint = spawnpoint;
		m_health = data.maxHealth;
		m_jumps = data.maxJump;
		m_oxy = data.maxOxy;
		m_canfire = data.canFire;
		m_stones = data.stones;
	}


	public void LoadLast(){
		DateTime startlevelDTTM = File.GetLastWriteTime (m_savePath + "/StartLevel.dat");
		DateTime checkpointDTTM = File.GetLastWriteTime (m_savePath + "/Checkpoint.dat");
		string fname = startlevelDTTM < checkpointDTTM ? "Checkpoint.dat" : "StartLevel.dat";
		Load (fname);
	}


	public bool Load(string fname){
		if (File.Exists (m_savePath + "/" + fname)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (m_savePath + "/" + fname, FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize (file);
			file.Close ();

			m_SceneNo = data.sceneNo;
			m_SpawnPoint = new Vector3 (data.XspawnPoint, data.YspawnPoint, 0);
			m_health = data.maxHealth;
			m_oxy = data.maxOxy;
			m_jumps = data.maxJump;
			m_canfire = data.canFire;
			m_stones = data.stones;
			m_playerColor = new Color(data.Rcolor,data.Gcolor,data.Bcolor);
			return true;
		} 
		return false;
	}
		

	public void DeleteFile(string fname){
		if (File.Exists (m_savePath + "/" + fname)) {
			File.Delete (m_savePath + "/" + fname);
//			Debug.Log ("Deleteed " + fname + ": " + File.Exists (m_savePath + "/" + fname));
		}
	}

}


[Serializable]
class SaveData{
	public int sceneNo;
	public float XspawnPoint;
	public float YspawnPoint;
	public int maxHealth;
	public int maxJump;
	public int maxOxy;
	public int stones;
	public bool canFire;
	public float Rcolor;
	public float Gcolor;
	public float Bcolor;
}
