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

  public int Score { get; private set; }
  #endregion

  public void Init() {
    setting_controller_.Load();
    data_context_ = setting_controller_.OperationDataInfo;
  }
  public void GoNextLevel() {
    data_context_.Level++;
    if (data_context_.Level > MaxLevel) {
      data_context_.Level = 1;
    }
  }

  public void NewGame() {
    data_context_.Level = 1;
  }
}
