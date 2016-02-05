using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Control_Selector : MonoBehaviour {

	public Text controlText;

	// Use this for initialization
	void Start () {
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
