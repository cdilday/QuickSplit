using UnityEngine;

public class PieceSplitterAssetHelper : MonoBehaviour
{
    //This script returns all the sprites and splitters that are currently active as they are called

    public Sprite[] RetroSprites = new Sprite[8];
    public Sprite[] ProgrammerSprites = new Sprite[8];
    public Sprite[] ArcaneSprites = new Sprite[8];
    public Sprite[] DefaultSprites = new Sprite[8];
    public Sprite[] BlobSprites = new Sprite[8];
    public Sprite[] DominoSprites = new Sprite[8];
    public Sprite[] PresentSprites = new Sprite[8];
    public Sprite[] PumpkinSprites = new Sprite[8];
    public Sprite[] SymbolSprites = new Sprite[8];
    public Sprite[] TechnoSprites = new Sprite[8];
    //don't ever use these this is a joke
    public Sprite[] FaceSprites = new Sprite[8];

    public Sprite[] Splitters;
    public RuntimeAnimatorController[] SplitterAnimations;

    public PieceSets PieceSet;
    public SplitterType SplitterType;

    public RuntimeAnimatorController[] DefaultAnimations = new RuntimeAnimatorController[8];
    public RuntimeAnimatorController[] ArcaneAnimations = new RuntimeAnimatorController[8];
    public RuntimeAnimatorController[] RetroAnimations = new RuntimeAnimatorController[8];
    public RuntimeAnimatorController[] BlobAnimations = new RuntimeAnimatorController[8];
    public RuntimeAnimatorController[] DominoAnimations = new RuntimeAnimatorController[8];
    public RuntimeAnimatorController[] PresentAnimations = new RuntimeAnimatorController[8];
    public RuntimeAnimatorController[] PumpkinAnimations = new RuntimeAnimatorController[8];
    public RuntimeAnimatorController[] SymbolAnimations = new RuntimeAnimatorController[8];
    public RuntimeAnimatorController[] TechnoAnimations = new RuntimeAnimatorController[8];
    private Achievement_Script achievementHandler;

    // Use this for initialization
    private void Start()
    {
        achievementHandler = GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<Achievement_Script>();
        if (PieceSet == PieceSets.Error)
        {
            PieceSet = (PieceSets)PlayerPrefs.GetInt(Constants.PieceSetOption, (int)PieceSets.Default);
        }

        if (SplitterType == SplitterType.Error)
        {
            SplitterType = (SplitterType)PlayerPrefs.GetInt(Constants.SplitterTypeOption, (int)SplitterType.Default);
        }
    }

    public Sprite[] GetSprites()
    {
        PieceSet = (PieceSets)PlayerPrefs.GetInt(Constants.PieceSetOption, (int)PieceSets.Default);
        switch (PieceSet)
        {
            case PieceSets.Arcane:
                return ArcaneSprites;
            case PieceSets.Retro:
                return RetroSprites;
            case PieceSets.Programmer:
                return ProgrammerSprites;
            case PieceSets.Default:
                return DefaultSprites;
            case PieceSets.Blob:
                return BlobSprites;
            case PieceSets.Domino:
                return DominoSprites;
            case PieceSets.Present:
                return PresentSprites;
            case PieceSets.Pumpkin:
                return PumpkinSprites;
            case PieceSets.Symbol:
                return SymbolSprites;
            case PieceSets.Techno:
                return TechnoSprites;
            default:
                return DefaultSprites;
        }
    }

    //returns sprite for currently active splitter
    public Sprite GetSplitter()
    {
        if (achievementHandler == null)
        {
            achievementHandler = GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<Achievement_Script>();
        }

        int index = PlayerPrefs.GetInt(Constants.SplitterTypeOption, (int)SplitterType.Default);
        return Splitters[index];
    }

    //returns sprite for the splitter with the given integer index
    public Sprite GetSplitter(int newSplitterIndex)
    {
        return Splitters[newSplitterIndex];
    }
    //returns the array of animation controllers for the current pieceset
    public RuntimeAnimatorController[] GetPieceSetAnimations()
    {
        switch (PieceSet)
        {
            case PieceSets.Default:
                return DefaultAnimations;
            case PieceSets.Arcane:
                return ArcaneAnimations;
            case PieceSets.Retro:
                return RetroAnimations;
            case PieceSets.Programmer:
                return null;
            case PieceSets.Blob:
                return BlobAnimations;
            case PieceSets.Domino:
                return DominoAnimations;
            case PieceSets.Present:
                return PresentAnimations;
            case PieceSets.Pumpkin:
                return PumpkinAnimations;
            case PieceSets.Symbol:
                return SymbolAnimations;
            case PieceSets.Techno:
                return TechnoAnimations;
            default:
                return DefaultAnimations;
        }
    }

    public RuntimeAnimatorController Get_Splitter_Animation()
    {
        if (achievementHandler == null)
        {
            achievementHandler = GameObject.FindGameObjectWithTag("Achievement Handler").GetComponent<Achievement_Script>();
        }

        int index = PlayerPrefs.GetInt(Constants.SplitterTypeOption, (int)SplitterType.Default);
        return SplitterAnimations[index];
    }

}