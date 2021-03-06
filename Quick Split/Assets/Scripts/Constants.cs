﻿using UnityEngine;

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
    public static readonly string GuideOption = "Guide";
    public static readonly string PlayedBeforeLookup = "Played Before";
    public static readonly string CustomModeMusic = "Custom Music";

    public static readonly string Splitter = "Splitter";
    public static readonly string PieceSet = "PieceSet";
    public static readonly string NewSplitterUnlocked = "New Splitter Unlocked!";
    public static readonly string NewPieceSetUnlocked = "New Piece Set Unlocked!";

    public static readonly string[] SplitterUnlockDescriptions =
    {
        "", // Default = 0, automatically unlocked
        "How did this work?", // Programmer = 1,
        "Some Candy for the Pain", // CandyCane = 2,
        "Get behind the line", // Caution = 3,
        "No Light; only darkness", // Dark = 4,
        "Not Reddy to die", // Red = 5,
        "Well Orange you clever?", // Orange = 6,
        "Nothing to Yellow-ver", // Yellow = 7,
        "Looking for Greener Pastures", // Green = 8,
        "In with the Blue", // Blue = 9,
        "Non-Violet Solutions", // Purple = 10,
        "I'll be Cyan you later", // Cyan = 11,
        "Cleaned up White away" // White = 12,
    };

    public static readonly string[] PieceSetUnlockDescriptions =
    {
        "", // Default = 0, always unlocked
        "You're a Wizard", // Arcane = 1,
        "8-bits of Splits", // Retro = 2,
        "Grey Areas", // Programmer = 3,
        "What a mess...", // Blob = 4,
        "A Crazy Contraption", // Domino = 5,
        "Not Exactly Re-gifting", // Present = 6,
        "Cheater Cheater Pumpkin-Eater", // Pumpkin = 7,
        "", // Symbol = 8, unlocked by default for colorblind players
        "Sleek Splits" // Techno = 9,
    };

    public static readonly string[] Hints =
    {
        "Having trouble telling pieces apart? Try a different set in the options menu!",
        "Spells take longer to charge the more you use them. Use them wisely",
        "The best time to earn points is at the start. Use the pieces on the sides to your advantage!",
        "Wit Split is all about piece management. Keep track of your next few pieces and take your time!",
        "Side pieces pulse after they shake. Pulsing means they'll enter the board only after your next move.",
        "Holy mode requires really smart use of your spells. Be careful.",
        "If you split-it into the void long enough, it splits you.",
        "The lights on the side of the grid will light up as the side pieces get closer to entering.",
        "Pieces cleared as a result of spells do not charge spells, but still add to your score.",
        "Cyan Spell Bombs take a moment to explode. Use that to pile on spillter pieces you don't want",
        "High Scores are only recorded on Game Overs. Splitters never Quit!",
        "Making combos over successive splits will save your multiplier. Use it to your advantage!",
        "You can turn the Splitter guides off in the options menu if you don't need it",
        "The music that plays in Custom Split can be set in the options menu. Pick your favorite!"
    };
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

public static class Splitter_Helper
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

public static class Piece_Set_Helper
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