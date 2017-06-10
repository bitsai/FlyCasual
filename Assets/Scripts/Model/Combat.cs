﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatStep
{
    None,
    Attack,
    Defence
}

public static partial class Combat
{

    private static GameManagerScript Game;

    public static DiceRoll DiceRollAttack;
    public static DiceRoll DiceRollDefence;
    public static DiceRoll CurentDiceRoll;

    public static CombatStep AttackStep = CombatStep.None;

    public static Ship.GenericShip Attacker;
    public static Ship.GenericShip Defender;

    public static Upgrade.GenericSecondaryWeapon SecondaryWeapon;

    // Use this for initialization
    static Combat() {
        Game = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }
	
    public static void PerformAttack(Ship.GenericShip attacker, Ship.GenericShip defender)
    {
        Attacker = attacker;
        Defender = defender;
        InitializeAttack();

        PlayAttackSound();
        AttackDiceRoll();
    }

    private static void InitializeAttack()
    {
        Roster.AllShipsHighlightOff();

        AttackStep = CombatStep.Attack;
        CallAttackStartEvents();
        Selection.ActiveShip = Attacker;
        if (SecondaryWeapon != null) SecondaryWeapon.PayAttackCost();
    }

    private static void AttackDiceRoll()
    {
        ShowDiceResultMenu(ConfirmAttackDiceResults);

        DiceRoll DiceRollAttack;
        DiceRollAttack = new DiceRoll("attack", Attacker.GetNumberOfAttackDices(Defender));
        DiceRollAttack.Roll();

        DiceRollAttack.CalculateResults(AfterAttackDiceRoll);
    }

    private static void PlayAttackSound()
    {
        if (SecondaryWeapon != null)
        {
            PlaySoundByParameters("Sounds/Proton-Torpedoes", 1);
        }
        else
        {
            switch (Selection.ActiveShip.Type)
            {
                case "X-Wing":
                    PlaySoundByParameters("Sounds/XWing-Laser", 3);
                    break;
                case "TIE Fighter":
                    PlaySoundByParameters("Sounds/TIE-Fire", 2);
                    break;
                default:
                    break;
            }
        }
    }

    private static void PlaySoundByParameters(string path, int times)
    {
        for (int i = 0; i < times; i++)
        {
            AudioSource audio = Selection.AnotherShip.Model.GetComponents<AudioSource>()[i];
            audio.clip = (AudioClip)Resources.Load(path);
            audio.PlayDelayed(i*0.5f);
        }
    }

    private static void AfterAttackDiceRoll(DiceRoll attackDiceRoll)
    {
        Combat.DiceRollAttack = attackDiceRoll;
        CurentDiceRoll = DiceRollAttack;
        ShowDiceModificationButtons();
    }

    public static void PerformDefence(Ship.GenericShip attacker, Ship.GenericShip defender)
    {
        Attacker = attacker;
        Defender = defender;
        InitializeDefence();

        DefenceDiceRoll();
    }

    private static void InitializeDefence()
    {
        AttackStep = CombatStep.Defence;
        CallDefenceStartEvents();
        Selection.ActiveShip = Defender;
    }

    private static void DefenceDiceRoll()
    {
        ShowDiceResultMenu(ConfirmDefenceDiceResults);
        DiceRoll DiceRollDefence = new DiceRoll("defence", Defender.GetNumberOfDefenceDices(Attacker));
        DiceRollDefence.Roll();
        
        DiceRollDefence.CalculateResults(AfterDefenceDiceRoll);
    }

    private static void AfterDefenceDiceRoll(DiceRoll defenceDiceRoll)
    {
        Combat.DiceRollDefence = defenceDiceRoll;
        CurentDiceRoll = DiceRollDefence;
        ShowDiceModificationButtons();
    }

    public static void CalculateAttackResults(Ship.GenericShip attacker, Ship.GenericShip defender)
    {
        DiceRollAttack.CancelHits(DiceRollDefence.Successes);
        if (DiceRollAttack.Successes != 0)
        {
            defender.SufferDamage(DiceRollAttack);
        }
    }

    public static void CallAttackStartEvents()
    {
        Attacker.AttackStart();
        Defender.AttackStart();
    }

    public static void CallDefenceStartEvents()
    {
        Attacker.DefenceStart();
        Defender.DefenceStart();
    }

    public static void SelectWeapon(Upgrade.GenericSecondaryWeapon secondaryWeapon = null)
    {
        SecondaryWeapon = secondaryWeapon;
    }

    public static void ConfirmDiceResults()
    {
        switch (AttackStep)
        {
            case CombatStep.Attack:
                ConfirmAttackDiceResults();
                break;
            case CombatStep.Defence:
                ConfirmDefenceDiceResults();
                break;
        }
    }

    public static void ConfirmAttackDiceResults()
    {
        HideDiceResultMenu();

        PerformDefence(Selection.ThisShip, Selection.AnotherShip);
    }

    public static void ConfirmDefenceDiceResults()
    {
        HideDiceResultMenu();

        //TODO: Show compare results dialog
        CalculateAttackResults(Selection.ThisShip, Selection.AnotherShip);

        MovementTemplates.ReturnRangeRuler();

        if (Roster.NoSamePlayerAndPilotSkillNotAttacked(Selection.ThisShip))
        {
            Phases.CurrentSubPhase.Next();
        }

    }

}
