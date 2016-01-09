using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;


public class GPG_Handler : MonoBehaviour {

	bool isLoggedIn = false;
	public bool debugIsMobile;

	PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ().Build();

	public GPG_Notification notification;

	void Awake(){
		GameObject[] gphs = GameObject.FindGameObjectsWithTag ("Google Play");
		if (gphs.Length > 1) {
			Destroy(gameObject);
			return;
		}
		//this is to stay alive at all times
		DontDestroyOnLoad (transform.gameObject);

		if( Application.platform == RuntimePlatform.Android || debugIsMobile){
			PlayGamesPlatform.InitializeInstance(config);
			// recommended for debugging:
			PlayGamesPlatform.DebugLogEnabled = true;
			// Activate the Google Play Games platform
			PlayGamesPlatform.Activate();
		}
		else{
			Destroy(gameObject);
			return;
		}
	}

	// Use this for initialization
	void Start () {
		//check if auto-login is a thing, if so login
		if (PlayerPrefs.GetInt ("auto-login", 0) == 1) {
			signIn (true);
		}
	}

	public bool isSignedIn(){
		isLoggedIn = PlayGamesPlatform.Instance.IsAuthenticated ();
		return isLoggedIn;
	}

	public void signIn(bool atStart)
	{
		if(!isLoggedIn){
			//login using the button
			if(!atStart){
				// authenticate user:
				Social.localUser.Authenticate((bool success) => {
					if(success){
						notification.GoogleSigninResponse(success);
						//sync achievements?
					}
					else{
						notification.GoogleSigninResponse(success);
					}

				});
			}
			else {
				Social.localUser.Authenticate((bool success) => {
					if(success){
						notification.GoogleSigninResponse(success);
						//sync achievements?
					}
					else{
						notification.GoogleSigninResponse(success);
					}
					
				});
				//login from the auto-login at the start
			}
		}
	}

	public void signOut()
	{
		PlayGamesPlatform.Instance.SignOut();
	}

	public void Post_Score(string gameMode, int score){
		string LBID;
		switch (gameMode) {
		case "Wiz":
			LBID = GPG_Ids.leaderboard_wiz_split_leaderboard;
			break;
		case "Quick":
			LBID = GPG_Ids.leaderboard_quick_split_leaderboard;
			break;
		case "Wit":
			LBID = GPG_Ids.leaderboard_wit_split_leaderboard;
			break;
		case "Holy":
			LBID = GPG_Ids.leaderboard_holy_split_leaderboard;
			break;
		default:
			LBID = null;
			break;
		}

		if(LBID != null){
			Social.ReportScore(score, LBID, (bool success) => {
				GameObject temp = GameObject.Find ("Google Play Text");
				if(temp != null){
					if(success){
						temp.GetComponent<Text>().text = "SSCORE SENT TO GOOGLE PLAY LEADERBOARDS";
					}
					else{
						temp.GetComponent<Text>().text = "FAILED TO SEND SCORE TO GOOGLEPLAY LEADERBOARDS";
					}
				}
			});
		}
	}

	public void Show_Leaderboards(){
		Social.ShowLeaderboardUI();
	}

	public void Load_LeaderBoards(string gameType){
		ILeaderboard lb = PlayGamesPlatform.Instance.CreateLeaderboard();
		switch (gameType) {
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
		lb.id = "MY_LEADERBOARD_ID";
		lb.LoadScores(ok =>
		              {
			if (ok) {
				GameObject temp = GameObject.Find ("High Score Calculator");
				if(temp != null)
				{
					High_Score_Calculator hsc = temp.GetComponent<High_Score_Calculator>();
					hsc.LoadUsersandScores(lb);
				}
			}
			else {
				Debug.Log("Error retrieving leaderboardi");
				GameObject temp = GameObject.Find ("High Score Calculator");
				if(temp != null)
				{
					High_Score_Calculator hsc = temp.GetComponent<High_Score_Calculator>();
					hsc.LoadUsersandScores(null);
				}
			}
		});
	}

	// Update is called once per frame
	void Update () {
	
	}

}