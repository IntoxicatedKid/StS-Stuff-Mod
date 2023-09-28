using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using static StSStuffMod.BepinexPlugin;
using UnityEngine;
using LBoL.Core;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Base.Extensions;
using System.Collections;
using LBoL.Presentation;
using LBoL.EntityLib.Cards.Neutral.Blue;
using Mono.Cecil;
using LBoL.Core.StatusEffects;
using System.Linq;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Randoms;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Other.Misfortune;
using static UnityEngine.TouchScreenKeyboard;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using LBoL.Presentation.UI.Panels;
using UnityEngine.InputSystem.Controls;
using LBoL.EntityLib.Exhibits;
using JetBrains.Annotations;
using Yarn;

namespace StSStuffMod.Exhibits
{
    public sealed class StSVelvetChokerDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSVelvetChoker);
        }
        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.ExhibitsEn.yaml");
            return locFiles;
        }
        public override ExhibitSprites LoadSprite()
        {
            // embedded resource folders are separated by a dot
            var folder = "";
            var exhibitSprites = new ExhibitSprites();
            Func<string, Sprite> wrap = (s) => ResourceLoader.LoadSprite(folder + GetId() + s + ".png", embeddedSource);
            exhibitSprites.main = wrap("");
            return exhibitSprites;
        }
        public override ExhibitConfig MakeConfig()
        {
            var exhibitConfig = new ExhibitConfig(
                Index: sequenceTable.Next(typeof(ExhibitConfig)),
                Id: "",
                Order: 10,
                IsDebug: false,
                IsPooled: false,
                IsSentinel: false,
                Revealable: false,
                Appearance: AppearanceType.Nowhere,
                Owner: "",
                LosableType: ExhibitLosableType.CantLose,
                Rarity: Rarity.Shining,
                Value1: 6,
                Value2: 1,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: ManaColor.Philosophy,
                BaseManaAmount: 2,
                HasCounter: true,
                InitialCounter: 0,
                Keywords: Keyword.Overdraft,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSVelvetChokerDef))]
        [UsedImplicitly]
        [ExhibitInfo(ExpireStageLevel = 3, ExpireStationLevel = 0)]
        public sealed class StSVelvetChoker : ShiningExhibit
        {
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsed));
                HandleBattleEvent(Battle.Player.TurnEnding, delegate (UnitEventArgs _)
                {
                    Counter = 0;
                });
            }
            private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
            {
                if (!Battle.BattleShouldEnd && Owner.IsInTurn)
                {
                    Counter = Counter + 1;
                    if (Counter > Value1)
                    {
                        yield return new LockRandomTurnManaAction(Value2);
                    }
                }
                yield break;
            }
            protected override void OnLeaveBattle()
            {
                Counter = 0;
            }
        }
    }
}