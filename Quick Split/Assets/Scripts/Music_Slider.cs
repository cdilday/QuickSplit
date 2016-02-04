using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Music_Slider : MonoBehaviour {

	//This script is attatched to and is used for the volume sliders which control what volume the music is played at

	GameObject MC;
	Slider mySlider;
	// Use this for initialization
	void Start () {
		MC = GameObject.Find ("Music Controller");
		mySlider = gameObject.GetComponent<Slider> ();
		mySlider.value = PlayerPrefs.GetFloat ("Music Volume", 1);
		mySlider.onValueChanged.AddListener (delegate{onValueChanged ();});
	}

	//update the volume while the value is being changed
	void onValueChanged()
	{
		MC.GetComponent<Music_Controller> ().Change_Music_Volume (mySlider.value);
		PlayerPrefs.SetFloat ("Music Volume", mySlider.value);
	}

}