using UnityEngine;
using System.Collections;

public class Sidebar_Script : MonoBehaviour {

	public Sprite[] Sequential_Lights;
	public Sprite[] Colored_Lights;
	public Sprite[] Flashing_Lights;

	GameController gameController;

	string activeGameMode;

	SpriteRenderer spriteRenderer;

	int lightStage;
	bool isFlashing;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		activeGameMode = gameController.gameType;
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.sprite = Sequential_Lights [0];
		lightStage = 0;
		isFlashing = false;
	}

	void FixedUpdate()
	{
		if (isFlashing) {
			int seconds = (int) Time.time;
			float tempTime = Time.time - seconds;
			if(tempTime <0.25f || (tempTime >= 0.5f && tempTime <0.75f))
			{
				spriteRenderer.sprite = Flashing_Lights[0];
			}
			else
			{
				spriteRenderer.sprite = Flashing_Lights[1];
			}
		}
	}
	
	public void Increment_Lights()
	{
		if (activeGameMode != "Wit") {
			if(lightStage == Sequential_Lights.Length - 1)
				isFlashing = true;
			else
			{
				lightStage++;
				spriteRenderer.sprite = Sequential_Lights[lightStage];
			}
		}
	}

	public void Reset()
	{
		isFlashing = false;
		lightStage = 0;
		spriteRenderer.sprite = Sequential_Lights[lightStage];
	}
}
