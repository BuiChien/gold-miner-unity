using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : Singleton<SettingController> {
  public UserSettings UserSettingsInfo { get; private set; }
  public OperationData OperationDataInfo { get; private set; }

  public void Load() {
    UserSettingsInfo = LoadSetting<UserSettings>();
    OperationDataInfo = LoadSetting<OperationData>();
  }

  public void SaveUserSettings() {
    SaveSetting<UserSettings>(UserSettingsInfo);
  }

  public void SaveOperationData() {
    SaveSetting<OperationData>(OperationDataInfo);
  }

  private T LoadSetting<T>() {
    T instance = default(T);
    string settingData = PlayerPrefs.GetString(typeof(T).Name, string.Empty);
    if(settingData != string.Empty) {
      instance = JsonUtility.FromJson<T>(settingData);
    }
    if(instance == null) {
      instance = Activator.CreateInstance<T>();
    }
    if(instance is ISetting) {
      ISetting setting = (ISetting)instance;
      setting.NullToDefault();
    }
    return instance;
  }

  private void SaveSetting<T>(T instance) {
    string settingData = JsonUtility.ToJson(instance);
    PlayerPrefs.SetString(instance.ToString(), settingData);
    PlayerPrefs.Save();
  }
}
