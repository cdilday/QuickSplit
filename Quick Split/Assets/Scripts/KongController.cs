using UnityEngine;
using System.Collections;

public class KongController : MonoBehaviour {
	
	private static KongController instance;

	// Use this for initialization
	void Awake ()
	{
		//get rid of redundant kong controllers, there should always be one but only one
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);

		Application.ExternalEval(
      		@"if(typeof(kongregateUnitySupport) != 'undefined'){
        	kongregateUnitySupport.initAPI('KongregateAPI', 'OnKongregateAPILoaded');
      		};"
    	);
	}

	//
	public void Submit_Score(string gameType, int score){
		Application.ExternalCall("kongregate.stats.submit", gameType, score);
	}

	//No real difference between this and submit score other than this is only for sending if an achievement has unlocked to kong
	public void Submit_Achievement (string achievename){
		Application.ExternalCall("kongregate.stats.submit", achievename, 1);
	}

}
