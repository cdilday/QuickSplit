using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Control_Selector : MonoBehaviour {

	public Text controlText;
	public GameObject[] controlObjects;

	// Use this for initialization
	void Start (){
		//WebGL/standalone don't have complicated or conflicting control schemes, and therefore have no reason for changing
		if (!Application.isMobilePlatform) {
			foreach(GameObject control in controlObjects)
				Destroy(control);
			PlayerPrefs.SetString ("Controls", "Follow");
			Destroy(gameObject);
		}
		else
			controlText.text = PlayerPrefs.GetString ("Controls", "Regions");
	}
	
	public void hit_button(){
		if (controlText.text == "Regions") {
			PlayerPrefs.SetString ("Controls", "Follow");
			controlText.text = "Follow";
		}else{
			PlayerPrefs.SetString ("Controls", "Regions");
			controlText.text = "Regions";
		}
	}
}
