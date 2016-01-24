using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GP_Icon_Handler : MonoBehaviour {

	// This script will change the color of the google Play Button depending one whether or not the player is connected
	// if it isn't mobile, then it deletes the button

	public string type;

	bool GPActive = false;
	GPG_Handler gpgh;
	Button_Image_Swapper bts;

	Achievement_Script achievementHandler;

	// Use this for initialization
	void Start () {
		GameObject GPGHObject = GameObject.FindGameObjectWithTag ("Google Play");
		if (GPGHObject == null) {
			Destroy (gameObject);
			return;
		}
		else{
			gpgh = GPGHObject.GetComponent<GPG_Handler>();
		}
		bts = GetComponent<Button_Image_Swapper> ();
		if (gpgh.isLoggedIn) {
			GPActive = true;
			bts.Change_Image(0);
		}
		else{
			GPActive = false;
			bts.Change_Image(1);
		}


		// make sure the button has the correct functions loaded
		Button button = GetComponent<Button> ();

		switch(type){
		case "Games":
			button.onClick.AddListener(() => gpgh.Show_Notification());
			break;
		case "Leaderboards":
			button.onClick.AddListener(() => gpgh.Show_Leaderboards());
			break;
		case "Achievements":
			achievementHandler = GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<Achievement_Script>();
			button.onClick.AddListener(() => gpgh.Show_Achievements());
			button.onClick.AddListener (() => achievementHandler.Sync_With_Google_Play());
			break;
		default:
			button.onClick.AddListener(() => gpgh.Show_Notification());
			break;
		}
	}

	void FixedUpdate()
	{
		if (gpgh.isLoggedIn != GPActive) {
			if(GPActive){
				GPActive = false;
				bts.Change_Image (1);
			}
			else{
				GPActive = true;
				bts.Change_Image (0);
			}
		}
	}
}
