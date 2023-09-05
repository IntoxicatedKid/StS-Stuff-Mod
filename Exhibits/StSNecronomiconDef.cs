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
                Appearance: AppearanceType.Anywhere,
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
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSNecronomiconDef))]
        [UsedImplicitly]
        public sealed class StSNecronomicon : Exhibit
        {
            protected override void OnGain(PlayerUnit player)
            {
                base.OnGain(player);
                base.GameRun.Damage(this.Value1, DamageType.HpLose, true, true, null);
                List<Card> list = new List<Card> { Library.CreateCard<StSNecronomicurse>() };
                base.GameRun.AddDeckCards(list, false, null);
            }
            protected override void OnEnterBattle()
            {
                base.HandleBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, delegate (UnitEventArgs _)
                {
                    base.Active = true;
                });
                base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            }
            private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
            {
                if (base.Active && (args.Card.CardType == CardType.Attack) && (args.ConsumingMana.Amount >= base.Value2) && !base.Battle.BattleShouldEnd)
                {
                    base.NotifyActivating();
                    base.Active = false;
                    args.ConsumingMana = new ManaGroup() { Any = 0 };
                    yield return new MoveCardAction(args.Card, CardZone.Hand);
                    yield return new UseCardAction(args.Card, args.Selector, this.Mana);
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