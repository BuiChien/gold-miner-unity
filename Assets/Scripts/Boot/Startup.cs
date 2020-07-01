using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : GameScript {

  [SerializeField]
  private float BootingTimeInSecond = 20;

  private float booting_count_in_second_ = 0;

  void Start() {

  }

  void Update() {
    booting_count_in_second_ += UnityEngine.Time.deltaTime * 10;
    if(booting_count_in_second_ > BootingTimeInSecond) {
      NotifyEvent(new GameEventArgs(EventNames.STARTUP_SUCCESS));
    }
  }

  protected override void OnAppEventHanlder(GameEventArgs gameEvent) {

  }
}
