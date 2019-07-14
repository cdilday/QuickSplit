using UnityEngine;

public class WedgesScript : MonoBehaviour
{
    //this script is for the wedges that make the splits on the splitter

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float animStartTime;
    private bool hasPlayedAnim = false;
    private SplitterType SplitterType;
    private int index;

    public Sprite[] IdleSprites;
    public RuntimeAnimatorController[] wedgeAnimators;
    private ScoreAndAchievementHandler achievementHandler;

    // Use this for initialization
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        achievementHandler = GameObject.Find("Achievement Handler").GetComponent<ScoreAndAchievementHandler>();
        SplitterType = (SplitterType)PlayerPrefs.GetInt(Constants.SplitterTypeOption, (int) SplitterType.Default);
        if (SplitterType == SplitterType.Programmer)
        {
            Destroy(gameObject);
            return;
        }
        index = (int)SplitterType;
        animator.runtimeAnimatorController = wedgeAnimators[index];
        spriteRenderer.sprite = IdleSprites[index];
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (hasPlayedAnim && animator.GetCurrentAnimatorStateInfo(0).length + animStartTime < Time.time)
        {
            float animPlayLength = gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * (1 / gameObject.GetComponent<Animator>().speed);
            if (animPlayLength + animStartTime < Time.time)
            {
                gameObject.GetComponent<Animator>().SetBool("isFiring", false);
                hasPlayedAnim = false;
                spriteRenderer.sprite = IdleSprites[index];
            }
        }
    }

    //begins the firing animations
    public void HasFired()
    {
        animator.SetBool("isFiring", true);
        animStartTime = Time.time;
        hasPlayedAnim = true;
    }

}