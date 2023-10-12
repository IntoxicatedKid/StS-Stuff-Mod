using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Adventures;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core;
using LBoL.Core.Cards;
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
using System.Linq;
using LBoL.EntityLib.Cards.Other.Enemy;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.Cards.Neutral.NoColor;

namespace StSStuffMod.Cards
{
    public sealed class StSRecycleDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSRecycle);
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
               TargetType: TargetType.Nobody,
               Colors: new List<ManaColor>() { ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { Green = 1 },
               UpgradedCost: new ManaGroup() { Any = 0 },
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: null,
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
               RelativeKeyword: Keyword.Exile,
               UpgradedRelativeKeyword: Keyword.Exile,

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
    [EntityLogic(typeof(StSRecycleDef))]
    public sealed class StSRecycle : Card
    {
        public override Interaction Precondition()
        {
            List<Card> list = Battle.HandZone.Where((hand) => hand != this).ToList();
            if (list.Count == 1)
            {
                oneTargetHand = list[0];
            }
            if (list.Count <= 1)
            {
                return null;
            }
            return new SelectHandInteraction(1, 1, list);
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                Card card = ((SelectHandInteraction)precondition).SelectedCards[0];
                if (card != null)
                {
                    yield return new ExileCardAction(card);
                    yield return new GainManaAction(card.ConfigCostAnyToColorless(false));
                }
                card = null;
            }
            else if (oneTargetHand != null)
            {
                yield return new ExileCardAction(oneTargetHand);
                yield return new GainManaAction(oneTargetHand.ConfigCostAnyToColorless(false));
                oneTargetHand = null;
            }
            yield break;
        }
        private Card oneTargetHand;
    }
}
