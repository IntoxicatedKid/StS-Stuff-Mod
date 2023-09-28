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

namespace StSStuffMod.Exhibits
{
    public sealed class StSCultistHeadpieceDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSCultistHeadpiece);
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
                LosableType: ExhibitLosableType.Losable,
                Rarity: Rarity.Common,
                Value1: 3,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: 0,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { "TempFirepowerNegative" },
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSCultistHeadpieceDef))]
        [UsedImplicitly]
        public sealed class StSCultistHeadpiece : Exhibit
        {
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(OnBattleStarted));
                //base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarted));
            }
            private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
            {
                NotifyActivating();
                yield return PerformAction.Chat(Battle.Player, "CAW!\nCAAAW", 2f, 0f, 0f, true);
                yield return PerformAction.Sfx("Raven", 0f);
                yield return PerformAction.Sfx("Raven", 0f);
                yield return PerformAction.Sfx("Raven", 0.5f);
                yield return PerformAction.Sfx("Raven", 0f);
                foreach (EnemyUnit enemyUnit in Battle.EnemyGroup)
                {
                    if (enemyUnit.IsAlive)
                    {
                        yield return new ApplyStatusEffectAction<TempFirepowerNegative>(enemyUnit, new int?(Value1), null, null, null, 0f, true);
                    }
                }
                yield break;
            }
        }
    }
}