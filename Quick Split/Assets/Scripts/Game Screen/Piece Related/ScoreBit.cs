﻿using UnityEngine;

public class ScoreBit : MonoBehaviour
{

    //this handles the movement and function of each individual score bit

    public PieceColor bitColor;
    public Vector2 target;
    private Vector2 moveVector;
    private Color thisColor;
    private float prevMagnitude;
    private bool isReturning;
    private float acceleration = 0.5f;
    private float speed;
    public int value;
    private BitPool BitPool;
    private GameController gameController;
    private SpellHandler spellHandler;

    public Sprite[] sprites = new Sprite[8];

    // Use this for initialization
    private void Start()
    {
        GameObject BitPoolObject = GameObject.Find("Bit Pool");
        if (BitPoolObject == null)
        {
            Debug.LogError("ScoreBit Error: Cannot find the Bit Pool");
        }
        else
        {
            BitPool = BitPoolObject.GetComponent<BitPool>();
        }
        transform.position = BitPoolObject.transform.position;
        PieceSplitterAssetHelper spriteHolder = GameObject.Find("Piece Sprite Holder").GetComponent<PieceSplitterAssetHelper>();
        sprites = spriteHolder.GetSprites();
        gameObject.SetActive(false);

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (Game_Mode_Helper.ActiveRuleSet.HasSpells)
        {
            spellHandler = GameObject.Find("Spell Handler").GetComponent<SpellHandler>();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            if (gameController.gameOver)
            {
                target = GameObject.FindGameObjectWithTag("Splitter").transform.position;
            }
            transform.Translate(new Vector3(moveVector.x * speed * Time.deltaTime, moveVector.y * speed * Time.deltaTime));
            if (isReturning)
            {
                speed += acceleration;
                if (prevMagnitude < Vector2.Distance(transform.position, target))
                {
                    GameObject.Find("Score Text").BroadcastMessage("beginPulse");
                    endJourney();
                }
            }
            else
            {
                speed -= acceleration;
                if (speed <= 1.5)
                {
                    isReturning = true;
                    Vector2 heading = target - new Vector2(transform.position.x, transform.position.y);
                    float distance = heading.magnitude;
                    moveVector = new Vector2((heading / distance).x, (heading / distance).y);
                }
            }
            prevMagnitude = Vector2.Distance(transform.position, target);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Bit Receptor")
        {
            other.BroadcastMessage("beginPulse");
            endJourney();
        }
    }

    public void changeColor(PieceColor newColor)
    {
        bitColor = newColor;
        SpriteRenderer myRenderer = GetComponent<SpriteRenderer>();

        thisColor = Piece.PieceColorValues[(int)newColor];
        myRenderer.sprite = sprites[(int)newColor];

        gameObject.GetComponentInChildren<SpriteRenderer>().color = thisColor;
    }

    //call once it begins to handle motion of the bit and spell activation 
    public void beginJourney()
    {
        //reactivate this object
        gameObject.SetActive(true);
        isReturning = false;
        moveVector = new Vector2(Random.Range(-1f, 1f), (Random.Range(-1f, 1f)));
        speed = 10f;
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
    }

    //handles what should happen when the bit is finished
    public void endJourney()
    {
        if (value < 1)
        {
            value = 1;
        }

        gameController.score += value;
        gameController.updateScore();
        if (Game_Mode_Helper.ActiveRuleSet.HasSpells && !gameController.gameOver)
        {
            for (int i = 0; i < value; i++)
            {
                spellHandler.addBit(bitColor);
            }
        }

        BitPool.returnToPool(gameObject);
        gameObject.SetActive(false);
    }

}