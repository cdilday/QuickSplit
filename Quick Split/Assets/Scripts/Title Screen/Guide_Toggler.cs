﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Guide_Toggler : MonoBehaviour {

	Toggle toggle;

	// Use this for initialization
	void Awake () {
		toggle = GetComponent<Toggle> ();
		int guideOnInt = PlayerPrefs.GetInt ("Guide", 1);
		if (guideOnInt == 1) {
			toggle.isOn = true;
		}
		else
		{
			toggle.isOn = false;
		}
	}
	
	public void OnToggle(){
		if (!toggle.isOn) {
			PlayerPrefs.SetInt("Guide", 0);
		}
		else{
			PlayerPrefs.SetInt("Guide", 1);
		}
	}
}