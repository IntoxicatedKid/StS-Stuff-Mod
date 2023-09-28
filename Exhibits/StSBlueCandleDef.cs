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
using JetBrains.Annotations;
using LBoL.EntityLib.Cards.Character.Marisa;

namespace StSStuffMod.Exhibits
{
    public sealed class StSBlueCandleDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSBlueCandle);
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
                Appearance: AppearanceType.Anywhere,
                Owner: "",
                LosableType: ExhibitLosableType.Losable,
                Rarity: Rarity.Uncommon,
                Value1: 1,
                Value2: null,
                Value3: null,
                Mana: new ManaGroup() { Any = 0 },
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: 0,
                Keywords: Keyword.Forbidden | Keyword.Exile | Keyword.Morph | Keyword.Misfortune,
                RelativeEffects: new List<string>() { },

                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSBlueCandleDef))]
        [UsedImplicitly]
        public sealed class StSBlueCandle : Exhibit
        {
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(OnBattleStarted));
                ReactBattleEvent(Battle.CardsAddedToDrawZone, new EventSequencedReactor<CardsAddingToDrawZoneEventArgs>(OnCardsAddedToDrawZone));
                ReactBattleEvent(Battle.CardsAddedToHand, new EventSequencedReactor<CardsEventArgs>(OnCardsAdded));
                ReactBattleEvent(Battle.CardsAddedToDiscard, new EventSequencedReactor<CardsEventArgs>(OnCardsAdded));
                ReactBattleEvent(Battle.CardsAddedToExile, new EventSequencedReactor<CardsEventArgs>(OnCardsAdded));
                ReactBattleEvent(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsed));
            }
            private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
            {
                NotifyActivating();
                foreach (Card card in Battle.EnumerateAllCards())
                {
                    if (card.CardType == CardType.Misfortune)
                    {
                        card.NotifyChanged();
                        card.SetBaseCost(Mana);
                        card.IsExile = true;
                        card.IsForbidden = false;
                    }
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardsAddedToDrawZone(CardsAddingToDrawZoneEventArgs args)
            {
                yield return MisfortuneCardModify(args.Cards);
                yield break;
            }
            private IEnumerable<BattleAction> OnCardsAdded(CardsEventArgs args)
            {
                yield return MisfortuneCardModify(args.Cards);
                yield break;
            }
            private BattleAction MisfortuneCardModify(IEnumerable<Card> cards)
            {
                foreach (Card card in cards)
                {
                    if (card.CardType == CardType.Misfortune)
                    {
                        card.NotifyChanged();
                        card.SetBaseCost(Mana);
                        card.IsExile = true;
                        card.IsForbidden = false;
                    }
                }
                return null;
            }
            private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
            {
                if (args.Card.CardType == CardType.Misfortune)
                {
                    NotifyActivating();
                    yield return new DamageAction(Owner, Owner, DamageInfo.HpLose(Value1), "Instant", GunType.Single);
                }
                yield break;
            }
        }
    }
}