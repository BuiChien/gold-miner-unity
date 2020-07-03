using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : GameScript {

  [SerializeField]
  private float BootingTimeInSecond = 2;
  SettingController setting_controller_;
  void Start() {
    setting_controller_ = SettingController.Instance;
    StartCoroutine(LoadData());
  }

  private IEnumerator LoadData() {
    setting_controller_.Load();
    yield return new WaitForSeconds(BootingTimeInSecond);
    NotifyEvent(new GameEventArgs(EventNames.STARTUP_SUCCESS));
    yield return null;
  }
}
