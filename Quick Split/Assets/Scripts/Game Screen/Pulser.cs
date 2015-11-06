using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pulser : MonoBehaviour {
	Text thisText;
	public float defaultSize;

	public bool pulsing;
	public bool growing;
	public int counter;

	AudioSource scoreBlip;

	// Use this for initialization
	void Start () {
		thisText = gameObject.GetComponent<Text> ();
		defaultSize = thisText.fontSize;
		pulsing = false;
		growing = false;

		scoreBlip = gameObject.GetComponent<AudioSource> ();
	}
	
	void FixedUpdate(){
		if(pulsing){
			handlePulse();
		}
	}

	void beginPulse(){
		scoreBlip.volume = 0.5f * (PlayerPrefs.GetFloat ("SFX Volume", 1));
		scoreBlip.pitch = 1 + Random.Range (0, 0.1f);
		scoreBlip.Play ();
		if(pulsing){
			counter = 10;
			thisText.fontSize = (int) (defaultSize + ((float)counter/2.5f));
		}
		else{
			counter = 0;
			thisText.fontSize = (int) defaultSize;
		}
		pulsing = true;
		growing = true;
	}

	void handlePulse(){
		if(growing){
			counter +=6;
			thisText.fontSize = (int) (defaultSize + ((float)counter/2.5f));
			if(counter >= 10)
			{
				growing = false;
			}
		}
		else{
			counter--;
			thisText.fontSize = (int) (defaultSize + ((float)counter/2.5f));
			if(counter <= 0){
				pulsing = false;
				thisText.fontSize = (int) defaultSize;
			}
		}
	}
}
