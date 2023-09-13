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
using LBoL.EntityLib.StatusEffects.Neutral.TwoColor;

namespace StSStuffMod
{
    public sealed class StSNecronomiconDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSNecronomicon);
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
            Func<string, Sprite> wrap = (s) => ResourceLoader.LoadSprite((folder + GetId() + s + ".png"), embeddedSource);
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
                Appearance: AppearanceType.NonShop,
                Owner: "",
                LosableType: ExhibitLosableType.CantLose,
                Rarity: Rarity.Rare,
                Value1: 15,
                Value2: 3,
                Value3: null,
                Mana: new ManaGroup() { Any = 0 },
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: 0,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                // example of referring to UniqueId of an entity without calling MakeConfig
                RelativeCards: new List<string>() { "StSNecronomicurse" }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSNecronomiconDef))]
        [UsedImplicitly]
        public sealed class StSNecronomicon : Exhibit
        {
            private bool Again;
            private Card card;
            private Card card2;
            private UnitSelector unitSelector;
            protected override void OnGain(PlayerUnit player)
            {
                //base.GameRun.Damage(/*((double)(base.GameRun.Player.Hp * base.Value1) / 100.0).RoundToInt()*/base.Value1, DamageType.HpLose, true, true, null);
                List<Card> list = new List<Card> { Library.CreateCard<StSNecronomicurse>() };
                base.GameRun.AddDeckCards(list, false, null);
            }
            protected override void OnEnterBattle()
            {
                this.Again = false;
                card = null;
                card2 = null;
                unitSelector = null;
                base.HandleBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, delegate (UnitEventArgs _)
                {
                    base.Active = true;
                });
                base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsing));
                base.ReactBattleEvent<CardMovingEventArgs>(base.Battle.CardMoving, new EventSequencedReactor<CardMovingEventArgs>(this.OnCardMoving));
                base.ReactBattleEvent<CardEventArgs>(base.Battle.CardExiling, new EventSequencedReactor<CardEventArgs>(this.OnCardExiling));
                base.ReactBattleEvent<CardEventArgs>(base.Battle.CardRemoving, new EventSequencedReactor<CardEventArgs>(this.OnCardRemoving));
            }
            private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
            {
                if (base.Active && args.Card.CardType == CardType.Attack && args.ConsumingMana.Amount >= base.Value2 && args.Card != card && args.Card != card2)
                {
                    this.Again = true;
                    card = args.Card;
                    unitSelector = args.Selector;
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardMoving(CardMovingEventArgs args)
            {
                if (!base.Battle.BattleShouldEnd && this.Again && args.Card == card && !(args.SourceZone == CardZone.PlayArea && args.DestinationZone == CardZone.Hand))
                {
                    this.Again = false;
                    if (base.Battle.MaxHand <= base.Battle.HandZone.Count)
                    {
                        yield break;
                    }
                    base.NotifyActivating();
                    args.CancelBy(this);
                    yield return new MoveCardAction(args.Card, CardZone.Hand);
                    if (args.Card.Zone == CardZone.Hand)
                    {
                        yield return new UseCardAction(args.Card, unitSelector, new ManaGroup() { Any = 0 });
                    }
                    base.Active = false;
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardExiling(CardEventArgs args)
            {
                if (!base.Battle.BattleShouldEnd && this.Again && args.Card == card)
                {
                    this.Again = false;
                    if (base.Battle.MaxHand <= base.Battle.HandZone.Count)
                    {
                        yield break;
                    }
                    base.NotifyActivating();
                    args.CancelBy(this);
                    yield return new MoveCardAction(args.Card, CardZone.Hand);
                    if (args.Card.Zone == CardZone.Hand)
                    {
                        yield return new UseCardAction(args.Card, unitSelector, new ManaGroup() { Any = 0 });
                    }
                    base.Active = false;
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardRemoving(CardEventArgs args)
            {
                if (!base.Battle.BattleShouldEnd && this.Again && args.Card == card)
                {
                    this.Again = false;
                    if (base.Battle.MaxHand <= base.Battle.HandZone.Count)
                    {
                        yield break;
                    }
                    base.NotifyActivating();
                    args.CancelBy(this);
                    yield return new MoveCardAction(args.Card, CardZone.Hand);
                    if (args.Card.Zone == CardZone.Hand)
                    {
                        yield return new UseCardAction(args.Card, unitSelector, new ManaGroup() { Any = 0 });
                    }
                    base.Active = false;
                }
                yield break;
            }
            protected override void OnLeaveBattle()
            {
                base.Active = false;
            }
        }
    }
}