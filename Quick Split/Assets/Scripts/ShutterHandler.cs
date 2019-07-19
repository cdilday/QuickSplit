using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is used for the shutter objects that overlap scene transitions
/// unfortunately magic numbers seems to be the best way to do this
/// </summary>
public class ShutterHandler : MonoBehaviour
{
    public Image ULShutter;
    private Vector3 ULClosePos = new Vector3(-278.3f, 171.4f, 0);
    private Vector3 ULOpenUpPos = new Vector3(-278.3f, 523f, 0);
    private Vector3 ULOpenLeftPos = new Vector3(-835f, 171.4f, 0);
    private Vector3 ULStart;
    private Vector3 ULTarget;
    private float ULSpeed;

    public Image URShutter;
    private Vector3 URClosePos = new Vector3(278.3f, 171.4f, 0);
    private Vector3 UROpenUpPos = new Vector3(278.3f, 523f, 0);
    private Vector3 UROpenRightPos = new Vector3(829f, 171.4f, 0);
    private Vector3 URStart;
    private Vector3 URTarget;
    private float URSpeed;

    public Image DRShutter;
    private Vector3 DRClosePos = new Vector3(278.3f, -175.2f, 0);
    private Vector3 DROpenDownPos = new Vector3(278.3f, -528f, 0);
    private Vector3 DROpenRightPos = new Vector3(829f, -175.2f, 0);
    private Vector3 DRStart;
    private Vector3 DRTarget;
    private float DRSpeed;

    public Image DLShutter;
    private Vector3 DLClosePos = new Vector3(-278.3f, -175.2f, 0);
    private Vector3 DLOpenDownPos = new Vector3(-278.3f, -528f, 0);
    private Vector3 DLOpenLeftPos = new Vector3(-835f, -175.2f, 0);
    private Vector3 DLStart;
    private Vector3 DLTarget;
    private float DLSpeed;
    private bool isOpeningV = false;
    private bool isClosingV = false;
    private bool isOpeningH = false;
    private bool isClosingH = false;
    private bool inMotion = false;
    private float StartTime;

    //move duration is how long the animation takes.
    private float MoveDuration = 1.5f;

    public AudioSource OpeningSFX;
    public AudioSource ClosingSFX;

    private void Start()
    {
        ULShutter.color = new Color(1, 1, 1, 1);
        URShutter.color = new Color(1, 1, 1, 1);
        DLShutter.color = new Color(1, 1, 1, 1);
        DRShutter.color = new Color(1, 1, 1, 1);
    }

    // Update is called once per frame
    private void Update()
    {
        //check if it's currently doing something
        if (isOpeningH || isOpeningV || isClosingH || isClosingV)
        {
            inMotion = true;
        }
        else
        {
            inMotion = false;
        }

        //if it is currently doing something
        if (inMotion)
        {
            //let's start with the case that the time has passed
            if (Time.realtimeSinceStartup >= StartTime + MoveDuration)
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
            else
            {
                //the shutter should be moving, time for some calculations per shutter
                //ULShutter
                float distCovered = (Time.realtimeSinceStartup - StartTime) * ULSpeed;
                float fracJourney = distCovered / (ULSpeed * MoveDuration);
                ULShutter.rectTransform.localPosition = Vector3.Lerp(ULStart, ULTarget, fracJourney);
                //URShutter
                distCovered = (Time.realtimeSinceStartup - StartTime) * URSpeed;
                fracJourney = distCovered / (URSpeed * MoveDuration);
                URShutter.rectTransform.localPosition = Vector3.Lerp(URStart, URTarget, fracJourney);
                //DRShutter
                distCovered = (Time.realtimeSinceStartup - StartTime) * DRSpeed;
                fracJourney = distCovered / (DRSpeed * MoveDuration);
                DRShutter.rectTransform.localPosition = Vector3.Lerp(DRStart, DRTarget, fracJourney);
                //DLShutter
                distCovered = (Time.realtimeSinceStartup - StartTime) * DLSpeed;
                fracJourney = distCovered / (DLSpeed * MoveDuration);
                DLShutter.rectTransform.localPosition = Vector3.Lerp(DLStart, DLTarget, fracJourney);
            }
        }

    }

    /// <summary>
    /// Closes the shutters vertically (going from title to game scene)
    /// </summary>
    public void BeginVerticalClose()
    {
        isOpeningV = true;

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
        ULSpeed = Mathf.Abs(Vector3.Distance(ULStart, ULTarget) / MoveDuration);
        URSpeed = Mathf.Abs(Vector3.Distance(URStart, URTarget) / MoveDuration);
        DRSpeed = Mathf.Abs(Vector3.Distance(DRStart, DRTarget) / MoveDuration);
        DLSpeed = Mathf.Abs(Vector3.Distance(DLStart, DLTarget) / MoveDuration);

        //begin the timer
        StartTime = Time.realtimeSinceStartup;
        ClosingSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        ClosingSFX.Play();
    }

    /// <summary>
    /// Closes the horizontal closing of the shutters (Game scene to title)
    /// </summary>
    public void BeginHorizontalClose()
    {
        isOpeningH = true;

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
        ULSpeed = Mathf.Abs(Vector3.Distance(ULStart, ULTarget) / MoveDuration);
        URSpeed = Mathf.Abs(Vector3.Distance(URStart, URTarget) / MoveDuration);
        DRSpeed = Mathf.Abs(Vector3.Distance(DRStart, DRTarget) / MoveDuration);
        DLSpeed = Mathf.Abs(Vector3.Distance(DLStart, DLTarget) / MoveDuration);

        //begin the timer
        StartTime = Time.realtimeSinceStartup;

        ClosingSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        ClosingSFX.Play();
    }

    /// <summary>
    /// Opens the shutters vertically (Title screen load)
    /// </summary>
    public void BeginVerticalOpen()
    {
        if (inMotion)
        {
            Debug.Log("Shutters are busy closing, continuing for a scene transition");
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
        ULSpeed = Mathf.Abs(Vector3.Distance(ULStart, ULTarget) / MoveDuration);
        URSpeed = Mathf.Abs(Vector3.Distance(URStart, URTarget) / MoveDuration);
        DRSpeed = Mathf.Abs(Vector3.Distance(DRStart, DRTarget) / MoveDuration);
        DLSpeed = Mathf.Abs(Vector3.Distance(DLStart, DLTarget) / MoveDuration);

        //begin the timer
        StartTime = Time.realtimeSinceStartup;
        OpeningSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        OpeningSFX.Play();

    }

    /// <summary>
    /// Opens the shutter horizontally (Game Screen loaded)
    /// </summary>
    public void BeginHorizontalOpen()
    {
        if (inMotion)
        {
            Debug.Log("Shutters are busy, redo the timing so this is impossible");
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
        ULSpeed = Mathf.Abs(Vector3.Distance(ULStart, ULTarget) / MoveDuration);
        URSpeed = Mathf.Abs(Vector3.Distance(URStart, URTarget) / MoveDuration);
        DRSpeed = Mathf.Abs(Vector3.Distance(DRStart, DRTarget) / MoveDuration);
        DLSpeed = Mathf.Abs(Vector3.Distance(DLStart, DLTarget) / MoveDuration);

        //begin the timer
        StartTime = Time.realtimeSinceStartup;

        OpeningSFX.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);
        OpeningSFX.Play();
    }
}
