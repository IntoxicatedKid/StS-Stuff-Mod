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

namespace StSStuffMod
{
    public sealed class StSFrozenEyeDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSFrozenEye);
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
                Appearance: AppearanceType.ShopOnly,
                Owner: "",
                LosableType: ExhibitLosableType.Losable,
                Rarity: Rarity.Common,
                Value1: null,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                // example of referring to UniqueId of an entity without calling MakeConfig
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSFrozenEyeDef))]
        [UsedImplicitly]
        public sealed class StSFrozenEye : Exhibit
        {
            [HarmonyPatch(typeof(PlayBoard), nameof(PlayBoard.ShowDrawZone))]
            class PlayBoard_ShowDrawZone_Patch
            {
                static bool Prefix()
                {
                    if (GameMaster.Instance.CurrentGameRun.Player.HasExhibit<StSFrozenEyeDef.StSFrozenEye>())
                    {
                        if (GameMaster.Instance.CurrentGameRun.Battle == null)
                        {
                            return false;
                        }
                        ShowCardsPayload showCardsPayload = new ShowCardsPayload
                        {
                            Name = "Game.DrawZoneOutOfOrder".Localize(true),
                            Description = "Cards.Show".Localize(true),
                            Cards = GameMaster.Instance.CurrentGameRun.Battle.DrawZone,
                            ShowType = ShowCardsType.None
                        };
                        UiManager.GetPanel<ShowCardsPanel>().Show(showCardsPayload);
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}