using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Region_Toggler : MonoBehaviour {

	//this handles the Region transparency toggle on the options menu
	
	Toggle toggle;
	
	// Use this for initialization
	void Awake () {
		toggle = GetComponent<Toggle> ();
		int guideOnInt = PlayerPrefs.GetInt ("Region Guide", 1);
		if (guideOnInt == 1) {
			toggle.isOn = true;
		}
		else{
			toggle.isOn = false;
		}
	}
	
	public void OnToggle(){
		if (!toggle.isOn) {
			PlayerPrefs.SetInt("Region Guide", 0);
		}
		else{
			PlayerPrefs.SetInt("Region Guide", 1);
		}
	}

}
