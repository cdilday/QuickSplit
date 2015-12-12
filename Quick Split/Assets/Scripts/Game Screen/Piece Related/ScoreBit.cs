using UnityEngine;
using System.Collections;

public class ScoreBit : MonoBehaviour {

	//this handles the movement and function of each individual score bit

	public string bitColor;
	public Vector2 target;
	Vector2 moveVector;
	Color thisColor;
	float prevMagnitude;
	bool isReturning;
	float acceleration = 0.5f;
	float speed;
	public int value;

	Bit_Pool BitPool;

	GameController gameController;

	bool spellActive = false;
	SpellHandler spellHandler;

	public bool charges;

	public Sprite[] sprites = new Sprite[8];

	// Use this for initialization
	void Start () {
		GameObject BitPoolObject = GameObject.Find ("Bit Pool");
		if (BitPoolObject == null) {
			Debug.LogError("ScoreBit Error: Cannot find the Bit Pool");
		}
		else
		{
			BitPool = BitPoolObject.GetComponent<Bit_Pool>();
		}
		transform.position = BitPoolObject.transform.position;
		Piece_Sprite_Holder spriteHolder = GameObject.Find ("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder> ();
		sprites = spriteHolder.Get_Sprites ();
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(gameObject.activeSelf){
			if (gameController.gameOver) {
				target = GameObject.FindGameObjectWithTag("Splitter").transform.position;
			}
			transform.Translate (new Vector3(moveVector.x *speed*Time.deltaTime, moveVector.y * speed * Time.deltaTime));
			if (isReturning) {
				speed += acceleration;
				if(prevMagnitude < Vector2.Distance (transform.position, target)){
					GameObject.Find("Score Text").BroadcastMessage("beginPulse");
					End_Journey();
				}
			} 
			else {
				speed -= acceleration;
				if (speed <= 1.5){
					isReturning = true;
					Vector2 heading = target - new Vector2( transform.position.x, transform.position.y);
					float distance = heading.magnitude;
					moveVector = new Vector2((heading/distance).x, (heading/distance).y);
				}
			} 
			prevMagnitude = Vector2.Distance (transform.position, target);
		}
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.tag == "Bit Receptor") {
			other.BroadcastMessage("beginPulse");
			End_Journey();
		}
	}

	public void changeColor(string newColor)
	{
		bitColor = newColor;
		SpriteRenderer myRenderer = GetComponent<SpriteRenderer> ();
		switch (newColor) {
		case "Red":
			thisColor = Color.red;
			myRenderer.sprite = sprites[0];
			break;
		case "Orange":
			thisColor = new Color(1f, 0.5f, 0f);
			myRenderer.sprite = sprites[1];
			break;
		case "Yellow":
			thisColor = Color.yellow;
			myRenderer.sprite = sprites[2];
			break;
		case "Green":
			thisColor = Color.green;
			myRenderer.sprite = sprites[3];
			break;
		case "Blue":
			thisColor = Color.blue;
			myRenderer.sprite = sprites[4];
			break;
		case "Purple":
			thisColor = new Color(0.6f, 0, 0.6f);
			myRenderer.sprite = sprites[5];
			break;
		case "Cyan":
			thisColor = Color.cyan;
			myRenderer.sprite = sprites[6];
			break;
		case "White":
			thisColor = Color.white;
			myRenderer.sprite = sprites[7];
			break;
		}
		gameObject.GetComponentInChildren<SpriteRenderer> ().color = thisColor;
	}

	//call once it begins to handle motion of the bit and spell activation 
	public void Begin_Journey()
	{
		//reactivate this object
		gameObject.SetActive (true);
		isReturning = false;
		moveVector = new Vector2 (Random.Range(-1f, 1f), (Random.Range(-1f, 1f)));
		speed = 10f;
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();
			if(gameController.gameType == "Holy" || gameController.gameType == "Wiz"){
				spellActive = true;
				spellHandler = GameObject.Find ("Spell Handler").GetComponent<SpellHandler>();
				if (spellHandler.spellActive){
					charges = false;
				}
				else
					charges = true;
			}
			else{
				spellActive = false;
			}
		}
	}

	//handles what should happen when the bit is finished
	void End_Journey(){
		if (value < 1)
			value = 1;
		if(!gameController.gameOver){
			gameController.score += value;
			gameController.updateScore();
			if (spellActive) {
				for(int i = 0; i < value; i++){
					spellHandler.addBit(bitColor);
				}
			}
		}
		BitPool.return_to_pool (gameObject);
		gameObject.SetActive (false);
	}

}