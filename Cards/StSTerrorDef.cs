using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Adventures;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Sakuya;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using LBoL.Core.StatusEffects;
using LBoL.Core.Randoms;
using static StSStuffMod.BepinexPlugin;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.EntityLib.StatusEffects.Reimu;
using LBoL.Base.Extensions;
using System.Linq;
using UnityEngine;
using JetBrains.Annotations;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.EntityLib.StatusEffects.Others;

namespace StSStuffMod.Cards
{
    public sealed class StSTerrorDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSTerror);
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
               Rarity: Rarity.Uncommon,
               Type: CardType.Skill,
               TargetType: TargetType.SingleEnemy,
               Colors: new List<ManaColor>() { ManaColor.White, ManaColor.Blue },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 1, White = 1, Blue = 1 },
               UpgradedCost: new ManaGroup() { Any = 1 },
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 99,
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

               Keywords: Keyword.Exile,
               UpgradedKeywords: Keyword.Exile,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { "Vulnerable" },
               UpgradedRelativeEffects: new List<string>() { "Vulnerable" },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: null,
               Unfinished: false,
               Illustrator: "Mega Crit",
               SubIllustrator: new List<string>() { "suwaneko" }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(StSTerrorDef))]
    public sealed class StSTerror : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DebuffAction<Vulnerable>(selector.SelectedEnemy, 0, Value1, 0, 0, true, 0.2f);
            if (selector.SelectedEnemy.HasStatusEffect<Vulnerable>())
            {
                if (selector.SelectedEnemy.GetStatusEffect<Vulnerable>().Duration >= 99)
                {
                    yield return new ApplyStatusEffectAction<StSTerrorSeDef.StSTerrorSe>(selector.SelectedEnemy, null, null, null, null, 0.2f, true);
                }
            }
            yield break;
        }
    }
    public sealed class StSTerrorSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSTerrorSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.StSTerrorSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 7,
                Type: StatusEffectType.Special,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: false,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { "Vulnerable" },
                VFX: "DebuffRed",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }
        [EntityLogic(typeof(StSTerrorSeDef))]
        public sealed class StSTerrorSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                HandleOwnerEvent(unit.StatusEffectRemoving, new GameEventHandler<StatusEffectEventArgs>(OnStatusEffectRemoving));
            }
            private void OnStatusEffectRemoving(StatusEffectEventArgs args)
            {
                if (args.Effect is Vulnerable)
                {
                    args.ForceCancelBecause(CancelCause.Reaction);
                    NotifyActivating();
                    if (args.Effect.Duration == 0)
                    {
                        args.Effect.Duration = 99;
                    }
                }
            }
        }
    }
}
