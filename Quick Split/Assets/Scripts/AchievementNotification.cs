using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Notification that appears when an achivement unlocks
/// </summary>
public class AchievementNotification : MonoBehaviour
{
    // Text that holds the achievement name
    public Text NameText;
    // Text that holds the achievement flavor text
    public Text UnlockText;

    //backdrop for the notifications
    private Image backGround;
    private ScoreAndAchievementHandler achievementHandler;
    private float startTime;

    //there are 4 stages. 0 is not active, 1 is fading in, 2 is idle but active, and 3 is fading out
    private int stage = 0;
    public bool isBusy;

    //how long the fading takes
    private float fadeDuration = 1.5f;

    //how long it stays onscreen between fadings
    private float waitDuration = 3f;
    private AudioSource AchievementSFX;

    // Use this for initialization
    private void Start()
    {
        achievementHandler = GameObject.Find("Achievement Handler").GetComponent<ScoreAndAchievementHandler>();
        achievementHandler.notification = GetComponent<AchievementNotification>();
        backGround = GetComponent<Image>();
        AchievementSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //if it's being used
        if (isBusy)
        {
            //fading in stage
            if (stage == 1)
            {
                //check if it is fully faded in
                if (Time.time - startTime > fadeDuration)
                {
                    stage = 2;
                    backGround.color = new Color(1, 1, 1, 1);
                    NameText.color = new Color(NameText.color.r, NameText.color.g, NameText.color.b, 1);
                    UnlockText.color = new Color(UnlockText.color.r, UnlockText.color.g, UnlockText.color.b, 1);
                    startTime = Time.time;
                }
                else
                {
                    //fade in alpha
                    float progress = (Time.time - startTime) / fadeDuration;
                    backGround.color = new Color(1, 1, 1, progress);
                    NameText.color = new Color(NameText.color.r, NameText.color.g, NameText.color.b, progress);
                    UnlockText.color = new Color(UnlockText.color.r, UnlockText.color.g, UnlockText.color.b, progress);
                }
            }
            //idle stage
            else if (stage == 2)
            {
                //check if the idle time has passed, then move on
                if (Time.time - startTime > waitDuration)
                {
                    stage = 3;
                    startTime = Time.time;
                }
            }
            //fading out stage
            else if (stage == 3)
            {
                //check if it should be fully faded out, than reset it for reuse
                if (Time.time - startTime > fadeDuration)
                {
                    stage = 0;
                    isBusy = false;
                    backGround.color = new Color(1, 1, 1, 0);
                    NameText.color = new Color(NameText.color.r, NameText.color.g, NameText.color.b, 0);
                    UnlockText.color = new Color(UnlockText.color.r, UnlockText.color.g, UnlockText.color.b, 0);
                }
                else
                {
                    //fade out alpha
                    float progress = (Time.time - startTime) / fadeDuration;
                    backGround.color = new Color(1, 1, 1, 1f - progress);
                    NameText.color = new Color(NameText.color.r, NameText.color.g, NameText.color.b, 1f - progress);
                    UnlockText.color = new Color(UnlockText.color.r, UnlockText.color.g, UnlockText.color.b, 1f - progress);
                }
            }
        }

    }

    public void AchievementUnlocked(SplitterType splitterUnlocked)
    {
        //first check to make sure it's not in use. if it is, just play the sound so they know something happened
        if (isBusy || splitterUnlocked == SplitterType.Error)
        {
            return;
        }
        AchievementSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        AchievementSFX.Play();

        isBusy = true;

        UnlockText.text = Constants.NewSplitterUnlocked;
        NameText.text = Constants.SplitterUnlockDescriptions[(int)splitterUnlocked];
        

        //begin the fadin stage
        stage = 1;
        startTime = Time.time;
    }

    public void AchievementUnlocked(PieceSets pieceSetUnlocked)
    {
        //first check to make sure it's not in use. if it is, just play the sound so they know something happened
        if (isBusy || pieceSetUnlocked == PieceSets.Error)
        {
            return;
        }
        AchievementSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        AchievementSFX.Play();

        isBusy = true;

        UnlockText.text = Constants.NewPieceSetUnlocked;
        NameText.text = Constants.PieceSetUnlockDescriptions[(int)pieceSetUnlocked];

        //begin the fadin stage
        stage = 1;
        startTime = Time.time;
    }
}
