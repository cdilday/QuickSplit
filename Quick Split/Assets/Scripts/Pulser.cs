using UnityEngine;
using System.Collections;

public class Pulser : MonoBehaviour {
	GUIText thisText;
	float defaultSize;

	bool pulsing;
	bool growing;
	int counter;

	// Use this for initialization
	void Start () {
		thisText = gameObject.GetComponent<GUIText> ();
		defaultSize = thisText.fontSize;
		pulsing = false;
		growing = false;
	}
	
	void FixedUpdate(){
		if(pulsing){
			handlePulse();
		}
	}

	void beginPulse(){
		if(pulsing){
			counter = 10;
			thisText.fontSize = (int) defaultSize + counter;
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
			thisText.fontSize = (int) defaultSize + counter;
			if(counter >= 10)
			{
				growing = false;
			}
		}
		else{
			counter--;
			thisText.fontSize = (int) defaultSize + counter;
			if(counter <= 0){
				pulsing = false;
				thisText.fontSize = (int) defaultSize;
			}
		}
	}
}
