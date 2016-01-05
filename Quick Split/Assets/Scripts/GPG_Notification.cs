using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GPG_Notification : MonoBehaviour {

	Vector3 activePos = new Vector3(0,0,0);
	Vector3 inactivePos;

	bool isActive;
	bool isBusy;

	public Text description;
	GPG_Handler gpgh;

	RectTransform rectTransform;

	int stage = 0;

	float transitionLength = 0.3f;
	float startTime;

	public Image loadingIcon;

	public string[] prompts;
	// 0  = "Do you want to log in"
	// 1 = "Do you want to log out"
	// 2 = login successful
	// 3 = Login unsuccessful, retry?
	// 4 = Signout successful
	// 5 = working...

	// Use this for initialization
	void Start () {
		//first check to see if google play is even an option and the handler exist
		GameObject gpgObject = GameObject.FindGameObjectWithTag ("Google Play");
		if (gpgObject == null) {
			Destroy (gameObject);
			return;
		}


		rectTransform = GetComponent<RectTransform> ();
		inactivePos = rectTransform.position;
		gpgh = gpgObject.GetComponent<GPG_Handler> ();
		gpgh.notification = this;
		isActive = false;
		isBusy = false;
	}
	
	public void activate()
	{
		if (isActive || isBusy)
			return;

		if (!gpgh.isSignedIn()) {
			stage = 0;
			description.text = prompts[0];
		}
		else {
			stage = 1;
			description.text = prompts[1];
		}
		isActive = true;
		isBusy = true;
		startTime = Time.time;
	}

	public void deactivate()
	{
		if (!isActive || isBusy)
			return;

		isActive = false;
		isBusy = true;
		startTime = Time.time;
	}

	void FixedUpdate()
	{
		//movement on/off screen
		if (isBusy) {
			//moving on screen
			if(isActive){
				//has finished moving into place
				if(Time.time - startTime > transitionLength){
					rectTransform.position = activePos;
					isBusy = false;
				}
				else{
					//move into position
				}
			}
			//moving offscreen
			else{
				//it has finished moving offscreen
				if(Time.time - startTime > transitionLength){
					rectTransform.position = inactivePos;
					isBusy = false;
				}
				else{
					//move out of active position
				}
			}
		}
	}


	public void clickYes()
	{
		switch (stage) {
		case 0:
			gpgh.signIn (false);
			description.text = prompts[5];
			stage = 5;
			//make loading image visible
			break;
		case 1:
			gpgh.signOut();
			description.text = prompts[4];
			stage = 4;
			break;
		case 2:
			deactivate ();
			break;
		case 3:
			gpgh.signIn (false);
			description.text = prompts[5];
			stage = 4;
			//make loading image visible
			break;
		case 4:
			deactivate ();
			break;
		case 5:
			break;
		}
	}

	public void clickNo()
	{
		if (stage == 5)
			return;
		deactivate ();
	}

	public void GoogleSigninResponse(bool success){
		if (success) {
			stage = 2;
			description.text = prompts[2];
		}
		else{
			stage = 3;
			description.text = prompts[3];
		}
	}
}
