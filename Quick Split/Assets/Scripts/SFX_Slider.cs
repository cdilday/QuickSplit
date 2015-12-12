using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems; 

public class SFX_Slider : MonoBehaviour, IPointerUpHandler {

	//this script is attached to the SFX sliders and is used for updating their volumes

	GameObject MC;
	//AudioSource MCsource;
	Slider mySlider;
	AudioSource sampleSource;
	// Use this for initialization
	void Awake () {
		MC = GameObject.Find ("Music Controller");
		mySlider = gameObject.GetComponent<Slider> ();
		mySlider.value = PlayerPrefs.GetFloat ("SFX Volume", 1);
		mySlider.onValueChanged.AddListener (delegate{onValueChanged ();});

		sampleSource = gameObject.GetComponent<AudioSource> ();
	}

	//update the value as soon as it changes
	void onValueChanged()
	{
		MC.GetComponent<Music_Controller> ().SFXVolume = mySlider.value;
		PlayerPrefs.SetFloat ("SFX Volume", mySlider.value);
	}

	//this will play a sound to indicate what it will sound like at the new volume
	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		sampleSource.volume = PlayerPrefs.GetFloat ("SFX Volume", 1);
		sampleSource.Play ();
	}
}
