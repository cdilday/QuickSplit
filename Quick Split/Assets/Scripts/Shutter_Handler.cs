using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shutter_Handler : MonoBehaviour {

	//unfortunately magic numbers seems to be the best way to do this
	public Image ULShutter;
	Vector3 ULClosePos = new Vector3(-278.3f, 171.4f, 0);
	Vector3 ULOpenUpPos = new Vector3(-278.3f, 523f, 0);
	Vector3 ULOpenLeftPos = new Vector3(-835f, 171.4f, 0);
	Vector3 ULStart;
	Vector3 ULTarget;
	float ULSpeed;

	public Image URShutter;
	Vector3 URClosePos = new Vector3(278.3f, 171.4f, 0);
	Vector3 UROpenUpPos = new Vector3(278.3f, 523f, 0);
	Vector3 UROpenRightPos = new Vector3(829f, 171.4f, 0);
	Vector3 URStart;
	Vector3 URTarget;
	float URSpeed;

	public Image DRShutter;
	Vector3 DRClosePos = new Vector3(278.3f, -175.2f, 0);
	Vector3 DROpenDownPos = new Vector3(278.3f, -528f, 0);
	Vector3 DROpenRightPos = new Vector3(829f, -175.2f, 0);
	Vector3 DRStart;
	Vector3 DRTarget;
	float DRSpeed;

	public Image DLShutter;
	Vector3 DLClosePos = new Vector3(-278.3f, -175.2f, 0);
	Vector3 DLOpenDownPos = new Vector3(-278.3f, -528f, 0);
	Vector3 DLOpenLeftPos = new Vector3(-835f, -175.2f, 0);
	Vector3 DLStart;
	Vector3 DLTarget;
	float DLSpeed;
	
	bool isOpeningV = false;
	bool isClosingV = false;
	bool isOpeningH = false;
	bool isClosingH = false;
	bool inMotion = false;

	float StartTime;
	//move duration is how long the animation takes.
	float MoveDuration = 1f;

	void Start(){
		ULShutter.color = new Color (1, 1, 1, 1);
		URShutter.color = new Color (1, 1, 1, 1);
		DLShutter.color = new Color (1, 1, 1, 1);
		DRShutter.color = new Color (1, 1, 1, 1);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isOpeningH || isOpeningV || isClosingH || isClosingV) {
			inMotion = true;
		}
		else
		{
			inMotion = false;
		}

		if(inMotion){
			//let's start with the case that the time has passed
			if(Time.time >= StartTime + MoveDuration)
			{
				isOpeningV = false;
				isOpeningH = false;
				isClosingH = false;
				isClosingV = false;
				ULShutter.rectTransform.localPosition = ULTarget;
				URShutter.rectTransform.localPosition = URTarget;
				DRShutter.rectTransform.localPosition = DRTarget;
				DLShutter.rectTransform.localPosition = DLTarget;
			}
			else{
				//the shutter should be moving, time for some calculations per shutter
				//ULShutter
				float distCovered = (Time.time - StartTime) * ULSpeed;
				float fracJourney = distCovered / (ULSpeed * MoveDuration);
				ULShutter.rectTransform.localPosition = Vector3.Lerp (ULStart, ULTarget, fracJourney);
				//URShutter
				distCovered = (Time.time - StartTime) * URSpeed;
				fracJourney = distCovered / (URSpeed * MoveDuration);
				URShutter.rectTransform.localPosition = Vector3.Lerp (URStart, URTarget, fracJourney);
				//DRShutter
				distCovered = (Time.time - StartTime) * DRSpeed;
				fracJourney = distCovered / (DRSpeed * MoveDuration);
				DRShutter.rectTransform.localPosition = Vector3.Lerp (DRStart, DRTarget, fracJourney);
				//DLShutter
				distCovered = (Time.time - StartTime) * DLSpeed;
				fracJourney = distCovered / (DLSpeed * MoveDuration);
				DLShutter.rectTransform.localPosition = Vector3.Lerp (DLStart, DLTarget, fracJourney);
			}
		}

	}

	// opens by having the shutters go up and down
	public void Begin_Vertical_Close()
	{
		if (inMotion) {
			Debug.Log ("Shutters are busy, redo the timing so this is impossible");
			return;
		}
		isOpeningV = true;

		//begin by setting the starting position of all four shutters
		ULShutter.rectTransform.localPosition = ULOpenUpPos;
		URShutter.rectTransform.localPosition = UROpenUpPos;
		DLShutter.rectTransform.localPosition = DLOpenDownPos;
		DRShutter.rectTransform.localPosition = DROpenDownPos;

		//load the start positions for movement calculations later
		ULStart = ULShutter.rectTransform.localPosition;
		URStart = URShutter.rectTransform.localPosition;
		DRStart = DRShutter.rectTransform.localPosition;
		DLStart = DLShutter.rectTransform.localPosition;

		//next set the targets
		ULTarget = ULClosePos;
		URTarget = URClosePos;
		DRTarget = DRClosePos;
		DLTarget = DLClosePos;

		//calculate the speeds
		ULSpeed = Mathf.Abs (Vector3.Distance(ULStart, ULTarget) / MoveDuration);
		URSpeed = Mathf.Abs (Vector3.Distance(URStart, URTarget) / MoveDuration);
		DRSpeed = Mathf.Abs (Vector3.Distance(DRStart, DRTarget) / MoveDuration);
		DLSpeed = Mathf.Abs (Vector3.Distance(DLStart, DLTarget) / MoveDuration);

		//begin the timer
		StartTime = Time.time;
	}

	// opens by having the shutters go left and right
	public void Begin_Horizontal_Close()
	{
		if (inMotion) {
			Debug.Log ("Shutters are busy, redo the timing so this is impossible");
			return;
		}
		isOpeningH = true;

		//begin by setting the starting position of all four shutters
		ULShutter.rectTransform.localPosition = ULOpenLeftPos;
		URShutter.rectTransform.localPosition = UROpenRightPos;
		DLShutter.rectTransform.localPosition = DLOpenLeftPos;
		DRShutter.rectTransform.localPosition = DROpenRightPos;

		//load the start positions for movement calculations later
		ULStart = ULShutter.rectTransform.localPosition;
		URStart = URShutter.rectTransform.localPosition;
		DRStart = DRShutter.rectTransform.localPosition;
		DLStart = DLShutter.rectTransform.localPosition;
		
		//next set the targets
		ULTarget = ULClosePos;
		URTarget = URClosePos;
		DRTarget = DRClosePos;
		DLTarget = DLClosePos;

		//calculate the speeds
		ULSpeed = Mathf.Abs (Vector3.Distance(ULStart, ULTarget) / MoveDuration);
		URSpeed = Mathf.Abs (Vector3.Distance(URStart, URTarget) / MoveDuration);
		DRSpeed = Mathf.Abs (Vector3.Distance(DRStart, DRTarget) / MoveDuration);
		DLSpeed = Mathf.Abs (Vector3.Distance(DLStart, DLTarget) / MoveDuration);

		//begin the timer
		StartTime = Time.time;
	}

	// closes by having the shutters go to the up and down sides
	public void Begin_Vertical_Open()
	{
		if (inMotion) {
			Debug.Log ("Shutters are busy, redo the timing so this is impossible");
			return;
		}
		isClosingV = true;

		//begin by setting the starting position of all four shutters
		ULShutter.rectTransform.localPosition = ULClosePos;
		URShutter.rectTransform.localPosition = URClosePos;
		DLShutter.rectTransform.localPosition = DLClosePos;
		DRShutter.rectTransform.localPosition = DRClosePos;

		//load the start positions for movement calculations later
		ULStart = ULShutter.rectTransform.localPosition;
		URStart = URShutter.rectTransform.localPosition;
		DRStart = DRShutter.rectTransform.localPosition;
		DLStart = DLShutter.rectTransform.localPosition;
		
		//next set the targets
		ULTarget = ULOpenUpPos;
		URTarget = UROpenUpPos;
		DRTarget = DROpenDownPos;
		DLTarget = DLOpenDownPos;

		//calculate the speeds
		ULSpeed = Mathf.Abs (Vector3.Distance(ULStart, ULTarget) / MoveDuration);
		URSpeed = Mathf.Abs (Vector3.Distance(URStart, URTarget) / MoveDuration);
		DRSpeed = Mathf.Abs (Vector3.Distance(DRStart, DRTarget) / MoveDuration);
		DLSpeed = Mathf.Abs (Vector3.Distance(DLStart, DLTarget) / MoveDuration);

		//begin the timer
		StartTime = Time.time;
	}
	
	// Closes by having the shutters go to the left and right sides
	public void Begin_Horizontal_Open()
	{
		if (inMotion) {
			Debug.Log ("Shutters are busy, redo the timing so this is impossible");
			return;
		}
		isClosingH = true;

		//begin by setting the starting position of all four shutters
		ULShutter.rectTransform.localPosition = ULClosePos;
		URShutter.rectTransform.localPosition = URClosePos;
		DLShutter.rectTransform.localPosition = DLClosePos;
		DRShutter.rectTransform.localPosition = DRClosePos;

		//load the start positions for movement calculations later
		ULStart = ULShutter.rectTransform.localPosition;
		URStart = URShutter.rectTransform.localPosition;
		DRStart = DRShutter.rectTransform.localPosition;
		DLStart = DLShutter.rectTransform.localPosition;
		
		//next set the targets
		ULTarget = ULOpenLeftPos;
		URTarget = UROpenRightPos;
		DRTarget = DROpenRightPos;
		DLTarget = DLOpenLeftPos;

		//calculate the speeds
		ULSpeed = Mathf.Abs (Vector3.Distance(ULStart, ULTarget) / MoveDuration);
		URSpeed = Mathf.Abs (Vector3.Distance(URStart, URTarget) / MoveDuration);
		DRSpeed = Mathf.Abs (Vector3.Distance(DRStart, DRTarget) / MoveDuration);
		DLSpeed = Mathf.Abs (Vector3.Distance(DLStart, DLTarget) / MoveDuration);

		//begin the timer
		StartTime = Time.time;
	}
}
