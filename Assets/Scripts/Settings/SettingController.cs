using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour {
  public UserSettings UserSettingsInfo { get => user_settings_; }
  public OperationData OperationDataInfo { get => operation_data_; }
  [SerializeField]
  private UserSettings user_settings_;
  [SerializeField]
  private OperationData operation_data_;
  public void Load() {
    LoadSetting<UserSettings>(user_settings_);
    LoadSetting<OperationData>(operation_data_);
  }

  public void SaveUserSettings() {
    SaveSetting<UserSettings>(user_settings_);
  }

  public void SaveOperationData() {
    SaveSetting<OperationData>(operation_data_);
  }

  private void LoadSetting<T>(T instance) {
    string settingData = PlayerPrefs.GetString(typeof(T).Name, string.Empty);
    if(settingData != string.Empty) {
      JsonUtility.FromJsonOverwrite(settingData, instance);
    }
  }

  private void SaveSetting<T>(T instance) {
    string settingData = JsonUtility.ToJson(instance);
    PlayerPrefs.SetString(instance.ToString(), settingData);
    PlayerPrefs.Save();
  }
}
