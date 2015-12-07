﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class High_Score_Calculator : MonoBehaviour {

	//High scores are stored using the lookup "<Gamemode> score <#>" where <Gamemode> is the short name for the mode and <#> is the score's rank

	public Text[] ScoreList;
	public VerticalScrollSnap Scopes;
	public HorizontalScrollSnap Modes;

	int currScope;
	int prevScope;
	int currMode;
	int prevMode;
	
	// Use this for initialization
	void Start () {
		currScope = 0;
		prevScope = -1;
		currMode = 0;
		prevMode = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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
			for(int i = 0; i < 15; i++)
				ScoreList[i].text = (i+1) + ". (not implemented)";
		}
	}

	public void Reset_All_Scores(){
		for (int g = 0; g < 4; g++) {
			for (int i = 0; i < 15; i++){
				PlayerPrefs.SetInt(Lookup_Game_Type(g) + " score " + i, 0);
			}
		}
		//TODO: either make this affect server stored scores or make a different way to reset those
	}

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


}