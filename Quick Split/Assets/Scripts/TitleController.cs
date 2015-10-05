using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Load_Game(int gameMode)
	{
		PlayerPrefs.SetInt("Mode", gameMode);
		Application.LoadLevel("Game Scene");
	}
}
