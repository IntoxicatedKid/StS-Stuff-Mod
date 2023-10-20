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
using StSStuffMod.Cards;
using System.Reflection.Emit;

namespace StSStuffMod.Exhibits
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
                Appearance: AppearanceType.NonShop,
                Owner: "",
                LosableType: ExhibitLosableType.CantLose,
                Rarity: Rarity.Rare,
                Value1: 3,
                Value2: null,
                Value3: null,
                Mana: new ManaGroup() { Any = 0 },
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: 0,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },

                RelativeCards: new List<string>() { "StSNecronomicurse" }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSNecronomiconDef))]
        [UsedImplicitly]
        public sealed class StSNecronomicon : Exhibit
        {
            private bool Again = false;
            private Card card = null;
            private ManaGroup manaGroup = ManaGroup.Empty;
            private UnitSelector unitSelector = null;
            protected override void OnGain(PlayerUnit player)
            {
                //base.GameRun.Damage(/*((double)(base.GameRun.Player.Hp * base.Value1) / 100.0).RoundToInt()*/base.Value1, DamageType.HpLose, true, true, null);
                List<Card> list = new List<Card> { Library.CreateCard<StSNecronomicurse>() };
                GameRun.AddDeckCards(list, false, null);
            }
            protected override void OnEnterBattle()
            {
                HandleBattleEvent(Battle.Player.TurnStarting, delegate (UnitEventArgs _)
                {
                    Active = true;
                });
                ReactBattleEvent(Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsing));
                ReactBattleEvent(Battle.CardMoving, new EventSequencedReactor<CardMovingEventArgs>(OnCardMoving));
                ReactBattleEvent(Battle.CardExiling, new EventSequencedReactor<CardEventArgs>(OnCardExiling));
                ReactBattleEvent(Battle.CardRemoving, new EventSequencedReactor<CardEventArgs>(OnCardRemoving));
            }
            private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
            {
                if (Active && args.Card.CardType == CardType.Attack && args.Card != card && args.ConsumingMana.Amount >= Value1)
                {
                    Again = true;
                    card = args.Card;
                    manaGroup = args.ConsumingMana;
                    unitSelector = args.Selector;
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardMoving(CardMovingEventArgs args)
            {
                if (!Battle.BattleShouldEnd && Again && args.Card == card && !(args.SourceZone == CardZone.PlayArea && args.DestinationZone == CardZone.Hand))
                {
                    foreach (var battleAction in Play(args.Card, args))
                    {
                        yield return battleAction;
                    }
                }
            }
            private IEnumerable<BattleAction> OnCardExiling(CardEventArgs args)
            {
                if (!Battle.BattleShouldEnd && Again && args.Card == card)
                {
                    foreach (var battleAction in Play(args.Card, args))
                    {
                        yield return battleAction;
                    }
                }
            }
            private IEnumerable<BattleAction> OnCardRemoving(CardEventArgs args)
            {
                if (!Battle.BattleShouldEnd && Again && args.Card == card)
                {
                    foreach (var battleAction in Play(args.Card, args))
                    {
                        yield return battleAction;
                    }
                }
            }
            private IEnumerable<BattleAction> Play(Card Card, GameEventArgs args)
            {
                Again = false;
                Battle.MaxHand += 1;
                if (Battle.HandZone.Count >= Battle.MaxHand)
                {
                    Battle.MaxHand -= 1;
                    card = null;
                    manaGroup = ManaGroup.Empty;
                    unitSelector = null;
                    yield break;
                }
                NotifyActivating();
                args.CancelBy(this);
                yield return new MoveCardAction(Card, CardZone.Hand);
                Battle.MaxHand -= 1;
                if (Card.Zone == CardZone.Hand)
                {
                    if (unitSelector.Type == TargetType.SingleEnemy && !unitSelector.SelectedEnemy.IsAlive)
                    {
                        unitSelector = new UnitSelector(Battle.AllAliveEnemies.Sample(GameRun.BattleRng));
                    }
                    Battle.GainMana(manaGroup);
                    Helpers.FakeQueueConsumingMana(manaGroup);
                    yield return new UseCardAction(Card, unitSelector, manaGroup);
                    if (Card.Zone == CardZone.Hand && Card.IsExile)
                    {
                        yield return new ExileCardAction(Card);
                    }
                    else if (Card.Zone == CardZone.Hand)
                    {
                        yield return new MoveCardAction(Card, CardZone.Discard);
                    }
                }
                card = null;
                manaGroup = ManaGroup.Empty;
                unitSelector = null;
                Active = false;
            }
            protected override void OnLeaveBattle()
            {
                Active = false;
                Again = false;
                card = null;
                manaGroup = ManaGroup.Empty;
                unitSelector = null;
            }
        }
    }
}