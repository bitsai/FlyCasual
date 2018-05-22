﻿using ActionsList;
using Arcs;
using GameModes;
using Movement;
using Ship;
using SquadBuilderNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Upgrade;

namespace RuleSets
{
    interface ISecondEditionShip
    {
        void AdaptShipToSecondEdition();
    }

    interface ISecondEditionPilot
    {
        void AdaptPilotToSecondEdition();
    }

    interface ISecondEditionUpgrade
    {
        void AdaptUpgradeToSecondEdition();
    }

    public class SecondEdition : RuleSet
    {
        public override string Name { get { return "Second Edition"; } }
        private bool IsSquadBuilderLocked = false;

        public override int MaxPoints { get { return 200; } }
        public override int MinShipCost { get { return 28; } }
        public override int MinShipsCount { get { return 1; } }
        public override int MaxShipsCount { get { return 8; } }
        public override string CombatPhaseName { get { return "Engagement"; } }
        public override Color MovementEasyColor { get { return new Color(0, 0.5f, 1); } }
        public override MovementComplexity IonManeuverComplexity { get { return MovementComplexity.Easy; } }

        //TODO: Change to SE cards
        public override Dictionary<Type, int> DamageDeckContent
        {
            get
            {
                return new Dictionary<Type, int>()
                {
                    { typeof(DamageDeckCardFE.DirectHit),           7 },
                    { typeof(DamageDeckCardFE.BlindedPilot),        2 },
                    { typeof(DamageDeckCardFE.DamagedCockpit),      2 },
                    { typeof(DamageDeckCardFE.DamagedEngine),       2 },
                    { typeof(DamageDeckCardFE.DamagedSensorArray),  2 },
                    { typeof(DamageDeckCardFE.LooseStabilizer),     2 },
                    { typeof(DamageDeckCardFE.MajorHullBreach),     2 },
                    { typeof(DamageDeckCardFE.ShakenPilot),         2 },
                    { typeof(DamageDeckCardFE.StructuralDamage),    2 },
                    { typeof(DamageDeckCardFE.ThrustControlFire),   2 },
                    { typeof(DamageDeckCardFE.WeaponsFailure),      2 },
                    { typeof(DamageDeckCardFE.ConsoleFire),         2 },
                    { typeof(DamageDeckCardFE.StunnedPilot),        2 },
                    { typeof(DamageDeckCardFE.MajorExplosion),      2 }
                };
            }
        }

        public override Dictionary<BaseSize, int> NegativeTokensToAffectShip
        {
            get
            {
                return new Dictionary<BaseSize, int>()
                {
                    { BaseSize.Small,   1 },
                    { BaseSize.Medium,  2 },
                    { BaseSize.Large,   3 }
                };
            }
        }

