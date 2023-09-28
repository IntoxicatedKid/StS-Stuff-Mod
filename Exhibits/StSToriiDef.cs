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
using LBoL.Core.Stations;
using HarmonyLib;
using LBoL.EntityLib.EnemyUnits.Normal;
using System.Runtime.CompilerServices;
using LBoL.Presentation.Effect;
using LBoL.Presentation.UI.Dialogs;
using LBoL.Presentation.UI.ExtraWidgets;
using LBoL.Presentation.UI.Widgets;
using LBoL.Presentation.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.Presentation.UI;
using LBoL.EntityLib.Adventures;
using Yarn;
using LBoL.Core.GapOptions;
using LBoL.EntityLib.Exhibits.Adventure;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.UI.GridLayoutGroup;

namespace StSStuffMod.Exhibits
{
    public sealed class StSToriiDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSTorii);
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
                Value1: 5,
                Value2: 1,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSToriiDef))]
        [UsedImplicitly]
        public sealed class StSTorii : Exhibit
        {
            protected override void OnEnterBattle()
            {
                HandleBattleEvent(Battle.Player.DamageTaking, new GameEventHandler<DamageEventArgs>(OnPlayerDamageTaking));
            }
            private void OnPlayerDamageTaking(DamageEventArgs args)
            {
                DamageInfo damageInfo = args.DamageInfo;
                if (damageInfo.DamageType == DamageType.Attack)
                {
                    int num = damageInfo.Damage.RoundToInt();
                    if (num <= Value1 && num > Value2)
                    {
                        NotifyActivating();
                        args.DamageInfo = damageInfo.ReduceActualDamageBy(num - Value2);
                        args.AddModifier(this);
                    }
                }
            }
        }
    }
}