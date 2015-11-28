using UnityEngine;
using System.Collections;

public class Spell_Tab : MonoBehaviour {

	/* Spell tabs are activated when the spell is ready in the handler
	 * they pop up on the bottom, hovering over them(or holding onto them)
	 * will spawn the explination text in the center
	 * clicking (or tapping quickly, maybe pulling up) will activate the power
	 */

	public string spellColor;

	Splitter_script splitter;

	SpellHandler spellHandler;
	GameObject DescCanvas;

	bool isReady;
	bool isTransitioning;

	bool wasTouching;

	float startTime;
	public float transitionLength;

	Piece_Sprite_Holder spriteHolder;
	public Sprite[] sprites = new Sprite[8];

	Vector3 activePos;
	Vector3 inActivePos;

	void Start () {
		activePos = transform.position;
		inActivePos = new Vector3 (transform.position.x, transform.position.y - 1.5f, -3.5f);
		transform.position = inActivePos;
		GameObject shObject = transform.parent.gameObject;
		if (shObject != null) {
			spellHandler = shObject.GetComponent<SpellHandler>();
		}
		isReady = false;
		DescCanvas = GameObject.Find ("Description Canvas");
		splitter = GameObject.FindGameObjectWithTag ("Splitter").GetComponent<Splitter_script> ();

		spriteHolder = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ();
		sprites = spriteHolder.Get_Sprites ();
		switch (spellColor){
		case "Red":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
			break;
		case "Orange":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
			break;
		case "Yellow":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
			break;
		case "Green":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[3];
			break;
		case "Blue":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[4];
			break;
		case "Purple":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[5];
			break;
		case "Cyan":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[6];
			break;
		case "White":
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[7];
			break;
		}
		wasTouching = false;
		isTransitioning = false;
	}

	void Update()
	{
		//mobile controls. Touch screens are too different to use fake mouse controls
		if (Application.isMobilePlatform) {
		
			bool isNotTouching = true;
			foreach (Touch poke in Input.touches) {

				//first check if it's on the boxcollider 2D
				Vector3 wp = Camera.main.ScreenToWorldPoint(poke.position);
				Vector2 touchPos = new Vector2(wp.x, wp.y);
				//TODO: Update this to work better when you get a new spell tab
				if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)){
					isNotTouching = false;
					//now check stage of the touch. If it's just happened, display the explination popup
					if( poke.phase == TouchPhase.Began){
						//popup the explination
						splitter.setState ("canShoot", false);
						DescCanvas.GetComponent<Spell_Descriptions> ().display (spellColor);
					}
					else if (poke.phase == TouchPhase.Ended){
						//activate the spell
						if(isReady){
							switch (spellColor) {
							case "Red":
								spellHandler.Redspell();
								break;
							case "Orange":
								spellHandler.Orangespell();
								break;
							case "Yellow":
								spellHandler.Yellowspell();
								break;
							case "Green":
								spellHandler.Greenspell();
								break;
							case "Blue":
								spellHandler.Bluespell();
								break;
							case "Purple":
								spellHandler.Purplespell();
								break;
							case "Cyan":
								spellHandler.Cyanspell();
								break;
							case "White":
								spellHandler.Whitespell();
								break;
							}
							isReady = false;
							isTransitioning = true;
							startTime = Time.time;
						}
					}
				}
		
			}
			if(wasTouching && isNotTouching){
				//hide popup
				splitter.setState ("canShoot", true);
				DescCanvas.GetComponent<Spell_Descriptions> ().hide ();
			}

			wasTouching = !isNotTouching;
		}
	}

	void FixedUpdate()
	{

		if (isTransitioning) {
			//going to active position
			if(isReady){
				// if transition should have ended
				if(Time.time > startTime + transitionLength){
					transform.position =  activePos;
					isTransitioning = false;
				}
				else{
					float t_point = (Time.time - startTime) / transitionLength;
					transform.position = new Vector3(Mathf.SmoothStep(inActivePos.x, activePos.x, t_point),
					                                 Mathf.SmoothStep(inActivePos.y, activePos.y, t_point), -3.5f);
				}
			}
			// going to inactive position
			else{
				// if transition should have ended
				if(Time.time > startTime + transitionLength){
					transform.position = inActivePos;
					isTransitioning = false;
				}
				else{
					float t_point = (Time.time - startTime) / transitionLength;
					transform.position = new Vector3(Mathf.SmoothStep(activePos.x, inActivePos.x, t_point),
					                                 Mathf.SmoothStep(activePos.y, inActivePos.y, t_point), -3.5f);
				}
			}
			
		}

		if(!isReady)
		{
			bool checkColor;
			switch (spellColor) {
			case "Red":
				checkColor = spellHandler.redReady;
				break;
			case "Orange":
				checkColor = spellHandler.orangeReady;
				break;
			case "Yellow":
				checkColor = spellHandler.yellowReady;
				break;
			case "Green":
				checkColor = spellHandler.greenReady;
				break;
			case "Blue":
				checkColor = spellHandler.blueReady;
				break;
			case "Purple":
				checkColor = spellHandler.purpleReady;
				break;
			case "Cyan":
				checkColor = spellHandler.cyanReady;
				break;
			case "White":
				checkColor = spellHandler.whiteReady;
				break;
			default:
				checkColor = false;
				break;
			}
			if (checkColor) {
				isReady = true;
				isTransitioning = true;
				startTime = Time.time;
			}
		}



	}

	void OnMouseOver()
	{
		//prevent mouse controls from firing when you click on the tab
		if(!Application.isMobilePlatform){
			splitter.setState ("canShoot", false);
			DescCanvas.GetComponent<Spell_Descriptions> ().display (spellColor);
		}
	}
	
	void OnMouseExit()
	{
		if(!Application.isMobilePlatform){
			splitter.setState ("canShoot", true);
			DescCanvas.GetComponent<Spell_Descriptions> ().hide ();
		}
	}

	void OnMouseDown()
	{
		if(isReady && !spellHandler.spellWorking){
			switch (spellColor) {
			case "Red":
				spellHandler.Redspell();
				break;
			case "Orange":
				spellHandler.Orangespell();
				break;
			case "Yellow":
				spellHandler.Yellowspell();
				break;
			case "Green":
				spellHandler.Greenspell();
				break;
			case "Blue":
				spellHandler.Bluespell();
				break;
			case "Purple":
				spellHandler.Purplespell();
				break;
			case "Cyan":
				spellHandler.Cyanspell();
				break;
			case "White":
				spellHandler.Whitespell();
				break;
			}
			isReady = false;
			isTransitioning = true;
			startTime = Time.time;
		}
	}

}
