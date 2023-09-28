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
using HarmonyLib;
using LBoL.Core.StatusEffects;
using UnityEngine.Rendering;
using LBoL.Core.Units;
using LBoL.EntityLib.Exhibits.Shining;
using Mono.Cecil;
using JetBrains.Annotations;
using System.Linq;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Other.Enemy;
using LBoL.Core.Randoms;

namespace StSStuffMod.Cards
{
    public sealed class StSMagnetismDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSMagnetism);
        }
        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(embeddedSource);
            imgs.AutoLoad(this, ".png", relativePath: "Resources.");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.CardsEn.yaml");
            return locFiles;
        }
        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
               Index: sequenceTable.Next(typeof(CardConfig)),
               Id: "",
               Order: 10,
               AutoPerform: true,
               Perform: new string[0][],
               GunName: "Simple1",
               GunNameBurst: "Simple1",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Rare,
               Type: CardType.Ability,
               TargetType: TargetType.Self,
               Colors: new List<ManaColor>() { ManaColor.Colorless },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 2, Colorless = 2 },
               UpgradedCost: new ManaGroup() { Any = 1, Colorless = 1 },
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 1,
               UpgradedValue1: null,
               Value2: null,
               UpgradedValue2: null,
               Mana: null,
               UpgradedMana: null,
               Scry: null,
               UpgradedScry: null,
               ToolPlayableTimes: null,
               Loyalty: null,
               UpgradedLoyalty: null,
               PassiveCost: null,
               UpgradedPassiveCost: null,
               ActiveCost: null,
               UpgradedActiveCost: null,
               UltimateCost: null,
               UpgradedUltimateCost: null,

               Keywords: Keyword.None,
               UpgradedKeywords: Keyword.None,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: null,
               Unfinished: false,
               Illustrator: "Mega Crit",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(StSMagnetismDef))]
    public sealed class StSMagnetism : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<StSMagnetismSeDef.StSMagnetismSe>(Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
    public sealed class StSMagnetismSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSMagnetismSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.StSMagnetismSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 10,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }
        [EntityLogic(typeof(StSMagnetismSeDef))]
        public sealed class StSMagnetismSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                ReactOwnerEvent(Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(OnOwnerTurnStarted));
            }
            private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
            {
                if (Battle.BattleShouldEnd)
                {
                    yield break;
                }
                yield return Trigger();
                yield break;
            }
            public BattleAction Trigger()
            {
                NotifyActivating();
                Card[] array = Battle.RollCardsWithoutManaLimit(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), Level, (config) => config.Colors.Contains(ManaColor.Colorless) && config.Id != base.Id);
                if (array.Count() > 0)
                {
                    foreach (Card card in array)
                    {
                        card.IsEthereal = true;
                        card.IsExile = true;
                    }
                    return new AddCardsToHandAction(array);
                }
                return null;
            }
        }
    }
}