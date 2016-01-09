using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class High_Score_Calculator : MonoBehaviour {

	//This script handles the High score list on the high score menu in the Main Menu
	//High scores are stored using the lookup "<Gamemode> score <#>" where <Gamemode> is the short name for the mode and <#> is the score's rank

	public Text[] ScoreList;
	public VerticalScrollSnap Scopes;
	public HorizontalScrollSnap Modes;

	int currScope;
	int prevScope;
	int currMode;
	int prevMode;

	GPG_Handler gpgh;
	
	// Use this for initialization
	void Start () {
		currScope = 0;
		prevScope = -1;
		currMode = 0;
		prevMode = 0;

		GameObject temp = GameObject.FindGameObjectWithTag ("Google Play");
		if (temp != null) {
			gpgh = temp.GetComponent<GPG_Handler>();
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		//check to make sure the scores for the proper scope and mode are on display, and if they aren't, change them
		if(Scopes.gameObject.activeInHierarchy){
			currScope = Scopes.CurrentScreen ();
			currMode = Modes.CurrentScreen ();
			if (currScope != prevScope || currMode != prevMode) {
				Populate_Scores_List();
			}
			prevScope = currScope;
			prevMode = currMode;
		}
	}

	//this actually fills the list in
	public void Populate_Scores_List(){
		if (Lookup_Scope (Scopes.CurrentScreen()) == "Local") {
			string gameMode = Lookup_Game_Type(Modes.CurrentScreen());
			for(int i = 0; i < 15; i++)
			{
				int currScore = PlayerPrefs.GetInt(gameMode + " score " + i, 0);
				if(currScore == 0)
				{
					ScoreList[i].text = (i+1) + ". -------";
				}else{
					ScoreList[i].text = (i+1) + ". " + currScore;
				}
			}
		}
		else{
			//TODO: Implement global and friends lists for real
			for(int i = 0; i < 14; i++)
				ScoreList[i].text = (i+1) + " -------";
			if(gpgh != null && gpgh.isSignedIn())
			{
				ScoreList[14].text = "Called this";
				ILeaderboard lb = PlayGamesPlatform.Instance.CreateLeaderboard();
				switch (Lookup_Game_Type(Modes.CurrentScreen())) {
				case "Wiz":
					lb.id = GPG_Ids.leaderboard_wiz_split_leaderboard;
					break;
				case "Quick":
					lb.id = GPG_Ids.leaderboard_quick_split_leaderboard;
					break;
				case "Wit":
					lb.id = GPG_Ids.leaderboard_wit_split_leaderboard;
					break;
				case "Holy":
					lb.id = GPG_Ids.leaderboard_holy_split_leaderboard;
					break;
				default:
					lb.id = GPG_Ids.leaderboard_wiz_split_leaderboard;
					break;
				}
				ScoreList[3].text = Lookup_Game_Type(Modes.CurrentScreen());
				lb.LoadScores(ok => {
					if (ok) {
						ScoreList[4].text = "Good";
						LoadUsersandScores(lb);
					}
					else {
						Debug.Log("Error retrieving leaderboardi");
						ScoreList[4].text = "Error";
						LoadUsersandScores(null);
					}
				});
			}
		}
	}



	//this resets all scores locally
	public void Reset_All_Scores(){
		for (int g = 0; g < 4; g++) {
			for (int i = 0; i < 15; i++){
				PlayerPrefs.SetInt(Lookup_Game_Type(g) + " score " + i, 0);
			}
		}
		//This will not affect server scores
	}

	//this returns the game type name at the given index
	string Lookup_Game_Type(int index){
		switch (index) {
		case 0:
			return "Wiz";
		case 1:
			return "Quick";
		case 2:
			return "Wit";
		case 3:
			return "Holy";
		default:
			return "Wiz";
		}
	}

	//This reutrns the name of the scope at the given index
	string Lookup_Scope(int index){
		switch (index) {
		case 0:
			return "Local";
		case 1:
			return "Friends";
		case 2:
			return "Global";
		default:
			return "Local";
		}
	}

	public void LoadUsersandScores(ILeaderboard lb)
	{
		ScoreList [0].text = "successfully called load";
		if (lb == null) {
			ScoreList [1].text = "but failed to get HS";
		}
		else{
			ScoreList [1].text = lb.localUserScore.ToString ();
		}
	}

}