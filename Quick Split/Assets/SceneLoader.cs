using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	public int sceneNumber;

	void OnMouseDown()
	{
		Application.LoadLevel (sceneNumber);
	}
}
