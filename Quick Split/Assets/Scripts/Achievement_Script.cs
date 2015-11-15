using UnityEngine;
using System.Collections;

public class Achievement_Script : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		//get rid of redundant Achievement Handlers
		GameObject[] mcs = GameObject.FindGameObjectsWithTag ("Achievement Handler");
		if (mcs.Length > 1) {
			Destroy(gameObject);
			return;
		}
		
		DontDestroyOnLoad (transform.gameObject);

		//make sure default sets are always unlocked at start of game to prevent crashes
		PlayerPrefs.SetInt("Wiz unlocked", 1);
		PlayerPrefs.SetInt ("Default Splitter unlocked", 1);
		PlayerPrefs.SetInt ("Default Pieceset unlocked", 1);
		PlayerPrefs.SetInt ("Symbol Pieceset unlocked", 1);
	}

	// Update is called once per frame
	void Update () {
		//debug reset all achievements button
		if (Debug.isDebugBuild) {
			if(Input.GetKey(KeyCode.Keypad5))
			{
				PlayerPrefs.SetInt("Wiz unlocked", 1);
				PlayerPrefs.SetInt("Quick unlocked", 0);
				PlayerPrefs.SetInt("Wit unlocked", 0);
				PlayerPrefs.SetInt("Holy unlocked", 0);
				Debug.Log ("Unlocked GameModes Reset!");
				PlayerPrefs.SetInt ("Default Splitter unlocked", 1);
				PlayerPrefs.SetInt ("Green Splitter unlocked", 0);
				PlayerPrefs.SetInt ("Programmer Splitter unlocked", 0);
				PlayerPrefs.SetInt ("Caution Splitter unlocked", 0);
				PlayerPrefs.SetInt ("Candy Cane Splitter unlocked", 0);
				PlayerPrefs.SetInt ("Dark Splitter unlocked", 0);
				Debug.Log("Unlocked Splitters Reset!");
				PlayerPrefs.SetInt ("Default Pieceset unlocked", 1);
				PlayerPrefs.SetInt ("Symbol Pieceset unlocked", 1);
				PlayerPrefs.SetInt ("Arcane Pieceset unlocked", 0);
				PlayerPrefs.SetInt ("Retro Pieceset unlocked", 0);
				PlayerPrefs.SetInt ("Programmer Pieceset unlocked", 0);
				PlayerPrefs.SetInt ("Blob Pieceset unlocked", 0);
				PlayerPrefs.SetInt ("Domino Pieceset unlocked", 0);
				PlayerPrefs.SetInt ("Present Pieceset unlocked", 0);
				PlayerPrefs.SetInt ("Pumpkin Pieceset unlocked", 0);
				PlayerPrefs.SetInt ("Techno Pieceset unlocked", 0);
				Debug.Log("Unlocked Piecesets Reset!");

			}
			else if (Input.GetKey(KeyCode.Keypad7))
			{
				PlayerPrefs.SetInt("Wiz unlocked", 1);
				PlayerPrefs.SetInt("Quick unlocked", 1);
				PlayerPrefs.SetInt("Wit unlocked", 1);
				PlayerPrefs.SetInt("Holy unlocked", 1);
				Debug.Log ("All GameModes Unlocked!");
				PlayerPrefs.SetInt ("Default Splitter unlocked", 1);
				PlayerPrefs.SetInt ("Green Splitter unlocked", 1);
				PlayerPrefs.SetInt ("Programmer Splitter unlocked", 1);
				PlayerPrefs.SetInt ("Caution Splitter unlocked", 1);
				PlayerPrefs.SetInt ("Candy Cane Splitter unlocked", 1);
				PlayerPrefs.SetInt ("Dark Splitter unlocked", 1);
				Debug.Log("All Splitters Unlocked!");
				PlayerPrefs.SetInt ("Default Pieceset unlocked", 1);
				PlayerPrefs.SetInt ("Symbol Pieceset unlocked", 1);
				PlayerPrefs.SetInt ("Arcane Pieceset unlocked", 1);
				PlayerPrefs.SetInt ("Retro Pieceset unlocked", 1);
				PlayerPrefs.SetInt ("Programmer Pieceset unlocked", 1);
				PlayerPrefs.SetInt ("Blob Pieceset unlocked", 1);
				PlayerPrefs.SetInt ("Domino Pieceset unlocked", 1);
				PlayerPrefs.SetInt ("Present Pieceset unlocked", 1);
				PlayerPrefs.SetInt ("Pumpkin Pieceset unlocked", 1);
				PlayerPrefs.SetInt ("Techno Pieceset unlocked", 1);
				Debug.Log("All Piecesets Unlocked!");
			}
		
		}
	}

	public bool Gamemode_Unlocked(string gameType)
	{
		if (PlayerPrefs.GetInt (gameType + " unlocked", 0) == 0) {
			return false;
		}
		return true;
	}

	public bool Splitter_Unlocked(string splitter){
		if (PlayerPrefs.GetInt (splitter + " Splitter unlocked", 0) == 0) {
			return false;
		}
		return true;
	}

	public bool Pieceset_Unlocked(string pieceSet)
	{
		if (PlayerPrefs.GetInt (pieceSet + " Pieceset unlocked", 0) == 0) {
			return false;
		}
		return true;
	}

	public void Check_Gamemode_Unlocked()
	{
		//the unlock order goes from Wiz -> Quick -> Wit -> Holy
		if (PlayerPrefs.GetInt ("Wiz", 0) > 0)
			PlayerPrefs.SetInt ("Quick unlocked", 1);
		if(PlayerPrefs.GetInt("Quick", 0) > 0)
			PlayerPrefs.SetInt ("Quick unlocked", 1);
		if (PlayerPrefs.GetInt ("Quick", 0) > 0)
			PlayerPrefs.SetInt ("Wit unlocked", 1);
		if (PlayerPrefs.GetInt ("Wit", 0) > 0)
			PlayerPrefs.SetInt ("Holy unlocked", 1);
		if(PlayerPrefs.GetInt("Holy", 0) > 0)
			PlayerPrefs.SetInt ("Present Pieceset unlocked", 1);
	}

	void OnLevelWasLoaded(int level){
		if (PlayerPrefs.GetInt ("Programmer Splitter unlocked", 0) == 0) {
			if(PlayerPrefs.GetInt ("Green Splitter unlocked", 0) == 1 &&
			   PlayerPrefs.GetInt ("Caution Splitter unlocked", 0) == 1 &&
			   PlayerPrefs.GetInt ("Candy Cane Splitter unlocked", 0) == 1 &&
			   PlayerPrefs.GetInt ("Dark Splitter unlocked", 0) == 1){
				//TODO: Progammer Splitter Unlock alert
				PlayerPrefs.SetInt ("Programmer Splitter unlocked", 1);
			}
		}
		if (PlayerPrefs.GetInt ("Programmer Pieceset unlocked", 0) == 0) {
			if(PlayerPrefs.GetInt ("Arcane Pieceset unlocked", 0) == 1 &&
			   PlayerPrefs.GetInt ("Retro Pieceset unlocked", 0) == 1 &&
			   PlayerPrefs.GetInt ("Blob Pieceset unlocked", 0) == 1 &&
			   PlayerPrefs.GetInt ("Domino Pieceset unlocked", 0) == 1 &&
			   PlayerPrefs.GetInt ("Present Pieceset unlocked", 0) == 1 &&
			   PlayerPrefs.GetInt ("Pumpkin Pieceset unlocked", 0) == 1 &&
			   PlayerPrefs.GetInt ("Techno Pieceset unlocked", 0) == 1){
				//TODO: Progammer Splitter Unlock alert
				PlayerPrefs.SetInt ("Programmer Pieceset unlocked", 1);
			}
		}
	}
}
