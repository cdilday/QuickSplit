using UnityEngine;
using System.Collections;

public class Achievement_Script : MonoBehaviour {
	//this is for checking specifically for the cyan splitter unlock, as it requires timing
	bool cyanCheck;

	float startTime;
	//all the splitter names and their appropriate unlocks
	public string[] Splitters;
	public bool[] splittersUnlocked;

	//all of the pieceset names and an array of bools that are true if the pieceset at that index is unlocked
	public string[] Piecesets;
	public bool[] piecesetsUnlocked;

	public Achievement_Notification notification;

	GPG_Handler gpgh;

	// Use this for initialization
	void Awake () {
		//there should only ever be 1 of these
		DontDestroyOnLoad (transform.gameObject);
		//get rid of redundant Achievement Handlers
		GameObject[] mcs = GameObject.FindGameObjectsWithTag ("Achievement Handler");
		if (mcs.Length > 1) {
			Destroy(gameObject);
			return;
		}

		GameObject temp = GameObject.FindGameObjectWithTag ("Google Play");
		if (temp != null) {
			gpgh = temp.GetComponent<GPG_Handler>();
		}

		//make sure default sets are always unlocked at start of game to prevent crashes
		PlayerPrefs.SetInt("Wiz unlocked", 1);
		PlayerPrefs.SetInt ("Default Splitter unlocked", 1);
		PlayerPrefs.SetInt ("Default Pieceset unlocked", 1);
		PlayerPrefs.SetInt ("Symbol Pieceset unlocked", 1);

		splittersUnlocked = new bool[Splitters.Length];
		piecesetsUnlocked = new bool[Piecesets.Length];

		//load the arrays for unlocks with the proper values already saved in prefs. This prevents longer lookups later
		for (int i = 0; i < splittersUnlocked.Length; i++) {
			if( PlayerPrefs.GetInt(Splitter_Lookup_Name_by_Index(i) + " Splitter unlocked", 0) == 0){
				splittersUnlocked[i] = false;
			}
			else{
				splittersUnlocked[i] = true;
			}
		}

		for (int i = 0; i < piecesetsUnlocked.Length; i++) {
			if( PlayerPrefs.GetInt(Pieceset_Lookup_Name_by_Index(i) + " Pieceset unlocked", 0) == 0){
				piecesetsUnlocked[i] = false;
			}
			else{
				piecesetsUnlocked[i] = true;
			}
		}
		//the programmer unlocks require all other things to be unlocked, so do that check now
		if (PlayerPrefs.GetInt ("Programmer Splitter unlocked", 0) == 0) {
			bool check = true;
			for(int i = 0; i < splittersUnlocked.Length; i++){
				if(splittersUnlocked[i] == false && Splitter_Lookup_Name_by_Index(i) != "Programmer"){
					check = false;
					break;
				}
			}
			if(check){
				Unlock_Splitter("Programmer");
			}
		}
		
		if (PlayerPrefs.GetInt ("Programmer Pieceset unlocked", 0) == 0) {
			bool check = true;
			for(int i = 0; i < piecesetsUnlocked.Length; i++){
				if(piecesetsUnlocked[i] == false && Pieceset_Lookup_Name_by_Index(i) != "Programmer"){
					check = false;
					break;
				}
			}
			if(check){
				Unlock_Pieceset("Programmer");
			}
		}
	}

