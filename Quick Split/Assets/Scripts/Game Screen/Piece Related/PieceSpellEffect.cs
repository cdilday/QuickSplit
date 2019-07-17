using System;
using UnityEngine;

//this script is used for spell effects that overlay the pieces

public class PieceSpellEffect : MonoBehaviour
{
    //this contains and handles all the animations and spell efects that happen on individual pieces

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private PieceColor activeSpellColor;
    private int spellStage;
    private bool purpleEnd;
    private bool orangeLeft = false;
    private bool orangeDead = false;
    public Sprite orangeMiddleSprite;
    private bool check = false;
    public bool lastPiece = false;
    private PieceColor spellColor;
    private Vector2 gridPos;
    private float startTime;
    private GameController gameController;
    private Piece piece;
    private SpellHandler spellHandler;

    public AudioSource TransformationSFX;
    public AudioSource CyanBombActiveSFX;
    public AudioSource CyanBombExplodeSFX;

    #region Private Methods

    // Use this for initialization
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        piece = gameObject.GetComponentInParent<Piece>();
        //no need for these if there are no spells
        if (Game_Mode_Helper.ActiveRuleSet.HasSpells == false)
        {
            Destroy(gameObject);
        }
        else
        {
            spellHandler = GameObject.Find("Spell Handler").GetComponent<SpellHandler>();
        }

        activeSpellColor = PieceColor.Empty;
        spellStage = 0;
        purpleEnd = false;

