using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour {

	public static LocalizationManager instance;

//	public bool m_loadDefaultLanguage = false;
	public string m_DefaultLanguage = "localizedText_en.json";
	public string m_defaultLanguageCode = "EN";

	[HideInInspector]public string m_language = "";
	[HideInInspector]public string m_langFile = "";

	private Dictionary<string, string> localizedText;
	private bool isReady = false;
	private string missingTextString = "Localized text not found";

	// Use this for initialization
	void Awake () 
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this)
		{
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}
		

	void Start(){
		LoadLanguage();
	}


	public void LoadLocalizedText(string fileName)
	{
		localizedText = new Dictionary<string, string> ();

		#if UNITY_EDITOR
			string filePath = Path.Combine (Application.streamingAssetsPath, fileName);
		#elif UNITY_ANDROID
			string filePath = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
		#endif

		WWW www = new WWW(filePath);
		if (string.IsNullOrEmpty (www.error)){
			while (!www.isDone) {
//				Debug.Log ("Ładowanie pliku...");
			}
//			string dataAsJson = File.ReadAllText (filePath);
			m_langFile = fileName;
			string dataAsJson = www.text;
			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData> (dataAsJson);
			for (int i = 0; i < loadedData.items.Length; i++) {
				localizedText.Add (loadedData.items [i].key, loadedData.items [i].value);   
			}
		} else {
			Debug.LogError ("Cannot find file!");
		}
//		isReady = true;
	}


	public string GetLocalizedValue(string key)
	{
		string result = missingTextString;
		if (localizedText.ContainsKey (key)) {
			result = localizedText [key];
		}

		return result;

	}


	public bool GetIsReady(){
		return isReady;
	}


	public void SetLanguage(string lang){
		m_language = lang;
		SaveLanguage ();
		isReady = true;
	}


	public void SaveLanguage(){
		string saveLang = m_language + "#" + m_langFile;
		PlayerPrefs.SetString ("Language", saveLang);
	}


	public void LoadLanguage(){
		string file = m_DefaultLanguage;
		string code = m_defaultLanguageCode;
		if (PlayerPrefs.HasKey ("Language")) {
			string data = PlayerPrefs.GetString ("Language");
			if (data == "")
				return;

			string[] allData = data.Split ('#');	//podział stringu
			code = allData[0];
			file = allData [1];
		}
		LoadLocalizedText (file);
		SetLanguage (code);
	}
		
}