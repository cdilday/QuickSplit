﻿using UnityEngine;
using System.Collections;

public class Achievement_Script : MonoBehaviour {
	//TODO: Comment this script. Also write up how exactly to add new splitters/ tilesets

	bool cyanCheck;

	float startTime;

	public string[] Splitters;
	public bool[] splittersUnlocked;

	public string[] Piecesets;
	public bool[] piecesetsUnlocked;

	public Achievement_Notification notification;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
		//get rid of redundant Achievement Handlers
		GameObject[] mcs = GameObject.FindGameObjectsWithTag ("Achievement Handler");
		if (mcs.Length > 1) {
			Destroy(gameObject);
			return;
		}

		//make sure default sets are always unlocked at start of game to prevent crashes
		PlayerPrefs.SetInt("Wiz unlocked", 1);
		PlayerPrefs.SetInt ("Default Splitter unlocked", 1);
		PlayerPrefs.SetInt ("Default Pieceset unlocked", 1);
		PlayerPrefs.SetInt ("Symbol Pieceset unlocked", 1);

		//TODO: Conglomerate this, this is silly
		splittersUnlocked = new bool[GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ().Splitters.Length];
		piecesetsUnlocked = new bool[Piecesets.Length];

		for (int i = 0; i < splittersUnlocked.Length; i++) {
			if( PlayerPrefs.GetInt(Splitter_Lookup_Name_by_Index(i) + " Splitter unlocked", 0) == 0)
			{
				splittersUnlocked[i] = false;
			}
			else
			{
				splittersUnlocked[i] = true;
			}
		}

		for (int i = 0; i < piecesetsUnlocked.Length; i++) {
			if( PlayerPrefs.GetInt(Pieceset_Lookup_Name_by_Index(i) + " Pieceset unlocked", 0) == 0)
			{
				piecesetsUnlocked[i] = false;
			}
			else
			{
				piecesetsUnlocked[i] = true;
			}
		}

		if (PlayerPrefs.GetInt ("Programmer Splitter unlocked", 0) == 0) {
			bool check = true;
			for(int i = 0; i < splittersUnlocked.Length; i++)
			{
				if(splittersUnlocked[i] == false && Splitter_Lookup_Name_by_Index(i) != "Programmer")
				{
					check = false;
					break;
				}
			}
			if(check)
			{
				Unlock_Splitter("Programmer");
			}
		}
		
