using UnityEngine;

public class SplitterChargedEffect : MonoBehaviour
{
    //This script is specifically for showing the yellow spell is ready on the splitter

    private Splitter splitter;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool prevCharged;
    private GameController gameController;

    // Use this for initialization
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (!Game_Mode_Helper.ActiveRuleSet.HasSpells)
        {
            Destroy(gameObject);
        }

        splitter = gameObject.GetComponentInParent<Splitter>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
        prevCharged = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (splitter.getState(Splitter.SplitterStates.yellowReady))
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
