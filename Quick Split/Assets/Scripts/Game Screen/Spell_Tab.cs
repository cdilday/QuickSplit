using UnityEngine;
using UnityEngine.UI;

public class Spell_Tab : MonoBehaviour
{

    /* Spell tabs are activated when the spell is ready in the handler
	 * they pop up on the bottom, hovering over them(or holding onto them)
	 * will spawn the explination text in the center
	 * clicking (or tapping quickly, maybe pulling up) will activate the power
	 */

    public PieceColor spellColor;
    private Splitter splitter;
    private SpellHandler spellHandler;
    private GameObject DescCanvas;
    private bool isReady;
    private bool isTransitioning;
    private bool wasTouching;
    private float startTime;
    public float transitionLength;
    private PieceSplitterAssetHelper spriteHolder;
    public Sprite[] sprites = new Sprite[8];
    private Vector3 activePos;
    private Vector3 inActivePos;
    private int trackingID = -1;
    private Image imageRenderer;
    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        activePos = transform.position;
        inActivePos = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
        transform.position = inActivePos;
        GameObject shObject = GameObject.Find("Spell Handler");
        if (shObject != null)
        {
            spellHandler = shObject.GetComponent<SpellHandler>();
        }
        isReady = false;
        DescCanvas = GameObject.Find("Description Canvas");
        splitter = GameObject.FindGameObjectWithTag("Splitter").GetComponent<Splitter>();

        spriteHolder = GameObject.Find("Piece Sprite Holder").GetComponent<PieceSplitterAssetHelper>();
        sprites = spriteHolder.GetSprites();

        imageRenderer = GetComponent<Image>();

        imageRenderer.sprite = sprites[(int)spellColor];

        wasTouching = false;
        isTransitioning = false;
    }

    private void Update()
    {
        //mobile controls. Touch screens are too different to use fake mouse controls
        if (gameController.isPaused)
        {
            return;
        }

        if (Application.isMobilePlatform || splitter.mobileDebugging)
        {

            bool isNotTouching = true;
            foreach (Touch poke in Input.touches)
            {

                //first check if it's on the boxcollider 2D
                Vector3 wp = Camera.main.ScreenToWorldPoint(poke.position);
                Vector2 touchPos = new Vector2(wp.x, wp.y);
                if (poke.fingerId == trackingID)
                {
                    //activate if the y position is too high
                    if (touchPos.y > 0.1f)
                    {
                        if (isReady)
                        {
                            switch (spellColor)
                            {
                                case PieceColor.Red:
                                    spellHandler.Redspell();
                                    break;
                                case PieceColor.Orange:
                                    spellHandler.Orangespell();
                                    break;
                                case PieceColor.Yellow:
                                    spellHandler.Yellowspell();
                                    break;
                                case PieceColor.Green:
                                    spellHandler.Greenspell();
                                    break;
                                case PieceColor.Blue:
                                    spellHandler.Bluespell();
                                    break;
                                case PieceColor.Purple:
                                    spellHandler.Purplespell();
                                    break;
                                case PieceColor.Cyan:
                                    spellHandler.Cyanspell();
                                    break;
                                case PieceColor.White:
                                    spellHandler.Whitespell();
                                    break;
                            }
                            isReady = false;
                            isTransitioning = true;
                            startTime = Time.time;
                        }
                        trackingID = -1;
                    }
                    else
                    {
                        //if the touchphase has ended, reset position
                        if (poke.phase == TouchPhase.Ended)
                        {
                            trackingID = -1;
                            transform.position = activePos;
                            isNotTouching = true;
                        }
                        else
                        {
                            transform.position = new Vector3(transform.position.x, touchPos.y, transform.position.z);
                        }
                    }
                }
                if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                {
                    isNotTouching = false;
                    //now check stage of the touch. If it's just happened, display the explination popup
                    if (poke.phase == TouchPhase.Began)
                    {
                        //popup the explination
                        splitter.setState(Splitter.SplitterStates.canShoot, false);
                        DescCanvas.GetComponent<Spell_Descriptions>().display(spellColor);
                        trackingID = poke.fingerId;
                    }
                }
            }
            if (wasTouching && isNotTouching)
            {
                //hide popup
                splitter.setState(Splitter.SplitterStates.canShoot, true);
                DescCanvas.GetComponent<Spell_Descriptions>().hide();
            }

            wasTouching = !isNotTouching;
        }
    }

    private void FixedUpdate()
    {

        if (isTransitioning)
        {
            //going to active position
            if (isReady)
            {
                // if transition should have ended
                if (Time.time > startTime + transitionLength)
                {
                    transform.position = activePos;
                    isTransitioning = false;
                }
                else
                {
                    float t_point = (Time.time - startTime) / transitionLength;
                    transform.position = new Vector3(Mathf.SmoothStep(inActivePos.x, activePos.x, t_point),
                                                     Mathf.SmoothStep(inActivePos.y, activePos.y, t_point), transform.position.z);
                }
            }
            // going to inactive position
            else
            {
                // if transition should have ended
                if (Time.time > startTime + transitionLength)
                {
                    transform.position = inActivePos;
                    isTransitioning = false;
                }
                else
                {
                    float t_point = (Time.time - startTime) / transitionLength;
                    transform.position = new Vector3(Mathf.SmoothStep(activePos.x, inActivePos.x, t_point),
                                                     Mathf.SmoothStep(activePos.y, inActivePos.y, t_point), transform.position.z);
                }
            }

        }

        if (!isReady)
        {
            bool checkColor = spellHandler.SpellReady[(int)spellColor];

            if (checkColor)
            {
                isReady = true;
                isTransitioning = true;
                startTime = Time.time;
            }
        }



    }

    private void OnMouseOver()
    {
        //prevent mouse controls from firing when you click on the tab
        if (!Application.isMobilePlatform && !splitter.mobileDebugging)
        {
            splitter.setState(Splitter.SplitterStates.canShoot, false);
            DescCanvas.GetComponent<Spell_Descriptions>().display(spellColor);
        }
    }

    private void OnMouseExit()
    {
        if (!Application.isMobilePlatform && !splitter.mobileDebugging)
        {
            splitter.setState(Splitter.SplitterStates.canShoot, true);
            DescCanvas.GetComponent<Spell_Descriptions>().hide();
        }
    }

    private void OnMouseDown()
    {
        //if everything's ready and it's not mobile controls, activate the spell
        if (isReady && !spellHandler.spellWorking && !Application.isMobilePlatform && !splitter.mobileDebugging)
        {
            switch (spellColor)
            {
                case PieceColor.Red:
                    spellHandler.Redspell();
                    break;
                case PieceColor.Orange:
                    spellHandler.Orangespell();
                    break;
                case PieceColor.Yellow:
                    spellHandler.Yellowspell();
                    break;
                case PieceColor.Green:
                    spellHandler.Greenspell();
                    break;
                case PieceColor.Blue:
                    spellHandler.Bluespell();
                    break;
                case PieceColor.Purple:
                    spellHandler.Purplespell();
                    break;
                case PieceColor.Cyan:
                    spellHandler.Cyanspell();
                    break;
                case PieceColor.White:
                    spellHandler.Whitespell();
                    break;
            }
            //tell it that it's just activated
            isReady = false;
            isTransitioning = true;
            startTime = Time.time;
        }
    }

}