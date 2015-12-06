using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Achievement_Tally : MonoBehaviour {

	public Text UnlockedText;
	public Text OutOfText;

	Achievement_Script achievementHandler;

	void OnEnable () {
		if (achievementHandler == null) {
			achievementHandler = GameObject.Find ("Achievement Handler").GetComponent<Achievement_Script> ();
		}
		//minus 3 because the default pieceset, splitter, and symbol piecesets are unlocked at the start
		int totalTally = achievementHandler.splittersUnlocked.Length + achievementHandler.piecesetsUnlocked.Length - 3;
		int unlockedTally = - 3;
		foreach (bool isUnlocked in achievementHandler.splittersUnlocked) {
			if(isUnlocked)
				unlockedTally++;
		}
		foreach (bool isUnlocked in achievementHandler.piecesetsUnlocked) {
			if(isUnlocked)
				unlockedTally++;
		}
		UnlockedText.text = unlockedTally.ToString();
		OutOfText.text = totalTally.ToString();

	}
}
