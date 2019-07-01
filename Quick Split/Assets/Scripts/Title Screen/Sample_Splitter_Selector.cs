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
        index = achievementHandler.Splitter_Lookup_Index_by_Name(headerText.text);

        image = gameObject.GetComponent<Image>();
    }

    public void Left_Button()
    {
        do
        {
            if (index <= 0)
            {
                index = achievementHandler.SplitterStrings.Length - 1;
            }
            else
            {
                index--;
            }
        } while (!achievementHandler.is_Splitter_Unlocked((Achievement_Script.SplittersEnum)index));

        PlayerPrefs.SetString("Splitter Type", achievementHandler.SplitterStrings[index]);
        headerText.text = achievementHandler.SplitterStrings[index];
        image.sprite = pieceSpriteHolder.Get_Splitter(index);
        htpSplitter.sprite = pieceSpriteHolder.Get_Splitter(index);
    }

    public void Right_Button()
    {
        do
        {
            if (index >= achievementHandler.SplitterStrings.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        } while (!achievementHandler.is_Splitter_Unlocked((Achievement_Script.SplittersEnum)index));

        PlayerPrefs.SetString("Splitter Type", achievementHandler.SplitterStrings[index]);
        headerText.text = achievementHandler.SplitterStrings[index];
        image.sprite = pieceSpriteHolder.Get_Splitter(index);
        htpSplitter.sprite = pieceSpriteHolder.Get_Splitter(index);
    }

}