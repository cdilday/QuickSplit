using UnityEngine;
using System.Collections;

public class Music_Controller : MonoBehaviour {

	//This script is attatched to the Music controller and handles the current song and volume that is being played

	public float musicVolume;
	public float SFXVolume;

	public AudioClip WitMusic;
	public AudioClip QuickMusic;
	public AudioClip WizMusic;
	public AudioClip HolyMusic;

	AudioSource MusicSource;

	void Awake(){
		//get rid of redundant music controllers, there should always be one but only one
		GameObject[] mcs = GameObject.FindGameObjectsWithTag ("Music Controller");
		if (mcs.Length > 1) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad (transform.gameObject);

		MusicSource = gameObject.GetComponent<AudioSource> ();
		MusicSource.loop = true;

		if (Application.loadedLevel == 0) {
			//TODO:play title main menu music
		} 
		else {
			//we're in the game scene, need to look up game type
			switch (GameObject.Find("Gamecontroller").GetComponent<GameController>().gameType){
			case "Wit":
				MusicSource.clip = WitMusic;
				break;
			case "Quick":
				MusicSource.clip = QuickMusic;
				break;
			case "Wiz":
				MusicSource.clip = WizMusic;
				break;
			case "Holy":
				MusicSource.clip = HolyMusic;
				break;
			default:
				break;
			}
		}

		MusicSource.volume = PlayerPrefs.GetFloat ("Music Volume", 1);
		SFXVolume = PlayerPrefs.GetFloat ("SFX Volume", 1);
	}

	//stops the music
	public void Stop_Music()
	{
		MusicSource.Stop ();
	}

	//pauses the music
	public void Pause_Music()
	{
		MusicSource.Pause ();
	}

	//unpauses the music
	public void Resume_Music()
	{
		MusicSource.UnPause ();
	}

	//plays the musc for the given game type
	public void Play_Music(string gameType)
	{
		switch (gameType){
		case "Wit":
			MusicSource.clip = WitMusic;
			break;
		case "Quick":
			MusicSource.clip = QuickMusic;
			break;
		case "Wiz":
			MusicSource.clip = WizMusic;
			break;
		case "Holy":
			MusicSource.clip = HolyMusic;
			break;
		default:
			break;
		}

		MusicSource.Play ();

	}

	//for if you just wanted to read in the track number
	public void Play_Music(int trackNum)
	{
		switch (trackNum){
		case 1:
			MusicSource.clip = WitMusic;
			break;
		case 2:
			MusicSource.clip = QuickMusic;
			break;
		case 3:
			MusicSource.clip = WizMusic;
			break;
		case 4:
			MusicSource.clip = HolyMusic;
			break;
		default:
			break;
		}

		MusicSource.Play ();
	}

	//Changes the volume of the music
	public void Change_Music_Volume(float value)
	{
		musicVolume = value;
		MusicSource.volume = musicVolume;
	}

}