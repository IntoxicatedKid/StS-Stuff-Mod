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

namespace StSStuffMod.Exhibits
{
    public sealed class StSSacredBarkDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSSacredBark);
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
                Value1: 100,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: 0,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },

                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(StSSacredBarkDef))]
        [UsedImplicitly]
        public sealed class StSSacredBark : Exhibit
        {
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(OnBattleStarted));
                ReactBattleEvent(Battle.CardsAddedToDrawZone, new EventSequencedReactor<CardsAddingToDrawZoneEventArgs>(OnCardsAddedToDrawZone));
                ReactBattleEvent(Battle.CardsAddedToHand, new EventSequencedReactor<CardsEventArgs>(OnAddCard));
                ReactBattleEvent(Battle.CardsAddedToDiscard, new EventSequencedReactor<CardsEventArgs>(OnAddCard));
                ReactBattleEvent(Battle.CardsAddedToExile, new EventSequencedReactor<CardsEventArgs>(OnAddCard));
            }
            private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
            {
                foreach (Card card in Battle.EnumerateAllCards())
                {
                    if (card.CardType == CardType.Tool)
                    {
                        if (card.Config.Damage != null && card.RawDamage > 0)
                        {
                            card.DeltaDamage += card.RawDamage * (Value1 / 100);
                        }
                        if (card.Config.Block != null && card.RawBlock > 0)
                        {
                            card.DeltaBlock += card.RawBlock * (Value1 / 100);
                        }
                        if (card.Config.Shield != null && card.RawShield > 0)
                        {
                            card.DeltaShield += card.RawShield * (Value1 / 100);
                        }
                        if (card.Config.Value1 != null && card.ConfigValue1 > 0)
                        {
                            card.DeltaValue1 += card.ConfigValue1 * (Value1 / 100);
                        }
                        if (card.Config.Value2 != null && card.ConfigValue2 > 0)
                        {
                            card.DeltaValue2 += card.ConfigValue2 * (Value1 / 100);
                        }
                        /*if (card.Config.Mana != null)
                        {
                            card.Config.Mana *= (base.Value1 / 50);
                        }
                        if (card.Config.Scry != null)
                        {
                            card.Config.Scry *= (base.Value1 / 50);
                        }*/
                    }
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardsAddedToDrawZone(CardsAddingToDrawZoneEventArgs args)
            {
                yield return ToolCardModify(args.Cards);
                yield break;
            }
            private IEnumerable<BattleAction> OnAddCard(CardsEventArgs args)
            {
                yield return ToolCardModify(args.Cards);
                yield break;
            }
            private BattleAction ToolCardModify(IEnumerable<Card> cards)
            {
                foreach (Card card in cards)
                {
                    if (card.CardType == CardType.Tool)
                    {
                        if (card.Config.Damage != null && card.RawDamage > 0)
                        {
                            card.DeltaDamage += card.RawDamage * (Value1 / 100);
                        }
                        if (card.Config.Block != null && card.RawBlock > 0)
                        {
                            card.DeltaBlock += card.RawBlock * (Value1 / 100);
                        }
                        if (card.Config.Shield != null && card.RawShield > 0)
                        {
                            card.DeltaShield += card.RawShield * (Value1 / 100);
                        }
                        if (card.Config.Value1 != null && card.ConfigValue1 > 0)
                        {
                            card.DeltaValue1 += card.ConfigValue1 * (Value1 / 100);
                        }
                        if (card.Config.Value2 != null && card.ConfigValue2 > 0)
                        {
                            card.DeltaValue2 += card.ConfigValue2 * (Value1 / 100);
                        }
                        /*if (card.Config.Mana != null)
                        {
                            card.Config.Mana *= (base.Value1 / 50);
                        }
                        if (card.Config.Scry != null)
                        {
                            card.Config.Scry *= (base.Value1 / 50);
                        }*/
                    }
                }
                return null;
            }
        }
    }
}