        spriteRenderer.sprite = null;
    }

    private void FixedUpdate()
    {
        switch (activeSpellColor)
        {
            case PieceColor.Empty:
                return;
            case PieceColor.Red:
                break;
            case PieceColor.Orange:
                orangeSpellUpdate();
                break;
            case PieceColor.Yellow:
                break;
            case PieceColor.Green:
            case PieceColor.Blue:
                greenBlueSpellUpdate();
                break;
            case PieceColor.Purple:
                purpleSpellUpdate();
                break;
            case PieceColor.Cyan:
                cyanSpellUpdate();
                break;
            case PieceColor.White:
                whiteSpellUpdate();
                break;
        }
    }

    private void orangeSpellUpdate()
    {
        //orange spell
        //these calculations need to happen the first frame after orange is activated
        if (spellStage == 0)
        {
            animator.Play("Orange Selected", -1, Time.time % animator.GetCurrentAnimatorStateInfo(0).length);
            spellStage = 1;
            //left pieces need to be activated twice; once to trigger the first animation, and again to load with the right choice
            if (orangeLeft == false)
            {
                //if the color is selected skip to the next stage
                spellStage = 2;
            }
        }
        //stage 2 marks the point where the animation should change 
        else if (spellStage == 2)
        {
            if (spriteRenderer.sprite == orangeMiddleSprite)
            {
                orangeLeft = false;
                spellStage = 3;
                animator.SetBool("Orange Active", false);
                startTime = Time.time;
            }
        }
        //stage 3 plays out the remainder of the animation, changes color, then cleans up
        else if (spellStage == 3)
        {
            //the color change
            if (!check && ((animator.GetCurrentAnimatorStateInfo(0).length / 2) + startTime < Time.time))
            {
                check = true;
                //delete pieces that were marked as uneven remainders
                if (orangeDead)
                {
                    Destroy(piece.gameObject);
                }
                else
                {
                    piece.ConvertColor(spellColor);
                    TransformationSFX.pitch = 1f + UnityEngine.Random.Range(-0.5f, 0.5f);
                    TransformationSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
                    TransformationSFX.Play();
                }
            }
            //the remainder of the animation
            else if (check && (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time))
            {
                activeSpellColor = PieceColor.Empty;
                spellStage = 0;
                animator.SetBool("inActive", true);
                spriteRenderer.sprite = null;
                check = false;
            }
        }
    }

    private void greenBlueSpellUpdate()
    {
                
        // Green and Blue spells are similar, and can therefore be combined in most aspects
        //this changes the color on the frame that the piece is behind the animation. check ensures it happens once
        if (!check && ((animator.GetCurrentAnimatorStateInfo(0).length / 2) + startTime < Time.time))
        {
            check = true;
            piece.ConvertColor(spellColor);
        }
        //this handles the resetting the piece back to its original state after the animation is finished
        else if (check && (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time))
        {
            activeSpellColor = PieceColor.Empty;
            animator.SetBool("Green Active", false);
            animator.SetBool("Blue Active", false);
            animator.SetBool("inActive", true);
            check = false;
            spriteRenderer.sprite = null;
            //do the board checks if the piece was the final one. This allows for bigger combos using the immediate multiplier
            if (lastPiece)
            {
                gameController.lazyBoardLoop();
                gameController.splitter.setState(Splitter.SplitterStates.isActive, true);
                lastPiece = false;
            }
        }
    }

    private void purpleSpellUpdate()
    {
        //purple spell
        //also relatively complicated, thought the traversal is handled in the spell handler
        //this checks when the first animation (scan) ends, and is when the animation changes
        if (!check && (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time))
        {
            check = true;
            startTime = Time.time;
            //delete and do the deletion animation
            if (piece.pieceColor == spellColor)
            {
                transform.SetParent(null);
                Destroy(piece.gameObject);
                animator.SetBool("inActive", false);
                animator.SetBool("Purple Fade", true);
                purpleEnd = true;
            }
            else
            {
                //nothing happened, play the shrinking animation
                animator.SetBool("Purple Active", false);
            }
        }
        //this simply plays out the animation chosen at the end of the last animation
        else if (check && (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time))
        {
            check = false;
            activeSpellColor = PieceColor.Empty;
            animator.SetBool("inActive", true);
            animator.SetBool("Purple Fade", false);
            animator.SetBool("Purple Active", false);
            spellColor = PieceColor.Empty;
            //if it's the final piece, do the board checks
            if (lastPiece)
            {
                StartCoroutine(GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<ScoreAndAchievementHandler>().PurpleSplitterChecker(gameController.Get_Danger_Pieces()));
                StartCoroutine(gameController.lazyBoardWaiter());
                spellHandler.spellWorking = false;
            }
            //if the piece was deleted, this effect is standalone and therefore uselees
            if (purpleEnd)
            {
                Destroy(gameObject);
            }
        }
    }

    private void cyanSpellUpdate()
    {
        //cyan spell
        if (spellStage > 1)
        {
            //the cyan spell requires stages due to its complex nature
            //stage 2 means that the piece has collided with the next piece
            if (spellStage == 2)
            {
                animator.SetBool("Cyan Active", false);
                spellStage = 3;
                CyanBombExplodeSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1) * 0.75f;
                CyanBombExplodeSFX.Play();
            }
            //stage 3 means the bomb is in the process of exploding and pieces need to be taken crae of
            else if (spellStage == 3)
            {
                //this takes care of the case when the bomb is in the process of exploding but the piece moves because of the sidebars
                if (piece != null)
                {
                    transform.position = new Vector3(piece.transform.position.x, piece.transform.position.y, transform.position.z);
                    gridPos = piece.gridPos;
                }
                //the frame where the pieces are properly deleted behind the animation
                if ((animator.GetCurrentAnimatorStateInfo(0).length / 1.75f) + startTime < Time.time)
                {
                    //cyan splitter cheevo
                    int numDeleted = 0;
                    for (int r = 0; r < 3; r++)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            //check to make sure it's a valid move
                            if ((int)(gridPos.x - 1 + r) >= 0 && (int)(gridPos.x - 1 + r) <= 7 && (int)gridPos.y - 1 + c >= 0 && (int)gridPos.y - 1 + c <= 15 &&
                               gameController.grid[(int)piece.gridPos.x - 1 + r, (int)piece.gridPos.y - 1 + c] != null)
                            {
                                Destroy(gameController.grid[(int)gridPos.x - 1 + r, (int)gridPos.y - 1 + c]);
                                gameController.grid[(int)gridPos.x - 1 + r, (int)gridPos.y - 1 + c] = null;
                                gameController.colorGrid[(int)gridPos.x - 1 + r, (int)gridPos.y - 1 + c] = PieceColor.Empty;
                                numDeleted++;
                            }
                        }
                    }
                    if (piece != null)
                    {
                        numDeleted++;
                        Destroy(piece.gameObject);
                    }
                    gameController.collapse();
                    spellStage = 4;

                    if (numDeleted >= 10)
                    {
                        GameObject.FindGameObjectWithTag("Achievement Handler").BroadcastMessage("CyanSplitterChecker", null, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            //the end of the animation, the spell effect is at this point a standalone object that is useless
            else if (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time)
            {
                spellHandler.spellWorking = false;
                Destroy(gameObject);
            }
        }
    }

    private void whiteSpellUpdate()
    {
        //this one just plays the animation, the spellhandler does the actual work.
        if (animator.GetCurrentAnimatorStateInfo(0).length + startTime < Time.time)
        {
            activeSpellColor = PieceColor.Empty;
            animator.SetBool("inActive", true);
            animator.SetBool("White Active", false);
            spriteRenderer.sprite = null;
        }
    }

    #endregion

    #region Public Methods

    //called to start the orange spell
    public void ActivateOrange(string action)
    {
        activeSpellColor = PieceColor.Orange;
        animator.SetBool("inActive", false);
        if (action == "left")
        {
            orangeLeft = true;
        }
        else if (action == "dead")
        {
            orangeDead = true;
        }
        else
        {
            // convert to PieceColor
            spellColor = (PieceColor)Enum.Parse(typeof(PieceColor), action, true);
        }
        //This requires multiple stages depending on whether these pieces are on the left or right side
        if (spellStage == 0)
        {
            animator.SetBool("Orange Active", true);
        }
        if (spellStage == 1)
        {
            spellStage = 2;
        }
    }

    //called to start the green spell
    public void ActivateGreen(PieceColor color)
    {
        activeSpellColor = PieceColor.Green;
        spellColor = color;
        startTime = Time.time;
        animator.SetBool("inActive", false);
        animator.SetBool("Green Active", true);
        TransformationSFX.pitch = 1f + UnityEngine.Random.Range(-0.5f, 0.5f);
        TransformationSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        TransformationSFX.Play();
    }

    //called to start the blue spell
    public void ActivateBlue(PieceColor color)
    {
        activeSpellColor = PieceColor.Blue;
        spellColor = color;
        startTime = Time.time;
        animator.SetBool("inActive", false);
        animator.SetBool("Blue Active", true);
        TransformationSFX.pitch = 1f + UnityEngine.Random.Range(-0.5f, 0.5f);
        TransformationSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        TransformationSFX.Play();
    }

    //called to start the purple spell
    public void ActivatePurple(PieceColor color)
    {
        activeSpellColor = PieceColor.Purple;
        spellColor = color;
        startTime = Time.time;
        animator.SetBool("inActive", false);
        animator.SetBool("Purple Active", true);
    }

    //called to start the cyan/cyan spell
    public void ActivateCyan()
    {
        //first stage is when the piece is still in splitter
        if (spellStage == 0)
        {
            animator.SetBool("inActive", false);
            animator.SetBool("Cyan Active", true);
            activeSpellColor = PieceColor.Cyan;
            spellStage = 1;
            CyanBombActiveSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1) * 0.5f;
            CyanBombActiveSFX.Play();
        }
        //second call tells it to explode
        else
        {
            transform.SetParent(null);
            startTime = Time.time;
            spellStage = 2;
            animator.SetBool("inActive", false);
            animator.SetBool("Cyan Active", true);
            gridPos = piece.gridPos;
        }
    }

    //called to start the White spell
    public void ActivateWhite()
    {
        activeSpellColor = PieceColor.White;
        animator.SetBool("inActive", false);
        animator.SetBool("White Active", true);
        startTime = Time.time;
    }

    #endregion

}