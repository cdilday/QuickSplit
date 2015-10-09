using UnityEngine;
using System.Collections;

public class Music_Controller : MonoBehaviour {

	//this is for adjusting in the options menu that we'll definitely have
	public float musicVolume;
	public float SFXVolume;

	public AudioClip WitMusic;
	public AudioClip QuickMusic;
	public AudioClip WizMusic;
	public AudioClip HolyMusic;

	AudioSource MusicSource;

	void Awake(){
		//get rid of redundant music controllers
		GameObject[] mcs = GameObject.FindGameObjectsWithTag ("Music Controller");
		if (mcs.Length > 1) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad (transform.gameObject);

		MusicSource = gameObject.GetComponent<AudioSource> ();
		MusicSource.loop = true;

		if (Application.loadedLevel == 0) {
			//play title main menu music
		} else {
			//we're in the game scene, need to look up game type
			switch (GameObject.Find("Gamecontroller").GetComponent<GameController>().gameType)
			{
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
	}

	public void Stop_Music()
	{
		MusicSource.Stop ();
	}

	public void Pause_Music()
	{
		MusicSource.Pause ();
	}

	public void Resume_Music()
	{
		MusicSource.UnPause ();
	}

	public void Play_Music(string gameType)
	{
		switch (gameType)
		{
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
		switch (trackNum)
		{
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

	public void Change_Music_Volume(float value)
	{
		musicVolume = value;
		MusicSource.volume = musicVolume;
	}
}
