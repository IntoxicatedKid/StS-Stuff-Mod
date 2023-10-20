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
using LBoL.EntityLib.Exhibits.Common;
using LBoL.EntityLib.Exhibits.Shining;

namespace StSStuffMod.Exhibits
{
    public sealed class StSEmotionChipDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSEmotionChip);
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
                Rarity: Rarity.Rare,
                Value1: null,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: 0,
                Keywords: Keyword.FriendCard,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSEmotionChipDef))]
        [UsedImplicitly]
        [ExhibitInfo(WeighterType = typeof(StSEmotionChipWeighter))]
        public sealed class StSEmotionChip : Exhibit
        {
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.Player.DamageReceived, new EventSequencedReactor<DamageEventArgs>(OnPlayerDamageReceived));
                ReactBattleEvent(Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(OnPlayerTurnStarted));
            }
            private IEnumerable<BattleAction> OnPlayerDamageReceived(DamageEventArgs args)
            {
                if (args.DamageInfo.Damage > 0f)
                {
                    Active = true;
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
            {
                if (Active && !Battle.BattleShouldEnd)
                {
                    NotifyActivating();
                    List<Card> list = Battle.HandZone.Where((Card card) => card.CardType == CardType.Friend && card.Summoned).ToList();
                    foreach (Card card in list)
                    {
                        IEnumerable<BattleAction> passiveActions = card.GetPassiveActions();
                        if (passiveActions != null)
                        {
                            foreach (BattleAction battleAction in passiveActions)
                            {
                                yield return battleAction;
                            }
                        }
                    }
                }
                Active = false;
            }
            protected override void OnLeaveBattle()
            {
                Active = false;
            }
            private class StSEmotionChipWeighter : IExhibitWeighter
            {
                public float WeightFor(Type type, GameRunController gameRun)
                {
                    if (gameRun.BaseDeck.Any((Card card) => card.CardType == CardType.Friend))
                    {
                        return 1f;
                    }
                    return 0f;
                }
            }
        }
    }
}