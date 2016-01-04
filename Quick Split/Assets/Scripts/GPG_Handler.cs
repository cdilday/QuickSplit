using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


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

	// Update is called once per frame
	void Update () {
	
	}
}