	// Update is called once per frame
	void Update () {
		//debug buttons
		// numpad 5 resets all the unlocks
		// numpad 7 unlocks all the things
		// numpad 6 unlocks the pumpkin pieceset
		if (Debug.isDebugBuild) {
			if(Input.GetKey(KeyCode.Keypad5)){
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
				for(int i = 0; i < Splitters.Length; i++){
					splittersUnlocked[i] = true;
					PlayerPrefs.SetInt (Splitter_Lookup_Name_by_Index(i) + " Splitter unlocked", 1);
				}
				Debug.Log("All Splitters Unlocked!");
				for(int i = 0; i < Piecesets.Length; i++){
					piecesetsUnlocked[i] = true;
					PlayerPrefs.SetInt (Pieceset_Lookup_Name_by_Index(i) + " Pieceset unlocked", 1);
				}
				Debug.Log("All Piecesets Unlocked!");
			}
			else if (Input.GetKey (KeyCode.Keypad6)){
				Unlock_Pieceset("Pumpkin");
			}
			else if (Input.GetKey (KeyCode.Keypad1)){
				Debug.Log ("Wiz " + PlayerPrefs.GetInt("Wiz unlocked", 1));
				Debug.Log ("Quick " + PlayerPrefs.GetInt("Quick unlocked", 0));
				Debug.Log ("Wit " + PlayerPrefs.GetInt("Wit unlocked", 0));
				Debug.Log ("Holy " + PlayerPrefs.GetInt("Holy unlocked", 0));
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

		if (gpgh != null && gpgh.isSignedIn () && score > PlayerPrefs.GetInt (gameMode + " score 0", 0)) {
			gpgh.Post_Score(gameMode, score);
		}

		for (int i = 0; i < 15; i++) {
			int currScore = PlayerPrefs.GetInt (gameMode + " score " + i, 0);
			if(currScore == 0){
				//We've hit the end of the list. Place the score here and exit
				PlayerPrefs.SetInt (gameMode + " score " + i, tempScore);
				break;
			}
			else if(currScore <= tempScore){
				//continue looking through the list
				PlayerPrefs.SetInt(gameMode + " score " + i, tempScore);
				tempScore = currScore;
			}
		}
	}

	void FixedUpdate()
	{
		//More cyan unlock stuff. This resets it if the time has passed
		if (cyanCheck && startTime + 3f < Time.time){
			cyanCheck = false;
		}
	}

	//returns true if the given game mode is unlocked
	public bool is_Gamemode_Unlocked(string gameType)
	{
		if (PlayerPrefs.GetInt (gameType + " unlocked", 0) == 0) {
			return false;
		}
		return true;
	}

	//unlocks the splitter with the given name
	public void Unlock_Splitter(string name)
	{
		splittersUnlocked [Splitter_Lookup_Index_by_Name (name)] = true;
		PlayerPrefs.SetInt (name + " Splitter unlocked", 1);
		if (notification != null) {
			notification.Achievement_Unlocked(name, "Splitter");
		}
		if(gpgh != null && gpgh.isSignedIn()){
			Social.ReportProgress(Name_to_ID(true, name), 100.0f, (bool success) => {
				// handle success or failure, dunno if necessary here
			});
		}
	}

	//returns true if the given splitter name is unlocked
	public bool is_Splitter_Unlocked(string splitter){
		return splittersUnlocked [Splitter_Lookup_Index_by_Name (splitter)];
	}

	//unlocks the pieceset with the given name
	public void Unlock_Pieceset(string name)
	{
		piecesetsUnlocked [Pieceset_Lookup_Index_by_Name (name)] = true;
		PlayerPrefs.SetInt (name + " Pieceset unlocked", 1);
		if (notification != null)
			notification.Achievement_Unlocked (name, "Pieceset");
		if(gpgh != null && gpgh.isSignedIn()){
			Social.ReportProgress(Name_to_ID(false, name), 100.0f, (bool success) => {
				// handle success or failure, dunno if necessary here
			});
		}
	}

	//returns true if the given pieceset name is unlocked
	public bool is_Pieceset_Unlocked(string pieceSet)
	{
		return piecesetsUnlocked[Pieceset_Lookup_Index_by_Name(pieceSet)];
	}

	//unlocks gamemodes as the player scores in previous ones
	public void Check_Gamemode_Unlocked()
	{
		//the unlock order goes from Wiz -> Quick -> Wit -> Holy
		if (PlayerPrefs.GetInt ("Wiz score 0", 0) > 0)
			PlayerPrefs.SetInt ("Quick unlocked", 1);
		if(PlayerPrefs.GetInt("Quick score 0", 0) > 0)
			PlayerPrefs.SetInt ("Wit unlocked", 1);
		if (PlayerPrefs.GetInt ("Wit score 0", 0) > 0)
			PlayerPrefs.SetInt ("Holy unlocked", 1);
		if(PlayerPrefs.GetInt("Holy score 0", 0) > 0)
			Unlock_Pieceset("Present");
	}

	//returns the name of the splitter at the given index
	public string Splitter_Lookup_Name_by_Index(int index)
	{
		return Splitters [index];
	}

	//returns the index of the splitter with the given name
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

	//returns the name of the Pieceset at the given index
	public string Pieceset_Lookup_Name_by_Index(int index)
	{
		return Piecesets [index];
	}

	//returns the index of the pieceset with the given name
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

	//takes the type of achievement and name of unlock, and returns the GPG ID
	string Name_to_ID(bool isSplitter, string name)
	{
		if (isSplitter) {
			switch(name){
			case "Programmer":
				return GPG_Ids.achievement_how_was_this_functional;
			case "Candy Cane":
				return GPG_Ids.achievement_some_candy_for_the_pain;
			case "Caution":
				return GPG_Ids.achievement_get_behind_the_line;
			case "Dark":
				return GPG_Ids.achievement_no_light_only_darkness;
			case "Red":
				return GPG_Ids.achievement_not_reddy_to_die;
			case "Orange":
				return GPG_Ids.achievement_well_orange_you_clever;
			case "Yellow":
				return GPG_Ids.achievement_nothing_to_yellowver;
			case "Green":
				return GPG_Ids.achievement_looking_for_greener_pastures;
			case "Blue":
				return GPG_Ids.achievement_out_with_the_old_in_with_the_blue;
			case "Purple":
				return GPG_Ids.achievement_dismantling_hostile_environments_using_nonviolet_solution;
			case "Cyan":
				return GPG_Ids.achievement_ill_be_cyan_you_later;
			case "White":
				return GPG_Ids.achievement_cleaned_up_white_away;
			}
		}
		else{
			switch(name){
			case "Arcane":
				return GPG_Ids.achievement_youre_a_wiz_at_this;
			case "Retro":
				return GPG_Ids.achievement_8bits_of_splits;
			case "Programmer":
				return GPG_Ids.achievement_there_are_some_grey_areas;
			case "Blob":
				return GPG_Ids.achievement_what_a_mess;
			case "Domino":
				return GPG_Ids.achievement_its_a_chain_reaction;
			case "Present":
				return GPG_Ids.achievement_not_exactly_regifting;
			case "Pumpkin":
				return GPG_Ids.achievement_cheater_cheater_pumpkineater;
			case "Techno":
				return GPG_Ids.achievement_sleek_technology;
			}
		}

		return null;
	}
	//this is for checking if the cyan splitter conditions for unlock have been met
	public void Cyan_Splitter_Checker()
	{
		if (splittersUnlocked [Splitter_Lookup_Index_by_Name ("Cyan")])
			return;
		else if (cyanCheck) {
			Unlock_Splitter("Cyan");
		}
		else{
			startTime = Time.time;
			cyanCheck = true;
		}
	}

	//this is for handling the purple splitter's conditions for unlocking
	public IEnumerator Purple_Splitter_Checker(int oldDangerPieces)
	{
		if (!is_Splitter_Unlocked("Purple") && oldDangerPieces < 3){
			yield return new WaitForSeconds(3);
			if(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Get_Danger_Pieces() == 0)
				Unlock_Splitter("Purple");
		}
	}

	//this is for handling the blue splitter's conditions for unlocking
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

	//this syncs all locally unlocked achievements with Google play, unlocking all GP achievements that correspond with locally unlocked
	public void Sync_With_Google_Play(){
		if (gpgh == null || !gpgh.isSignedIn ())
			return;
		for (int i = 1; i < splittersUnlocked.Length; i++) {
			if(splittersUnlocked[i]){
				Social.ReportProgress(Name_to_ID(true, Splitter_Lookup_Name_by_Index(i)), 100.0f, (bool success) => {
					// handle success or failure, dunno if necessary here
				});
			}
		}

		for (int i = 1; i < piecesetsUnlocked.Length; i++) {
			if(i != Pieceset_Lookup_Index_by_Name("Symbol") && piecesetsUnlocked[i]){
				Social.ReportProgress(Name_to_ID(false, Pieceset_Lookup_Name_by_Index(i)), 100.0f, (bool success) => {
					// handle success or failure, dunno if necessary here
				});
			}
		}
	}

}
