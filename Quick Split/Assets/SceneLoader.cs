using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	public int sceneNumber;

	void OnMouseDown()
	{
		if(sceneNumber != null)
		{
			Application.LoadLevel (sceneNumber);
		}
		else{
			Debug.Log ("No levl to load entered in settings!");
		}
	}
}
