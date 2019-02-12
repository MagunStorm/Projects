using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {

	public string key;


	private IEnumerator Start () 
	{
		while (!LocalizationManager.instance.GetIsReady ()) {
			yield return null;
		}

		LocalizeText ();
	}


	public void LocalizeText(){
		Text text = GetComponent<Text> ();
		text.text = LocalizationManager.instance.GetLocalizedValue (key);
	}

}