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
using LBoL.Core.Stations;
using LBoL.Presentation.UI;
using LBoL.EntityLib.Exhibits.Shining;
using static System.Collections.Specialized.BitVector32;
using HarmonyLib;
using LBoL.Presentation.UI.Widgets;
using LBoL.EntityLib.Cards.Neutral.Green;
using UnityEngine.UI;

namespace StSStuffMod.Exhibits
{
    public sealed class StSOrreryDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSOrrery);
        }
        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.ExhibitsEn.yaml");
            return locFiles;
        }
        public override ExhibitSprites LoadSprite()
        {
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
                Appearance: AppearanceType.ShopOnly,
                Owner: "",
                LosableType: ExhibitLosableType.Losable,
                Rarity: Rarity.Uncommon,
                Value1: 5,
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
        [EntityLogic(typeof(StSOrreryDef))]
        [UsedImplicitly]
        public sealed class StSOrrery : Exhibit
        {
            protected override IEnumerator SpecialGain(PlayerUnit player)
            {
                Active = true;
                OnGain(player);
                UiManager.GetPanel<ShopPanel>().Hide();
                HandleGameRunEvent(GameRun.DeckCardsAdded, delegate (CardsEventArgs args)
                {
                    if (Active)
                    {
                        GameMaster.Instance.StartCoroutine(Wait());
                    }
                });
                GameRun.CurrentStation.ClearRewards();
                GameRun.CurrentStation.AddReward(GetOrreryReward());
                GameRun.CurrentStation.AddReward(GetOrreryReward());
                GameRun.CurrentStation.AddReward(GetOrreryReward());
                GameRun.CurrentStation.AddReward(GetOrreryReward());
                GameRun.CurrentStation.AddReward(GetOrreryReward());
                UiManager.GetPanel<RewardPanel>().Show(new ShowRewardContent
                {
                    Station = GameRun.CurrentStation,
                    Rewards = GameRun.CurrentStation.Rewards
                });
                UiManager.GetPanel<VnPanel>().SetNextButton(false, null, null);
                UiManager.GetPanel<RewardPanel>()._rewardWidgets.Do(rw => rw.Click += () =>
                {
                    UiManager.GetPanel<VnPanel>().SetNextButton(false, null, null);
                    UiManager.GetPanel<RewardPanel>().abandonButton.onClick.AddListener(delegate
                    {
                        UiManager.GetPanel<VnPanel>().SetNextButton(false, null, null);
                        if (UiManager.GetPanel<RewardPanel>()._rewardWidgets.Count == 0)
                        {
                            Active = false;
                            UiManager.GetPanel<RewardPanel>().Hide();
                            UiManager.GetPanel<VnPanel>().SetNextButton(true, null, null);
                        }
                    });
                    UiManager.GetPanel<RewardPanel>().returnButton.onClick.AddListener(delegate
                    {
                        UiManager.GetPanel<VnPanel>().SetNextButton(false, null, null);
                        if (UiManager.GetPanel<RewardPanel>()._rewardWidgets.Count == 0)
                        {
                            Active = false;
                            UiManager.GetPanel<RewardPanel>().Hide();
                            UiManager.GetPanel<VnPanel>().SetNextButton(true, null, null);
                        }
                    });
                });
                GameMaster.Instance.StartCoroutine(WaitMore());
                yield break;
            }
            private IEnumerator Wait()
            {
                yield return new WaitForEndOfFrame();
                UiManager.GetPanel<VnPanel>().SetNextButton(false, null, null);
                if (UiManager.GetPanel<RewardPanel>()._rewardWidgets.Count == 0)
                {
                    Active = false;
                    UiManager.GetPanel<RewardPanel>().Hide();
                    UiManager.GetPanel<VnPanel>().SetNextButton(true, null, null);
                }
            }
            private IEnumerator WaitMore()
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                UiManager.GetPanel<VnPanel>().SetNextButton(false, null, null);
            }
            private StationReward GetOrreryReward()
            {
                return StationReward.CreateCards(GameRun.GetRewardCards(GameRun.CurrentStage.EnemyCardOnlyPlayerWeight, GameRun.CurrentStage.EnemyCardWithFriendWeight, GameRun.CurrentStage.EnemyCardNeutralWeight, GameRun.CurrentStage.EnemyCardWeight, GameRun.RewardCardCount, false));
            }
        }
    }
}