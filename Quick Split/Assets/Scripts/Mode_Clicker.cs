using UnityEngine;
using System.Collections;

public class Mode_Clicker : MonoBehaviour {

	public int gameMode;

	void OnMouseOver()
	{
		gameObject.GetComponent<GUIText> ().color = Color.yellow;
		if (Input.GetMouseButtonDown (0)) {
			PlayerPrefs.SetInt("Mode", gameMode);
			Application.LoadLevel("Game Scene");
		}
	}

	void OnMouseExit()
	{
		gameObject.GetComponent<GUIText> ().color = Color.white;
	}
}
