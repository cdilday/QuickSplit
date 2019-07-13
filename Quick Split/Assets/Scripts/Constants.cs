using UnityEngine;

public static class Constants
{
    public static readonly string TopScorePredicate = " score 0";
    public static readonly string ScoreLookup = " score ";
    public static readonly string GameModeUnlockedPredicate = " unlocked";
    public static readonly string SplitterUnlockedPredicate = " Splitter unlocked";
    public static readonly string PieceSetUnlockedPredicate = " Pieceset unlocked";

    public static readonly string SfxVolumeLookup = "SFX Volume";
    public static readonly string MusicVolumeLookup = "Music Volume";

    public static readonly string CustomHighScoreCountNumLookup = "Number of Custom Scores";
    public static readonly string CustomHighScoreRulesetLookup = "Custom Ruleset ";

    public static readonly string PieceSetOption = "Piece Set";
    public static readonly string SplitterTypeOption = "Splitter Type";
}

/// <summary>
/// Contains all the unlockable splitters and their associated indexes in any arrays that may have them (image arrays, for example)
/// </summary>
public enum SplitterType
{
    Error = -1,
    Default = 0,
    Programmer = 1,
    CandyCane = 2,
    Caution = 3,
    Dark = 4,
    Red = 5,
    Orange = 6,
    Yellow = 7,
    Green = 8,
    Blue = 9,
    Purple = 10,
    Cyan = 11,
    White = 12,
}

public static class SplitterHelper
{
    public static readonly string[] SplitterStrings =
    {
        "Default",
        "Programmer",
        "Candy Cane",
        "Caution",
        "Dark",
        "Red",
        "Orange",
        "Yellow",
        "Green",
        "Blue",
        "Purple",
        "Cyan",
        "White"
    };

    /// <summary>
    /// Returns the splitter name from given index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string Get_Splitter_Name(int index)
    {
        if (index < 0 || index >= SplitterStrings.Length)
        {
            Debug.Log("Get_Splitter_Name recieved Error value");
            return string.Empty;
        }

        return SplitterStrings[index];
    }

    /// <summary>
    /// Returns the splitter name from given enum
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string Get_Splitter_Name(SplitterType splitterType)
    {
        if (splitterType == SplitterType.Error)
        {
            Debug.Log("Get_Splitter_Name recieved Error value");
            return string.Empty;
        }

        return SplitterStrings[(int)splitterType];
    }

    /// <summary>
    /// Attempts to get the SplitterType enum from the given string
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static SplitterType Get_SplitterType_Enum(string name)
    {
        object splitterEnum = System.Enum.Parse(typeof(SplitterType), name.Replace(" ", string.Empty));
        if (splitterEnum == null)
        {
            return SplitterType.Error;
        }

        return (SplitterType)splitterEnum;
    }
}


public enum PieceSets
{
    Error = -1,
    Default = 0,
    Arcane = 1,
    Retro = 2,
    Programmer = 3,
    Blob = 4,
    Domino = 5,
    Present = 6,
    Pumpkin = 7,
    Symbol = 8,
    Techno = 9,
}

public static class PieceSetHelper
{
    public static readonly string[] PieceSetStrings =
    {
        "Default",
        "Arcane",
        "Retro",
        "Programmer",
        "Blob",
        "Domino",
        "Present",
        "Pumpkin",
        "Symbol",
        "Techno",
    };

    /// <summary>
    /// returns the name of the Pieceset at the given index
    /// </summary>
    /// <param name="index"> index</param>
    /// <returns></returns>
    public static string Get_Pieceset_Name(int index)
    {
        if (index < 0 || index >= PieceSetStrings.Length)
        {
            Debug.Log("Get_Pieceset_Name recieved Error value");
            return string.Empty;
        }

        return PieceSetStrings[index];
    }

    public static string Get_Pieceset_Name(PieceSets pieceSet)
    {
        if (pieceSet == PieceSets.Error)
        {
            Debug.Log("Get_Pieceset_Name recieved Error value");
            return string.Empty;
        }

        return PieceSetStrings[(int)pieceSet];
    }

    /// <summary>
    /// Returns the corresponding PieceSet Enum for the given name, the Error value if one doesn't exist
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static PieceSets Get_Pieceset_Enum(string name)
    {
        object splitterEnum = System.Enum.Parse(typeof(PieceSets), name.Replace(" ", string.Empty));
        if (splitterEnum == null)
        {
            return PieceSets.Error;
        }

        return (PieceSets)splitterEnum;
    }
}