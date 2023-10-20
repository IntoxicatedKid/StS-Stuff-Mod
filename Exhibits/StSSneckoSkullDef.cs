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
using LBoL.EntityLib.StatusEffects.Others;

namespace StSStuffMod.Exhibits
{
    public sealed class StSSneckoSkullDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSSneckoSkull);
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
                Rarity: Rarity.Common,
                Value1: 1,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: 0,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { "Poison" },
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSSneckoSkullDef))]
        [UsedImplicitly]
        [ExhibitInfo(WeighterType = typeof(StSSneckoSkullWeighter))]
        public sealed class StSSneckoSkull : Exhibit
        {
            protected override void OnEnterBattle()
            {
                foreach (EnemyUnit enemyUnit in Battle.AllAliveEnemies)
                {
                    ReactBattleEvent(enemyUnit.StatusEffectAdding, new EventSequencedReactor<StatusEffectApplyEventArgs>(OnEnemyStatusEffectAdding));
                }
                HandleBattleEvent(Battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(OnEnemySpawned));
            }
            private IEnumerable<BattleAction> OnEnemyStatusEffectAdding(StatusEffectApplyEventArgs args)
            {
                if (args.Effect is Poison)
                {
                    NotifyActivating();
                    args.Effect.Level += Value1;
                }
                yield break;
            }
            private void OnEnemySpawned(UnitEventArgs args)
            {
                ReactBattleEvent(args.Unit.StatusEffectAdding, new EventSequencedReactor<StatusEffectApplyEventArgs>(OnEnemyStatusEffectAdding));
            }
            private class StSSneckoSkullWeighter : IExhibitWeighter
            {
                public float WeightFor(Type type, GameRunController gameRun)
                {
                    if (gameRun.BaseDeck.Any((Card card) => card.IsUpgraded ? card.Config.UpgradedRelativeEffects.Contains("Poison") : card.Config.RelativeEffects.Contains("Poison")))
                    {
                        return 1f;
                    }
                    return 0f;
                }
            }
        }
    }
}