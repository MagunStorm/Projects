using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

//[RequireComponent(typeof(Button))]
public class AdManager : MonoBehaviour{
	
	//---------- ONLY NECESSARY FOR ASSET PACKAGE INTEGRATION: ----------//
	#if UNITY_IOS
	private string gameId = "2886668";
	#elif UNITY_ANDROID
//	private string gameId = "2886669";
	private string gameId = "2842254";
	#endif
	//-------------------------------------------------------------------//

	public Button m_Button;
	public string placementId = "video";

	void Start (){    
		if (m_Button) 
			m_Button.onClick.AddListener(ShowAd);

		//---------- ONLY NECESSARY FOR ASSET PACKAGE INTEGRATION: ----------//
		if (Advertisement.isSupported) {
			Advertisement.Initialize (gameId, true);
		}
		//-------------------------------------------------------------------//
	}


	void Update (){
		if (m_Button) 
			m_Button.interactable = Advertisement.IsReady(placementId);
	}


	void ShowAd (){
		ShowOptions options = new ShowOptions();
		options.resultCallback = HandleShowResult;

		Advertisement.Show(placementId, options);
	}


	void HandleShowResult (ShowResult result)
	{
		if(result == ShowResult.Finished) {
			Debug.Log("Video completed - Offer a reward to the player");
			GameManager.m_Instance.m_stars += 2;
			GameManager.m_Instance.Save ();
			EventManager.TriggerEvent("Victory");
		}else if(result == ShowResult.Skipped) {
			Debug.LogWarning("Video was skipped - Do NOT reward the player");

		}else if(result == ShowResult.Failed) {
			Debug.LogError("Video failed to show");
		}
	}
}
