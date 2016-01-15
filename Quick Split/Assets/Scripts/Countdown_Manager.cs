using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Countdown_Manager : MonoBehaviour {

	//inorder, 0 = SPLITIT, 1 =1, etc.
	public Sprite[] CountdownImages;

	float startTime;

	RectTransform rectTransform;
	GameController gameController;
	Image image;

	AudioSource CountdownSFX;

	int stage;



	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.FindGameObjectWithTag ("GameController");
		if (temp == null) {
			Destroy (gameObject);
			return;
		}
		gameController = temp.GetComponent <GameController> ();
		rectTransform = GetComponent<RectTransform> ();
		image = GetComponent<Image> ();
		CountdownSFX = GetComponent<AudioSource> ();
		if (gameController.gameType == "Quick") {
			Begin_Countdown();
		}
		else{
			Destroy (gameObject);
			return;
		}
	}

	void Update()
	{
		if ( gameController.isPaused) {
			CountdownSFX.Pause();
		}
		else {
			CountdownSFX.UnPause();
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (stage == 4) {
			//stage 4 is the initial wait stage, and just waits for a second while the shutters open
			if(Time.time >= startTime +1f){
				image.color = new Color (1, 1, 1, 1);
				CountdownSFX.volume = PlayerPrefs.GetFloat ("SFX Volume", 1);
				CountdownSFX.Play ();
				stage--;
				startTime = Time.time;
				image.sprite = CountdownImages[stage];
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 200);
			}
		}
		else if (stage > 0){
			if(Time.time >= startTime +1f){
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 200);
				stage--;
				startTime = Time.time;
				image.sprite = CountdownImages[stage];
			}
			else{
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y -1.5f);
			}
		}
		else if (stage == 0){
			if(Time.time >= startTime +1f){
				Destroy (gameObject);
				return;
			}
		}
	
	}

	public void Begin_Countdown(){
		image.color = new Color (1, 1, 1, 0);
		startTime = Time.time;
		//set first image
		stage = 4;
	}

}
