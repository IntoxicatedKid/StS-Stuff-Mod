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
using LBoL.EntityLib.Exhibits.Adventure;
using LBoL.EntityLib.Adventures.Common;
using LBoL.EntityLib.Adventures.Shared12;
using LBoL.EntityLib.Adventures.Stage1;

namespace StSStuffMod.Exhibits
{
    public sealed class StSSneckoEyeDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSSneckoEye);
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
                Appearance: AppearanceType.Anywhere,
                Owner: "",
                LosableType: ExhibitLosableType.CantLose,
                Rarity: Rarity.Shining,
                Value1: 2,
                Value2: null,
                Value3: null,
                Mana: new ManaGroup() { Any = 1 },
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSSneckoEyeDef))]
        [UsedImplicitly]
        public sealed class StSSneckoEye : ShiningExhibit
        {
            protected override void OnAdded(PlayerUnit player)
            {
                GameRun.DrawCardCount += Value1;
            }
            protected override void OnRemoved(PlayerUnit player)
            {
                GameRun.DrawCardCount -= Value1;
            }
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.CardDrawn, new EventSequencedReactor<CardEventArgs>(OnCardDrawn));
            }
            private IEnumerable<BattleAction> OnCardDrawn(CardEventArgs args)
            {
                Card card = args.Card;
                _costs = new UniqueRandomPool<int>(false)
                {
                    { 0, 1f },
                    { 1, 1f },
                    { 2, 1f },
                    { 3, 1f },
                    { 4, 1f },
                    { 5, 1f }
                }.SampleMany(GameRun.BattleRng, 6, true);
                for (int j = 0; j < 6; j++)
                {
                    switch (_costs[j])
                    {
                        case 0:
                            card.SetBaseCost(ManaGroup.Anys(j));
                            break;
                    }
                }
                yield break;
            }
            private int[] _costs;
        }
    }
}