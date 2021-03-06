﻿using System;
using System.Text;
using UnityEngine;

/// <summary>
/// Static cless that contains important functions involving checking game modes
/// </summary>
public static class Game_Mode_Helper
{
    public static readonly RuleSet WizRuleSet = new RuleSet(GameMode.Wiz, true, 15, 5, 77);
    public static readonly RuleSet QuickRuleSet = new RuleSet(GameMode.Quick, false, new TimeSpan(0, 0, 20), 4, 77);
    public static readonly RuleSet WitRuleSet = new RuleSet(GameMode.Wit, false, 0, 8, 0);
    public static readonly RuleSet HolyRuleSet = new RuleSet(GameMode.Holy, true, 15, 8, 0);

    public static RuleSet[] AllRuleSets =
    {
        WizRuleSet,
        QuickRuleSet,
        WitRuleSet,
        HolyRuleSet,
        null,
    };

    public static RuleSet ActiveRuleSet = WizRuleSet;

    public static RuleSet GetRuleSet(GameMode mode)
    {
        return AllRuleSets[(int)mode];
    }

    //returns true if the given game mode is unlocked
    public static bool isGamemodeUnlocked(GameMode gameMode)
    {
        if (PlayerPrefs.GetInt(gameMode + Constants.GameModeUnlockedPredicate, 0) == 0)
        {
            return false;
        }
        return true;
    }
}

public class RuleSet
{
    #region Public Fields

    public GameMode Mode;
    public bool HasSpells;

    public bool TimedCrunch;
    public TimeSpan TimePerCrunch;
    public bool TurnedCrunch;
    public int SplitsPerCrunch;

    public int UnlockedPieces;
    public int SplitsToUnlock;

    #endregion Public Fields

    #region Properties

    /// <summary>
    /// Whether or not this RuleSet uses the sides
    /// </summary>
    public bool UsesSides
    {
        get
        {
            return TimedCrunch || TurnedCrunch;
        }
    }

    #endregion Properties

    #region Constructors

    public RuleSet()
    {

    }

    /// <summary>
    /// Constructor for sides coming in via time passed (quicksplit). Passing MinValue for Timespan will assume there are no sides
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="hasSpells"></param>
    /// <param name="timePerCrunch"></param>
    /// <param name="unlockedPieces"></param>
    /// <param name="splitsToUnlock"></param>
    public RuleSet(GameMode mode, bool hasSpells, TimeSpan timePerCrunch, int unlockedPieces, int splitsToUnlock)
    {
        Mode = mode;
        HasSpells = hasSpells;
        TimedCrunch = timePerCrunch != null && timePerCrunch > TimeSpan.MinValue;
        TimePerCrunch = timePerCrunch;
        TurnedCrunch = false;
        UnlockedPieces = unlockedPieces;
        SplitsToUnlock = splitsToUnlock;
    }

    /// <summary>
    /// Constructor for sides coming in after a set amount of splits (wiz split). Using 0 will remove the sides
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="hasSpells"></param>
    /// <param name="splitsPerCrunch"></param>
    /// <param name="unlockedPieces"></param>
    /// <param name="splitsToUnlock"></param>
    public RuleSet(GameMode mode, bool hasSpells, int splitsPerCrunch, int unlockedPieces, int splitsToUnlock)
    {
        Mode = mode;
        HasSpells = hasSpells;
        TurnedCrunch = splitsPerCrunch > 0;
        SplitsPerCrunch = splitsPerCrunch;
        TimedCrunch = false;
        TimePerCrunch = TimeSpan.MinValue;
        UnlockedPieces = unlockedPieces;
        SplitsToUnlock = splitsToUnlock;
    }

    #endregion Constructors

    #region Public Methods

    public override string ToString()
    {
        if (Mode != GameMode.Custom)
        {
            return Mode.ToString();
        }

        StringBuilder sb = new StringBuilder();
        sb.Append(Mode.ToString());
        sb.Append(" ");
        sb.Append((HasSpells ? 1 : 0).ToString());
        sb.Append(" ");
        sb.Append((TimedCrunch ? 1 : 0).ToString());
        sb.Append(" ");
        if (TimedCrunch)
        {
            sb.Append(((int)TimePerCrunch.TotalSeconds).ToString());
            sb.Append(" ");
        }
        else
        {
            sb.Append("0 ");
        }

        sb.Append((TurnedCrunch ? 1 : 0).ToString());
        sb.Append(" ");

        if (TurnedCrunch)
        {
            sb.Append(SplitsPerCrunch.ToString());
            sb.Append(" ");
        }
        else
        {
            sb.Append("0 ");
        }

        sb.Append(UnlockedPieces);
        sb.Append(" ");

        sb.Append(SplitsToUnlock);

        return sb.ToString();
    }

    #endregion
}

/// <summary>
/// Enum for the kind of game mode
/// </summary>
public enum GameMode
{
    Wiz = 0,
    Quick = 1,
    Wit = 2,
    Holy = 3,
    Custom = 4,
}
