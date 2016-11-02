using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Achievement_Notification : MonoBehaviour {

	// Text that holds the achievement name
	public Text NameText;
	// Text that holds the achievement flavor text
	public Text UnlockText;
	//backdrop for the notifications
	Image backGround;

	Achievement_Script achievementHandler;

	float startTime;
	//there are 4 stages. 0 is not active, 1 is fading in, 2 is idle but active, and 3 is fading out
	int stage = 0;
	public bool isBusy;

	//how long the fading takes
	float fadeDuration = 1.5f;
	//how long it stays onscreen between fadings
	float waitDuration = 3f;

	AudioSource AchievementSFX;

	// Use this for initialization
	void Start () {
		achievementHandler = GameObject.Find ("Achievement Handler").GetComponent<Achievement_Script> ();
		achievementHandler.notification = GetComponent<Achievement_Notification> ();
		backGround = GetComponent<Image> ();
		AchievementSFX = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//if it's being used
		if (isBusy) {
			//fading in stage
			if(stage == 1){
				//check if it is fully faded in
				if(Time.time - startTime > fadeDuration){
					stage = 2;
					backGround.color = new Color(1,1,1,1);
					NameText.color = new Color(NameText.color.r, NameText.color.g, NameText.color.b, 1);
					UnlockText.color = new Color(UnlockText.color.r, UnlockText.color.g, UnlockText.color.b, 1);
					startTime = Time.time;
				}
				else{
					//fade in alpha
					float progress = (Time.time - startTime) / fadeDuration;
					backGround.color = new Color(1,1,1,progress);
					NameText.color = new Color(NameText.color.r, NameText.color.g, NameText.color.b, progress);
					UnlockText.color = new Color(UnlockText.color.r, UnlockText.color.g, UnlockText.color.b, progress);
				}
			}
			//idle stage
			else if(stage == 2){
				//check if the idle time has passed, then move on
				if(Time.time - startTime > waitDuration){
					stage = 3;
					startTime = Time.time;
				}
			}
			//fading out stage
			else if(stage == 3){
				//check if it should be fully faded out, than reset it for reuse
				if(Time.time - startTime > fadeDuration){
					stage =  0;
					isBusy = false;
					backGround.color = new Color(1,1,1,0);
					NameText.color = new Color(NameText.color.r, NameText.color.g, NameText.color.b, 0);
					UnlockText.color = new Color(UnlockText.color.r, UnlockText.color.g, UnlockText.color.b, 0);
				}
				else{
					//fade out alpha
					float progress = (Time.time - startTime) / fadeDuration;
					backGround.color = new Color(1,1,1,1f-progress);
					NameText.color = new Color(NameText.color.r, NameText.color.g, NameText.color.b, 1f - progress);
					UnlockText.color = new Color(UnlockText.color.r, UnlockText.color.g, UnlockText.color.b, 1f - progress);
				}
			}
		}
	
	}

	public void Achievement_Unlocked(string thingUnlocked, string unlockType){
		// We're going to send Kongregate the achievement info First, then check for busyness of the notification
		GameObject temp = GameObject.FindGameObjectWithTag ("Kong Controller");
		if (temp != null) {
			string achieveName = "";
			if (unlockType == "Splitter") {
				UnlockText.text = "New Splitter Unlocked!";
				switch(thingUnlocked){
				case "Candy Cane":
					achieveName = "Some Candy for the Pain";
					break;
				case "Dark":
					achieveName = "No Light; only darkness";
					break;
				case "Caution":
					achieveName = "Get behind the line";
					break;
				case "Red":
					achieveName = "Not Reddy to die";
					break;
				case "Orange":
					achieveName = "Well Orange you clever?";
					break;
				case "Yellow":
					achieveName = "Nothing to Yellow-ver";
					break;
				case "Green":
					achieveName = "Looking for Greener Pastures";
					break;
				case "Blue":
					achieveName = "In with the Blue";
					break;
				case "Purple":
					achieveName = "Non-Violet Solutions";
					break;
				case "Cyan":
					achieveName = "I'll be Cyan you later";
					break;
				case "White":
					achieveName = "Cleaned up White away";
					break;
				case "Programmer":
					achieveName = "How did this work?";
					break;
				}
			}
			else if(unlockType == "Pieceset"){
				UnlockText.text = "New Pieceset Unlocked!";
				switch(thingUnlocked){
				case "Blob":
					achieveName = "What a mess...";
					break;
				case "Retro":
					achieveName = "8-bits of Splits";
					break;
				case "Techno":
					achieveName = "Sleek Splits";
					break;
				case "Arcane":
					achieveName = "You're a Wazard";
					break;
				case "Present":
					achieveName = "Not Exactly Re-gifting";
					break;
				case "Domino":
					achieveName = "A Crazy Contraption";
					break;
				case "Pumpkin":
					achieveName = "Cheater Cheater Pumpkin-Eater";
					break;
				case "Programmer":
					achieveName = "Grey Areas";
					break;
				}
			}
			if(achieveName != "")
				temp.GetComponent<KongController>().Submit_Achievement(achieveName);
			else
				Debug.Log("given Achievement doesn't match any programmed ones");
		} 
		//first check to make sure it's not in use. if it is, just play the sound so they know something happened
		if (isBusy) {
			return;
		}
		AchievementSFX.volume = PlayerPrefs.GetFloat ("SFX Volume", 1);
		AchievementSFX.Play ();

		isBusy = true;

		//if it's a splitter, it needs to have the right description
		if (unlockType == "Splitter") {
			UnlockText.text = "New Splitter Unlocked!";
			switch(thingUnlocked){
			case "Candy Cane":
				NameText.text = "Some Candy for the Pain";
				break;
			case "Dark":
				NameText.text = "No Light; only darkness";
				break;
			case "Caution":
				NameText.text = "Get behind the line";
				break;
			case "Red":
				NameText.text = "Not Reddy to die";
				break;
			case "Orange":
				NameText.text = "Well Orange you clever?";
				break;
			case "Yellow":
				NameText.text = "Nothing to Yellow-ver";
				break;
			case "Green":
				NameText.text = "Looking for Greener Pastures";
				break;
			case "Blue":
				NameText.text = "In with the Blue";
				break;
			case "Purple":
				NameText.text = "Non-Violet Solutions";
				break;
			case "Cyan":
				NameText.text = "I'll be Cyan you later";
				break;
			case "White":
				NameText.text = "Cleaned up White away";
				break;
			case "Programmer":
				NameText.text = "How did this work?";
				break;
			}
		}
		//if it's a pieceset, it also needs to have the right description
		else if(unlockType == "Pieceset"){
			UnlockText.text = "New Pieceset Unlocked!";
			switch(unlockType = thingUnlocked){
			case "Blob":
				NameText.text = "What a mess...";
				break;
			case "Retro":
				NameText.text = "8-bits of Splits";
				break;
			case "Techno":
				NameText.text = "Sleek Splits";
				break;
			case "Arcane":
				NameText.text = "You're a Wazard";
				break;
			case "Present":
				NameText.text = "Not Exactly Re-gifting";
				break;
			case "Domino":
				NameText.text = "A Crazy Contraption";
				break;
			case "Pumpkin":
				NameText.text = "Cheater Cheater Pumpkin-Eater";
				break;
			case "Programmer":
				NameText.text = "Grey Areas";
				break;
			}
		}
		//begin the fadin stage
		stage = 1;
		startTime = Time.time;

	}
}