		if (PlayerPrefs.GetInt ("Programmer Pieceset unlocked", 0) == 0) {
			bool check = true;
			for(int i = 0; i < piecesetsUnlocked.Length; i++)
			{
				if(piecesetsUnlocked[i] == false && Pieceset_Lookup_Name_by_Index(i) != "Programmer")
				{
					check = false;
					break;
				}
			}
			if(check)
			{
				Unlock_Pieceset("Programmer");
			}
		}
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
				for(int i = 1; i < Splitters.Length; i++){
					splittersUnlocked[i] = false;
					PlayerPrefs.SetInt (Splitter_Lookup_Name_by_Index(i) + " Splitter unlocked", 0);
				}
				Debug.Log("Unlocked Splitters Reset!");
				for(int i = 1; i < Piecesets.Length; i++){
					piecesetsUnlocked[i] = false;
					PlayerPrefs.SetInt (Pieceset_Lookup_Name_by_Index(i) + " Pieceset unlocked", 0);
				}
				PlayerPrefs.SetInt ("Default Pieceset unlocked", 1);
				Unlock_Pieceset("Symbol");
				Debug.Log("Unlocked Piecesets Reset!");

			}
			else if (Input.GetKey(KeyCode.Keypad7))
			{
				PlayerPrefs.SetInt("Wiz unlocked", 1);
				PlayerPrefs.SetInt("Quick unlocked", 1);
				PlayerPrefs.SetInt("Wit unlocked", 1);
				PlayerPrefs.SetInt("Holy unlocked", 1);
				Debug.Log ("All GameModes Unlocked!");
				for(int i = 0; i < Splitters.Length; i++)
				{
					splittersUnlocked[i] = true;
					PlayerPrefs.SetInt (Splitter_Lookup_Name_by_Index(i) + " Splitter unlocked", 1);
				}
				Debug.Log("All Splitters Unlocked!");
				for(int i = 0; i < Piecesets.Length; i++)
				{
					piecesetsUnlocked[i] = true;
					PlayerPrefs.SetInt (Pieceset_Lookup_Name_by_Index(i) + " Pieceset unlocked", 1);
				}
				Debug.Log("All Piecesets Unlocked!");
			}
			else if (Input.GetKey (KeyCode.Keypad6))
			{
				Unlock_Pieceset("Pumpkin");
			}
		
		}
	}

	//use this to add new high scores
	public void Add_Score(string gameMode, int score)
	{
		//you need to actually score to save a high score
		if (score == 0)
			return;
		int tempScore = score;
		for (int i = 0; i < 15; i++) {
			int currScore = PlayerPrefs.GetInt (gameMode + " score " + i, 0);
			if(currScore == 0)
			{
				PlayerPrefs.SetInt (gameMode + " score " + i, tempScore);
				break;
			}
			else if(currScore <= tempScore)
			{
				PlayerPrefs.SetInt(gameMode + " score " + i, tempScore);
				tempScore = currScore;
			}
		}
	}

	void FixedUpdate()
	{
		if (cyanCheck && startTime + 3f < Time.time)
		{
			cyanCheck = false;
		}
	}

	public bool is_Gamemode_Unlocked(string gameType)
	{
		if (PlayerPrefs.GetInt (gameType + " unlocked", 0) == 0) {
			return false;
		}
		return true;
	}

	public void Unlock_Splitter(string name)
	{
		splittersUnlocked [Splitter_Lookup_Index_by_Name (name)] = true;
		PlayerPrefs.SetInt (name + " Splitter unlocked", 1);
		if (notification != null) {
			notification.Achievement_Unlocked(name, "Splitter");
		}
	}

	public bool is_Splitter_Unlocked(string splitter){
		return splittersUnlocked [Splitter_Lookup_Index_by_Name (splitter)];
	}

	public void Unlock_Pieceset(string name)
	{
		piecesetsUnlocked [Pieceset_Lookup_Index_by_Name (name)] = true;
		PlayerPrefs.SetInt (name + " Pieceset unlocked", 1);
		if (notification != null)
			notification.Achievement_Unlocked (name, "Pieceset");
	}

	public bool is_Pieceset_Unlocked(string pieceSet)
	{
		return piecesetsUnlocked[Pieceset_Lookup_Index_by_Name(pieceSet)];
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
			Unlock_Pieceset("Present");
	}

	void OnLevelWasLoaded(int level){
	}


	public string Splitter_Lookup_Name_by_Index(int index)
	{
		return Splitters [index];
	}

	public int Splitter_Lookup_Index_by_Name(string name)
	{
		switch (name) {
		case "Default":
			return 0;
		case "Programmer":
			return 1;
		case "Candy Cane":
			return 2;
		case "Caution":
			return 3;
		case "Dark":
			return 4;
		case "Red":
			return 5;
		case "Orange":
			return 6;
		case "Yellow":
			return 7;
		case "Green":
			return 8;
		case "Blue":
			return 9;
		case "Purple":
			return 10;
		case "Cyan":
			return 11;
		case "White":
			return 12;
		default:
			return 0;
		}
	}

	public string Pieceset_Lookup_Name_by_Index(int index)
	{
		return Piecesets [index];
	}
	
	public int Pieceset_Lookup_Index_by_Name(string name)
	{
		switch (name) {
		case "Default":
			return 0;
		case "Arcane":
			return 1;
		case "Retro":
			return 2;
		case "Programmer":
			return 3;
		case "Blob":
			return 4;
		case "Domino":
			return 5;
		case "Present":
			return 6;
		case "Pumpkin":
			return 7;
		case "Symbol":
			return 8;
		case "Techno":
			return 9;
		default:
			return 0;
		}
	}

	public void Cyan_Splitter_Checker()
	{
		if (splittersUnlocked [Splitter_Lookup_Index_by_Name ("Cyan")])
			return;
		else if (cyanCheck) {
			Unlock_Splitter("Cyan");
		}
		else
		{
			startTime = Time.time;
			cyanCheck = true;
		}
	}

	public IEnumerator Purple_Splitter_Checker(int oldDangerPieces)
	{
		if (!is_Splitter_Unlocked("Purple") && oldDangerPieces < 3){
			yield return new WaitForSeconds(3);
			if(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Get_Danger_Pieces() == 0)
				Unlock_Splitter("Purple");
		}
	}

	public IEnumerator Blue_Splitter_Checker(){
		GameController gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		int oldCount = 0;
		for(int r = 0; r < 8; r++){
			for(int c = 0; c < 16; c++){
				if(gameController.grid[r,c] != null){
					oldCount++;
				}
			}
		}
		yield return new WaitForSeconds (2f);
		int newCount = 0;
		for(int r = 0; r < 8; r++){
			for(int c = 0; c < 16; c++){
				if(gameController.grid[r,c] != null){
					newCount++;
				}
			}
		}

		if (oldCount - newCount >= 16)
				Unlock_Splitter ("Blue");
	} 
}
