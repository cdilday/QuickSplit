using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Achievement_Notification : MonoBehaviour {

	public Text NameText;
	public Text UnlockText;
	Image backGround;

	Achievement_Script achievementHandler;

	float startTime;
	int stage = 0;
	public bool isBusy;

	float fadeDuration = 1.5f;
	float waitDuration = 3f;

	// Use this for initialization
	void Start () {
		achievementHandler = GameObject.Find ("Achievement Handler").GetComponent<Achievement_Script> ();
		achievementHandler.notification = GetComponent<Achievement_Notification> ();
		backGround = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isBusy) {
			if(stage == 1){
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
			}else if(stage == 2){
				if(Time.time - startTime > waitDuration){
					stage = 3;
					startTime = Time.time;
				}
			}else if(stage == 3){
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
		if (isBusy) {
			//TODO: play the unlock sound delayed
			return;
		}

		//TODO: play the unlock sound

		isBusy = true;

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

		stage = 1;
		startTime = Time.time;

	}
}
