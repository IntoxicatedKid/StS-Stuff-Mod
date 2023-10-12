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
    public sealed class StSStrangeSpoonDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSStrangeSpoon);
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
                Rarity: Rarity.Uncommon,
                Value1: null,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.Exile,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSStrangeSpoonDef))]
        [UsedImplicitly]
        public sealed class StSStrangeSpoon : Exhibit
        {
            private Card card = null;
            private System.Random random = new System.Random();
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsing));
                ReactBattleEvent(Battle.CardExiling, new EventSequencedReactor<CardEventArgs>(OnCardExiling));
            }
            private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
            {
                if (args.Card.IsExile && args.Card.CardType != CardType.Status && args.Card.CardType != CardType.Misfortune)
                {
                    card = args.Card;
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardExiling(CardEventArgs args)
            {
                if (args.Card == card)
                {
                    if (random.Next(0, 2) != 0)
                    {
                        NotifyActivating();
                        args.CancelBy(this);
                        yield return new MoveCardAction(args.Card, CardZone.Discard);
                    }
                    card = null;
                }
                yield break;
            }
            protected override void OnLeaveBattle()
            {
                card = null;
            }
        }
    }
}