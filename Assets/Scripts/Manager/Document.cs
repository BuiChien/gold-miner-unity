﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreAmountType {
  LOW,
  NORMAL,
  HIGH
}

public class Document : Singleton<Document> {
  #region PrivateField
  [SerializeField]
  private SettingController setting_controller_;
  OperationData data_context_;
  private const int LOW_SCORE_MARGIN        = 150;
  private const int NORMAL_SCORE_MARGIN     = 400;
  #endregion

  #region PublicFields
  public UserSettings UserSettingsInfo { get => setting_controller_.UserSettingsInfo; }
  public int Level {
    get => data_context_.Level;
  }

  public int TagetScore { get; private set; }

  public int LevelScore { get; private set; }

  public int HookSpeed { get; private set; }

  public int TotalScore {
    get => data_context_.TotalScore; 
    private set {
      data_context_.TotalScore = value;
    }
  }
  public int ScoreAmount { get; private set; }
  public ScoreAmountType AmountType { get; private set; }
  public int TotalTime { get; private set; }

  public bool IsVictory { get => TotalScore >= TagetScore; }

  public List<ItemPickup> BuyItems {
    get {
      List<ItemPickup> buyItems = new List<ItemPickup>();
      buy_item_list_.ForEach(x => {
        buyItems.Add(x);
      });
      return buyItems;
    }
  }

  private List<ItemPickup> buy_item_list_;
  #endregion

  public void Init() {
    setting_controller_.Load();
    data_context_ = setting_controller_.OperationDataInfo;
    buy_item_list_ = new List<ItemPickup>();
  }

  public void GoNextLevel() {
    data_context_.Level++;
    if(BuyItems.FindIndex(x => x.Type == ItemPickupType.CLOCK) >= 0) {
      TotalTime = (int)((Random.Range(0, Level) / Level) * 20) + 70;
    } else {
      TotalTime = 60;
    }
    if (BuyItems.FindIndex(x => x.Type == ItemPickupType.POWER) >= 0) {
      HookSpeed = 5;
    } else {
      HookSpeed = 3;
    }
    int oldTargetScore = TagetScore;
    TagetScore = ((Level - 1) * 1200) + 800 + Random.Range(0, Level) * 500;
    LevelScore = (TagetScore - oldTargetScore) + Random.Range(800, 2000);
  }

  public void NewGame() {
    data_context_.Level = 1;
    TotalTime = 61;
    HookSpeed = 3;
    TagetScore = 800;
    LevelScore = TagetScore + Random.Range(800, 2000);
  }

  public void UpdateScore() {
    TotalScore += ScoreAmount;
  }

  public void SetScoreAmount(IVictim victim) {
    int index;
    switch (victim.Tag) {
      case "Rock":
        index = BuyItems.FindIndex(x => x.Type == ItemPickupType.BOOK_STONE);
        if (index >= 0) {
          ScoreAmount = victim.ScoreAmount * 3;
        } else {
          ScoreAmount = victim.ScoreAmount;
        }
        break;
      case "Diamond":
        index = BuyItems.FindIndex(x => x.Type == ItemPickupType.DIAMOND);
        if (index >= 0) {
          ScoreAmount = victim.ScoreAmount + 100;
        } else {
          ScoreAmount = victim.ScoreAmount;
        }
        break;
      default:
        ScoreAmount = victim.ScoreAmount;
        break;
    }
    if(ScoreAmount <= LOW_SCORE_MARGIN) {
      AmountType = ScoreAmountType.LOW;
    } else if(ScoreAmount <= NORMAL_SCORE_MARGIN) {
      AmountType = ScoreAmountType.NORMAL;
    } else {
      AmountType = ScoreAmountType.HIGH;
    }
  }

  public void DropBomb() {

  }

  public void SaveData() {
    setting_controller_.SaveOperationData();
  }
}
