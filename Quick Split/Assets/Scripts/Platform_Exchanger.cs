using UnityEngine;
using System.Collections;

public class Platform_Exchanger : MonoBehaviour {

	// this script checks which platform the game is on, and activates the objects related to that platform and deactivates all others
	// this is a reusable script, so it contains platforms that the game may not be a part of. Also, this can/should be expanded to include all platforms

	public GameObject[] computerObjects;
	public GameObject[] mobileRegionObjects;
	public GameObject[] mobileFollowObjects;

	//debugging while in editor
	public bool isMobile;
	
	// Use this for initialization
	void OnEnable() {
		if (isMobile) {
			foreach(GameObject ob in computerObjects){
				ob.SetActive(false);
			}
			mobileControlSwap(true);
			return;
		}

		switch (Application.platform) {
		case RuntimePlatform.WindowsPlayer:
			foreach(GameObject ob in computerObjects){
				ob.SetActive(true);
			}
			mobileControlSwap(false);
			break;
		case RuntimePlatform.WebGLPlayer:
			foreach(GameObject ob in computerObjects){
				ob.SetActive(true);
			}
			mobileControlSwap(false);
			break;
		case RuntimePlatform.Android:
			foreach(GameObject ob in computerObjects){
				ob.SetActive(false);
			}
			mobileControlSwap(true);
			break;
		case RuntimePlatform.IPhonePlayer:
			foreach(GameObject ob in computerObjects){
				ob.SetActive(false);
			}
			mobileControlSwap(true);
			break;
		default:
			foreach(GameObject ob in computerObjects){
				ob.SetActive(true);
			}
			mobileControlSwap(false);
			break;
		}
	}

	void mobileControlSwap(bool isMobile){
		//deactivate everything first
		foreach(GameObject ob in mobileRegionObjects){
			ob.SetActive(false);
		}
		foreach(GameObject ob in mobileFollowObjects){
			ob.SetActive(false);
		}
		if(isMobile){
			switch(PlayerPrefs.GetString("Controls", "Follow")){
			case "Regions":
				foreach(GameObject ob in mobileRegionObjects){
					ob.SetActive(true);
				}
				break;
			case "Follow":
				foreach(GameObject ob in mobileFollowObjects){
					ob.SetActive(true);
				}
				break;
			default:
				foreach(GameObject ob in mobileRegionObjects){
					ob.SetActive(true);
				}
				break;
			}
		}
	}

}
