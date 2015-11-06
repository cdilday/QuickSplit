using UnityEngine;
using System.Collections;

public class Mode_Clicker : MonoBehaviour {

	public int gameMode;
	public GUIText description;

	void Start(){
		if (transform.GetChild(0)) {
			description = transform.GetChild(0).GetComponent<GUIText> ();
		}
	}

	void OnMouseOver()
	{
		gameObject.GetComponent<GUIText> ().color = Color.yellow;
		description.color = Color.yellow;
		if (Input.GetMouseButtonDown (0)) {
			PlayerPrefs.SetInt("Mode", gameMode);
			Application.LoadLevel("Game Scene");
		}
	}

	void OnMouseExit()
	{
		gameObject.GetComponent<GUIText> ().color = Color.white;
		description.color = Color.white;
	}
}
