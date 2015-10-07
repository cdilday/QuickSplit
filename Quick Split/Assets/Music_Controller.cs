using UnityEngine;
using System.Collections;

public class Music_Controller : MonoBehaviour {

	//this is for adjusting in the options menu that we'll definitely have
	public int volume;


	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform.gameObject);
		if (Application.loadedLevel == 0) {
						//play title main menu music
		} else {
			//we're in the game scene, need to look up game type
			switch (GameObject.Find("Gamecontroller").GetComponent<GameController>().gameType)
			{
			case "Wit":
				break;
			case "Quick":
				break;
			case "Wiz":
				break;
			case "Holy":
				break;
			default:
				break;
			}
		}
	}

	public void Stop_Music()
	{

	}

	public void Pause_Music()
	{
	
	}

	public void Resume_Music()
	{

	}

	public void Play_Music(string songTitle)
	{

	}

	//for if you just wanted to read in the track number
	public void Play_Music(int trackNum)
	{

	}
}
