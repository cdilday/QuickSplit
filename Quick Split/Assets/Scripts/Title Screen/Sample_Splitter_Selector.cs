using UnityEngine;
using UnityEngine.UI;

public class Sample_Splitter_Selector : MonoBehaviour
{
    //recoloring the other splitter on the how to play screen
    public Image htpSplitter;

    public int index = 0;
    public Text headerText;
    private Image image;
    private Achievement_Script achievementHandler;
    private Piece_Sprite_Holder pieceSpriteHolder;

    // Use this for initialization
    private void Start()
    {
        gameObject.GetComponent<Image>().sprite = GameObject.Find("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder>().Get_Splitter();
        pieceSpriteHolder = GameObject.Find("Piece Sprite Holder").GetComponent<Piece_Sprite_Holder>();
        htpSplitter.sprite = pieceSpriteHolder.Get_Splitter();

        achievementHandler = GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<Achievement_Script>();

        headerText.text = PlayerPrefs.GetString("Splitter Type", "Default");
        index = (int) SplitterHelper.Get_SplitterType_Enum(headerText.text);

        image = gameObject.GetComponent<Image>();
    }

    public void Left_Button()
    {
        do
        {
            if (index <= 0)
            {
                index = SplitterHelper.SplitterStrings.Length - 1;
            }
            else
            {
                index--;
            }
        } while (!achievementHandler.is_Splitter_Unlocked((SplitterType)index));

        string splitterStr = SplitterHelper.SplitterStrings[index];
        PlayerPrefs.SetString("Splitter Type", splitterStr);
        headerText.text = splitterStr;
        image.sprite = pieceSpriteHolder.Get_Splitter(index);
        htpSplitter.sprite = pieceSpriteHolder.Get_Splitter(index);
    }

    public void Right_Button()
    {
        do
        {
            if (index >= SplitterHelper.SplitterStrings.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        } while (!achievementHandler.is_Splitter_Unlocked((SplitterType)index));

        string splitterStr = SplitterHelper.SplitterStrings[index];
        PlayerPrefs.SetString("Splitter Type", splitterStr);
        headerText.text = splitterStr;
        image.sprite = pieceSpriteHolder.Get_Splitter(index);
        htpSplitter.sprite = pieceSpriteHolder.Get_Splitter(index);
    }

}