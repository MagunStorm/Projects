using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;


public class GameManager : MonoBehaviour {

	public static GameManager m_Instance;
	public string m_SceneListJson = "SceneInfo.json";

	[HideInInspector] public int m_stars = 0;
	[HideInInspector] public List<SceneInfo> m_scenes;
	[HideInInspector] public string m_selectedWorldPrefix;
	[HideInInspector] public Material m_selectedWorldSkybox;
	[HideInInspector] public AudioClip m_selectedWorldClip;
	[HideInInspector] public string m_sceneToLoad = "";

	[Header("DEBUG")]
	public bool m_clearPlayerPrefs = false;
	public bool m_saveOnStart = false;


	void Awake(){
		if (m_Instance == null) {
			m_Instance = this;
		} else {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);

//		--------- DEBUG -----------
		if (m_clearPlayerPrefs) {
			PlayerPrefs.DeleteAll ();
		}
		if (m_saveOnStart) {
			Save ();	
		}
//		---------------------------

//		Debug.Log (Application.persistentDataPath);
		if (PlayerPrefs.HasKey ("Stars")) {
			m_stars = PlayerPrefs.GetInt ("Stars");
		} else {
			Save ();
		}
	}


	void Start(){
		LoadSceneInfo (m_SceneListJson);
	}


	public void Save(){
		PlayerPrefs.SetInt ("Stars", m_stars);
	}


	public void LoadSceneInfo(string fileName){
		m_scenes = new List<SceneInfo> ();

		#if UNITY_EDITOR
		string filePath = Path.Combine (Application.streamingAssetsPath, fileName);
		#elif UNITY_ANDROID
		string filePath = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
		#endif

//		WWW www = new WWW(filePath);
		UnityWebRequest www = UnityWebRequest.Get (filePath);
		www.SendWebRequest ();

		if (string.IsNullOrEmpty (www.error)){
			while (!www.isDone) {
				//				Debug.Log ("Ładowanie pliku...");
			}

//			string dataAsJason = www.text;
			string dataAsJason = www.downloadHandler.text;
			SceneList loadedData = JsonUtility.FromJson<SceneList> (dataAsJason);
			for (int i = 0; i < loadedData.items.Length; i++) {
				m_scenes.Add (loadedData.items [i]);
			}
		} else {
			Debug.LogError ("Cannot find file!");
		}
	}
		
}



public class PuzzleData{

	public PuzzleData(string levelName){
		string data = PlayerPrefs.GetString (levelName);
		if (data == "")
			return;

		string[] allData = data.Split ('&');	//podział stringu
		BestScore = int.Parse(allData[0]);
		SilverScore = int.Parse (allData [1]);
		GoldScore = int.Parse (allData [2]);
		stars = int.Parse (allData [3]);
	}

	public int BestScore { set; get;}
	public int SilverScore { set; get;}
	public int GoldScore { set; get;}
	public int stars { set; get; }
}


[System.Serializable]
public class SceneList{
	public SceneInfo[] items;
}


[System.Serializable]
public class SceneInfo 
{
	public string name;
	public int stars;
}

