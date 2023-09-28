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
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using LBoL.Presentation.UI.Panels;
using UnityEngine.InputSystem.Controls;
using JetBrains.Annotations;
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.Cards.Character.Cirno;

namespace StSStuffMod.Exhibits
{
    public sealed class StSOrangePelletsDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSOrangePellets);
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
                IsPooled: true,
                IsSentinel: false,
                Revealable: false,
                Appearance: AppearanceType.ShopOnly,
                Owner: "",
                LosableType: ExhibitLosableType.Losable,
                Rarity: Rarity.Common,
                Value1: 4,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: true,
                InitialCounter: 0,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSOrangePelletsDef))]
        [UsedImplicitly]
        public sealed class StSOrangePellets : Exhibit
        {
            private bool unknown = false;
            private bool attack = false;
            private bool defense = false;
            private bool skill = false;
            private bool ability = false;
            private bool friend = false;
            private bool tool = false;
            private bool status = false;
            private bool misfortune = false;
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsed));
                HandleBattleEvent(Battle.Player.TurnEnding, delegate (UnitEventArgs _)
                {
                    Counter = 0;
                    unknown = false;
                    attack = false;
                    defense = false;
                    skill = false;
                    ability = false;
                    friend = false;
                    tool = false;
                    status = false;
                    misfortune = false;
                });
            }
            private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
            {
                if (Owner.IsInTurn)
                {
                    if (args.Card.CardType == CardType.Unknown && !unknown)
                    {
                        unknown = true;
                        Counter += 1;
                    }
                    else if (args.Card.CardType == CardType.Attack && !attack)
                    {
                        attack = true;
                        Counter += 1;
                    }
                    else if (args.Card.CardType == CardType.Defense && !defense)
                    {
                        defense = true;
                        Counter += 1;
                    }
                    else if (args.Card.CardType == CardType.Skill && !skill)
                    {
                        skill = true;
                        Counter += 1;
                    }
                    else if (args.Card.CardType == CardType.Ability && !ability)
                    {
                        ability = true;
                        Counter += 1;
                    }
                    else if (args.Card.CardType == CardType.Friend && !friend)
                    {
                        friend = true;
                        Counter += 1;
                    }
                    else if (args.Card.CardType == CardType.Tool && !tool)
                    {
                        tool = true;
                        Counter += 1;
                    }
                    else if (args.Card.CardType == CardType.Status && !status)
                    {
                        status = true;
                        Counter += 1;
                    }
                    else if (args.Card.CardType == CardType.Misfortune && !misfortune)
                    {
                        misfortune = true;
                        Counter += 1;
                    }
                    if (Counter >= Value1)
                    {
                        NotifyActivating();
                        Counter = 0;
                        unknown = false;
                        attack = false;
                        defense = false;
                        skill = false;
                        ability = false;
                        friend = false;
                        tool = false;
                        status = false;
                        misfortune = false;
                        yield return new RemoveAllNegativeStatusEffectAction(Battle.Player);
                    }
                }
                yield break;
            }
            protected override void OnLeaveBattle()
            {
                Counter = 0;
                unknown = false;
                attack = false;
                defense = false;
                skill = false;
                ability = false;
                friend = false;
                tool = false;
                status = false;
                misfortune = false;
            }
        }
    }
}