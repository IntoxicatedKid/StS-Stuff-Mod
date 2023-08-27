using LBoL.Base;
using LBoL.ConfigData;
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
using System.Linq;
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Others;
using static StSStuffMod.BepinexPlugin;

namespace StSStuffMod
{
    public sealed class StSDramaticEntranceDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSDramaticEntrance);
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
               GunName: "StarPas",
               GunNameBurst: "StarPas",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Rare,
               Type: CardType.Attack,
               TargetType: TargetType.AllEnemies,
               Colors: new List<ManaColor>() { ManaColor.Colorless },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 0 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: 0,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 25,
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

               Keywords: Keyword.Exile | Keyword.Initial,
               UpgradedKeywords: Keyword.Forbidden,
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
    [EntityLogic(typeof(StSDramaticEntranceDef))]
    public sealed class StSDramaticEntrance : Card
    {
        public override int AdditionalDamage
        {
            get
            {
                if (base.Battle != null && this.IsUpgraded)
                {
                    List<Card> list = base.GameRun.BaseDeck.Where((Card card) => card is StSDramaticEntrance && card.IsUpgraded).ToList<Card>();
                    return list.Sum((Card card) => card.Value1);
                }
                return Value1;
            }
        }
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<GameEventArgs>(base.Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(this.OnBattleStarted));
        }
        private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
        {
            if (this == base.Battle.EnumerateAllCards().First((Card card) => card is StSDramaticEntrance && card.IsUpgraded))
            {
                List<Card> list = base.Battle.DrawZone.Where((Card card) => card is StSDramaticEntrance && card.IsUpgraded).ToList<Card>();
                yield return new ExileManyCardAction(list);
                yield return base.AttackAction(base.Battle.AllAliveEnemies, "StarPasNoAni");
            }
            yield break;
        }
    }
}
