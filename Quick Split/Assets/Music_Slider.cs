using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Music_Slider : MonoBehaviour {

	GameObject MC;
	AudioSource MCsource;
	Slider mySlider;
	// Use this for initialization
	void Start () {
		MC = GameObject.Find ("Music Controller");
		mySlider = gameObject.GetComponent<Slider> ();
		MCsource = MC.GetComponent<AudioSource> ();
		mySlider.value = PlayerPrefs.GetFloat ("Music Volume", 1);
		mySlider.onValueChanged.AddListener (delegate{onValueChanged ();});
	}

	void onValueChanged()
	{
		MCsource.volume = mySlider.value;
		PlayerPrefs.SetFloat ("Music Volume", mySlider.value);
	}
}
