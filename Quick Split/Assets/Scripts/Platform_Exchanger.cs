using UnityEngine;
using System.Collections;

public class Platform_Exchanger : MonoBehaviour {

	// this script checks which platform the game is on, and activates the objects related to that platform and deactivates all others
	// this is a reusable script, so it contains platforms that the game may not be a part of. Also, this can/should be expanded to include all platforms

	public GameObject[] computerObjects;
	public GameObject[] mobileObjects;

	//debugging while in editor
	public bool isMobile;
	
	// Use this for initialization
	void OnEnable() {
		if (isMobile) {
			foreach(GameObject ob in computerObjects){
				ob.SetActive(false);
			}
			foreach(GameObject ob in mobileObjects){
				ob.SetActive(true);
			}
			return;
		}

		switch (Application.platform) {
		case RuntimePlatform.WindowsPlayer:
			foreach(GameObject ob in computerObjects){
				ob.SetActive(true);
			}
			foreach(GameObject ob in mobileObjects){
				ob.SetActive(false);
			}
			break;
		case RuntimePlatform.WebGLPlayer:
			foreach(GameObject ob in computerObjects){
				ob.SetActive(true);
			}
			foreach(GameObject ob in mobileObjects){
				ob.SetActive(false);
			}
			break;
		case RuntimePlatform.Android:
			foreach(GameObject ob in computerObjects){
				ob.SetActive(false);
			}
			foreach(GameObject ob in mobileObjects){
				ob.SetActive(true);
			}
			break;
		case RuntimePlatform.IPhonePlayer:
			foreach(GameObject ob in computerObjects){
				ob.SetActive(false);
			}
			foreach(GameObject ob in mobileObjects){
				ob.SetActive(true);
			}
			break;
		default:
			foreach(GameObject ob in computerObjects){
				ob.SetActive(true);
			}
			foreach(GameObject ob in mobileObjects){
				ob.SetActive(false);
			}
			break;
		}
	}

}
