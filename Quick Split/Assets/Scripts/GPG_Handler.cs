using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


public class GPG_Handler : MonoBehaviour {

	public bool isLoggedIn = false;
	public bool debugIsMobile;

	PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ().Build();

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
	
	}

	public void signIn(){
		if(!isLoggedIn){
			// authenticate user:
			Social.localUser.Authenticate((bool success) => {
				if(success){
					//dim the login button
					//Update and remove the standby screen
					//sync achievements?
				}
				else{
					//say i failed on the standby screen
					//see if internet connection is on and if that is why
					//offer to retry or exit the standby screen
				}

			});
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
