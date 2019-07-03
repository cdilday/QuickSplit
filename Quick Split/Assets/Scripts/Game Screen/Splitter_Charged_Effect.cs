using UnityEngine;

public class Splitter_Charged_Effect : MonoBehaviour
{

    //This script is specifically for showing the yellow spell is ready on the splitter

    private Splitter_script splitter;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool prevCharged;
    private GameController gameController;

    // Use this for initialization
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (gameController.gameMode != GameMode.Wiz && gameController.gameMode != GameMode.Holy)
        {
            Destroy(gameObject);
        }

        splitter = gameObject.GetComponentInParent<Splitter_script>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
        prevCharged = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (splitter.getState("yellowReady"))
        {
            if (!prevCharged)
            {
                animator.SetBool("inActive", false);
                prevCharged = true;
            }
        }
        else if (prevCharged)
        {
            animator.SetBool("inActive", true);
            spriteRenderer.sprite = null;
            prevCharged = false;
        }
    }
}
