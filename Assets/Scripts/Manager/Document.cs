using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Document : Singleton<Document> {
  #region PrivateField
  [SerializeField]
  private SettingController setting_controller_;
  OperationData data_context_;
  #endregion
  #region PublicFields
  public UserSettings UserSettingsInfo { get => setting_controller_.UserSettingsInfo; }
  public int Level {
    get => data_context_.Level;
  }

  public int MaxLevel {
    get => data_context_.LevelData.Length;
  }

  public int TagetScore {
    get => data_context_.LevelData[Level].TargetScore;
  }

  public int TotalScore {
    get => data_context_.TotalScore; 
    private set {
      data_context_.TotalScore = value;
    }
  }
  public int Score { get; private set; }
  public int ScoreAmount { get; set; }
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
    if (data_context_.Level > MaxLevel) {
      data_context_.Level = 1;
    }
  }

  public void NewGame() {
    Score = 0;
    data_context_.Level = 1;
    TotalTime = 61;
  }

  public void UpdateScore() {
    Score += ScoreAmount;
  }

  public void SaveData() {
    setting_controller_.SaveOperationData();
  }
}
