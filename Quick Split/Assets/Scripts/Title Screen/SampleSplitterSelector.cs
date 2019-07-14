using UnityEngine;
using UnityEngine.UI;

public class SampleSplitterSelector : MonoBehaviour
{
    //recoloring the other splitter on the how to play screen
    public Image htpSplitter;

    public int index = 0;
    public Text headerText;
    private Image image;
    private ScoreAndAchievementHandler achievementHandler;
    private PieceSplitterAssetHelper pieceSpriteHolder;
    private SplitterType activeSplitter;

    // Use this for initialization
    private void Start()
    {
        gameObject.GetComponent<Image>().sprite = GameObject.Find("Piece Sprite Holder").GetComponent<PieceSplitterAssetHelper>().GetSplitter();
        pieceSpriteHolder = GameObject.Find("Piece Sprite Holder").GetComponent<PieceSplitterAssetHelper>();
        htpSplitter.sprite = pieceSpriteHolder.GetSplitter();

        achievementHandler = GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<ScoreAndAchievementHandler>();

        activeSplitter = (SplitterType)PlayerPrefs.GetInt(Constants.SplitterTypeOption, (int)SplitterType.Default);
        headerText.text = Splitter_Helper.Get_Splitter_Name(activeSplitter);
        index = (int) activeSplitter;

        image = gameObject.GetComponent<Image>();
    }

    public void LeftButton()
    {
        do
        {
            if (index <= 0)
            {
                index = Splitter_Helper.SplitterStrings.Length - 1;
            }
            else
            {
                index--;
            }
        } while (!achievementHandler.isSplitterUnlocked((SplitterType)index));

        activeSplitter = (SplitterType)index;
        PlayerPrefs.SetInt(Constants.SplitterTypeOption, index);
        headerText.text = Splitter_Helper.Get_Splitter_Name(activeSplitter);
        image.sprite = pieceSpriteHolder.GetSplitter(index);
        htpSplitter.sprite = pieceSpriteHolder.GetSplitter(index);
    }

    public void RightButton()
    {
        do
        {
            if (index >= Splitter_Helper.SplitterStrings.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        } while (!achievementHandler.isSplitterUnlocked((SplitterType)index));

        activeSplitter = (SplitterType)index;
        PlayerPrefs.SetInt(Constants.SplitterTypeOption, index);
        headerText.text = Splitter_Helper.Get_Splitter_Name(activeSplitter);
        image.sprite = pieceSpriteHolder.GetSplitter(index);
        htpSplitter.sprite = pieceSpriteHolder.GetSplitter(index);
    }

}