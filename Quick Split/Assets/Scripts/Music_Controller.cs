using UnityEngine;
using System.Collections;

public class Music_Controller : MonoBehaviour {

	//This script is attatched to the Music controller and handles the current song and volume that is being played

	public float musicVolume;
	public float SFXVolume;

	public AudioClip WizMusic;
	public AudioClip QuickMusic;
	public AudioClip WitMusic;
	public AudioClip HolyMusic;

	// 0 is the slow tick, 1 is the fast tick
	public AudioClip[] WizTicks;
	public AudioClip[] QuickTicks;
	public AudioClip[] WitTicks;
	public AudioClip[] HolyTicks;

	/* Song titles -> modes:
	 * Cog Capers = Quick
	 * Resistance March = Holy
	 * Satellite = Wit
	 * The Flying Machine = Wiz
	 * */

	AudioSource MusicSource;
	public AudioSource SlowTickSource;
	public AudioSource FastTickSource;

	bool isSlowTicking = false;
	bool isFastTicking = false;

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
				SlowTickSource.clip = WitTicks[0];
				FastTickSource.clip = WitTicks[1];
				break;
			case "Quick":
				MusicSource.clip = QuickMusic;
				SlowTickSource.clip = QuickTicks[0];
				FastTickSource.clip = QuickTicks[1];
				break;
			case "Wiz":
				MusicSource.clip = WizMusic;
				SlowTickSource.clip = WizTicks[0];
				FastTickSource.clip = WizTicks[1];
				break;
			case "Holy":
				MusicSource.clip = HolyMusic;
				SlowTickSource.clip = HolyTicks[0];
				FastTickSource.clip = HolyTicks[1];
				break;
			default:
				break;
			}
		}

		MusicSource.volume = PlayerPrefs.GetFloat ("Music Volume", 1);
		musicVolume = MusicSource.volume;
		SlowTickSource.volume = musicVolume;
		FastTickSource.volume = musicVolume;
		SFXVolume = PlayerPrefs.GetFloat ("SFX Volume", 1);

	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Keypad1)){
			Start_Slow_Tick();
		}
		else if(Input.GetKeyDown(KeyCode.Keypad2)){
			Stop_Slow_Tick();
		}
		else if(Input.GetKeyDown(KeyCode.Keypad3)){
			Start_Fast_Tick();
		}
		else if(Input.GetKeyDown(KeyCode.Keypad4)){
			Stop_Fast_Tick();
		}
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
		if (isSlowTicking) {
			SlowTickSource.Pause ();
		}
		if (isFastTicking) {
			FastTickSource.Pause ();
		}
	}

	//unpauses the music
	public void Resume_Music()
	{
		MusicSource.UnPause ();
		if (isSlowTicking) {
			SlowTickSource.UnPause ();
		}
		if (isFastTicking) {
			FastTickSource.UnPause ();
		}
	}

	//plays the musc for the given game type
	public void Play_Music(string gameType)
	{
		switch (gameType){
		case "Wit":
			MusicSource.clip = WitMusic;
			SlowTickSource.clip = WitTicks[0];
			FastTickSource.clip = WitTicks[1];
			break;
		case "Quick":
			MusicSource.clip = QuickMusic;
			SlowTickSource.clip = QuickTicks[0];
			FastTickSource.clip = QuickTicks[1];
			break;
		case "Wiz":
			MusicSource.clip = WizMusic;
			SlowTickSource.clip = WizTicks[0];
			FastTickSource.clip = WizTicks[1];
			break;
		case "Holy":
			MusicSource.clip = HolyMusic;
			SlowTickSource.clip = HolyTicks[0];
			FastTickSource.clip = HolyTicks[1];
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
		SlowTickSource.volume = musicVolume;
		FastTickSource.volume = musicVolume;
	}

	public void Start_Slow_Tick(){
		float startTime = MusicSource.time;
		SlowTickSource.Play ();
		SlowTickSource.timeSamples = MusicSource.timeSamples;
		isSlowTicking = true;
	}

	public void Stop_Slow_Tick(){
		SlowTickSource.Stop();
		isSlowTicking = false;
	}

	public void Start_Fast_Tick(){
		float startTime = MusicSource.time;
		FastTickSource.Play ();
		FastTickSource.timeSamples = MusicSource.timeSamples;
		isFastTicking = true;
	}
	
	public void Stop_Fast_Tick(){
		FastTickSource.Stop ();
		isFastTicking = false;
	}

}