        public override Dictionary<string, string> PreGeneratedAiSquadrons
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "Vader + Mini + Swarm",  "{\"name\":\"Vader + Mini + Swarm\",\"faction\":\"imperial\",\"points\":200,\"version\":\"0.3.0\",\"pilots\":[{\"name\":\"darthvader\",\"points\":76,\"ship\":\"tieadv\",\"upgrades\":{\"system\":[\"firecontrolsystem\"],\"missile\":[\"clustermissiles\"],\"ept\":[\"sense\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Blue\"}}},{\"name\":\"blacksquadronace\",\"points\":31,\"ship\":\"tiefighter\",\"upgrades\":{\"ept\":[\"crackshot\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"name\":\"blacksquadronace\",\"points\":31,\"ship\":\"tiefighter\",\"upgrades\":{\"ept\":[\"crackshot\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"name\":\"blacksquadronace\",\"points\":31,\"ship\":\"tiefighter\",\"upgrades\":{\"ept\":[\"crackshot\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}},{\"name\":\"blacksquadronace\",\"points\":31,\"ship\":\"tiefighter\",\"upgrades\":{\"ept\":[\"crackshot\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Gray\"}}}],\"description\":\"Darth Vader + Fire-Control System + Cluster Missiles + Sense\nBlack Squadron Ace + Crack Shot\nBlack Squadron Ace + Crack Shot\nBlack Squadron Ace + Crack Shot\nBlack Squadron Ace + Crack Shot\"}" },
                    { "Boba + Joy + Zealot",  "{\"name\":\"Boba + Joy + Zealot\",\"faction\":\"scum\",\"points\":98,\"version\":\"0.3.0\",\"pilots\":[{\"name\":\"bobafett\",\"points\":46,\"ship\":\"firespray31\",\"upgrades\":{\"bomb\":[\"seismiccharges\"],\"crew\":[\"perceptivecopilot\"],\"illicit\":[\"inertialdampeners\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Boba Fett\"}}},{\"name\":\"joyrekkoff\",\"points\":32,\"ship\":\"protectoratestarfighter\",\"upgrades\":{\"torpedo\":[\"iontorpedoes\"],\"ept\":[\"fearless\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Protectorate Starfighter\"}}},{\"name\":\"zealousrecruit\",\"points\":20,\"ship\":\"protectoratestarfighter\",\"upgrades\":{},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Protectorate Starfighter\"}}}],\"description\":\"Boba Fett (Firespray-31) + Seismic Charges + Perceptive Copilot + Inertial Dampeners\nJoy Rekkoff + Ion Torpedoes + Fearless\nZealous Recruit\"}" },
                    { "Luke + Wedge + Gold", "{\"name\":\"Luke + Wedge + Gold\",\"faction\":\"rebel\",\"points\":181,\"version\":\"0.3.0\",\"pilots\":[{\"name\":\"lukeskywalker\",\"points\":69,\"ship\":\"xwing\",\"upgrades\":{\"mod\":[\"servomotorsfoils(attack)\"],\"amd\":[\"r2d2\"],\"torpedo\":[\"protontorpedoes\"],\"ept\":[\"instinctiveaim\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Red\"}}},{\"name\":\"wedgeantilles\",\"points\":81,\"ship\":\"xwing\",\"upgrades\":{\"mod\":[\"servomotorsfoils(attack)\",\"afterburners\"],\"ept\":[\"predator\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Red\"}}},{\"name\":\"goldsquadronveteran\",\"points\":31,\"ship\":\"ywing\",\"upgrades\":{\"turret\":[\"ioncannonturret\"],\"amd\":[\"r5astromech\"],\"bomb\":[\"protonbombs\"],\"ept\":[\"experthandling\"]},\"vendor\":{\"Sandrem.FlyCasual\":{\"skin\":\"Yellow\"}}}],\"description\":\"Luke Skywalker + Servomotor S-Foils (Attack) + R2-D2 + Proton Torpedoes + Instinctive Aim\nWedge Antilles + Servomotor S-Foils (Attack) + AfterBurners + Predator\nGold Squadron Veteran + Ion Cannon Turret + R5 Astromech + Proton Bombs + Expert Handling\"}" }
                };
            }
        }

        public override void EvadeDiceModification(DiceRoll diceRoll)
        {
            if (diceRoll.Blanks > 0)
            {
                diceRoll.ChangeOne(DieSide.Blank, DieSide.Success);
            }
            else if (diceRoll.Focuses > 0)
            {
                diceRoll.ChangeOne(DieSide.Focus, DieSide.Success);
            }
            else
            {
                Messages.ShowError("Evade Token is spent, but no effect");
            }
        }

        public override void ActionIsFailed(GenericShip ship, Type actionType)
        {
            Messages.ShowError("Action is failed and skipped");
            Phases.CurrentSubPhase.PreviousSubPhase.Resume();
            GameMode.CurrentGameMode.SkipButtonEffect();
        }

        public override bool ShipIsAllowed(GenericShip ship)
        {
            return ship.ShipRuleType == typeof(SecondEdition);
        }

        public override bool PilotIsAllowed(GenericShip ship)
        {
            return ship.PilotRuleType == typeof(SecondEdition);
        }

        public override void AdaptShipToRules(GenericShip ship)
        {
            if (ship is ISecondEditionShip)
            {
                (ship as ISecondEditionShip).AdaptShipToSecondEdition();
                ship.ShipRuleType = typeof(SecondEdition);
            }
        }

        public override void AdaptPilotToRules(GenericShip ship)
        {
            if (ship is ISecondEditionPilot)
            {
                (ship as ISecondEditionPilot).AdaptPilotToSecondEdition();
                ship.PilotRuleType = typeof(SecondEdition);
            }
        }

        public override void AdaptUpgradeToRules(GenericUpgrade upgrade)
        {
            if (upgrade is ISecondEditionUpgrade)
            {
                (upgrade as ISecondEditionUpgrade).AdaptUpgradeToSecondEdition();
                upgrade.Charges = upgrade.MaxCharges;

                upgrade.UpgradeRuleType = typeof(SecondEdition);
            }
        }

        public override void AdaptArcsToRules(GenericShip ship)
        {
            ship.ArcInfo.Arcs.Add(new ArcBullseye(ship.ShipBase));
        }

        public override bool WeaponHasRangeBonus()
        {
            return Combat.ChosenWeapon is PrimaryWeaponClass || (Combat.ChosenWeapon as GenericUpgrade).Types.Contains(UpgradeType.Cannon) || (Combat.ChosenWeapon as GenericUpgrade).Types.Contains(UpgradeType.Turret);
        }

        public override void SetShipBaseImage(GenericShip ship)
        {
            ship.SetShipBaseImageSecondEdition();
        }

        public override void RotateMobileFiringArc(ArcFacing facing)
        {
            Selection.ThisShip.ShowMobileFiringArcHighlight(facing);
        }

        public override void ActivateGenericUpgradeAbility(GenericShip host, List<UpgradeType> upgradeTypes)
        {
            if (upgradeTypes.Contains(UpgradeType.Turret))
            {
                host.ShipBaseArcsType = BaseArcsType.ArcMobile;
                host.InitializeShipBaseArc();

                // Temporary
                if (!host.PrintedActions.Any(n => n.GetType() == typeof(RotateArcAction))) host.PrintedActions.Add(new RotateArcAction());
            }
        }

        public override void BarrelRollTemplatePlanning()
        {
            (Phases.CurrentSubPhase as SubPhases.BarrelRollPlanningSubPhase).PerfromTemplatePlanningSecondEdition();
        }

        public override void ReloadAction()
        {
            ActionsList.ReloadAction.RestoreOneCharge();
        }

        public override void SquadBuilderIsOpened()
        {
            if (!IsSquadBuilderLocked) return;

            string squadName = "";

            switch (SquadBuilder.CurrentSquadList.SquadFaction)
            {
                case Faction.Rebel:
                    squadName = "Luke + Wedge + Gold";
                    break;
                case Faction.Imperial:
                    squadName = "Vader + Mini + Swarm";
                    break;
                case Faction.Scum:
                    squadName = "Boba + Joy + Zealot";
                    break;
            }

            SquadBuilder.CreateSquadFromImportedJson(PreGeneratedAiSquadrons[squadName], SquadBuilder.CurrentPlayer, delegate { });
        }
    }
}