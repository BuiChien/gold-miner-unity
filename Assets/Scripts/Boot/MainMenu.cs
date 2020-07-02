using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : GameScript {
  public Button NewGameButton;
  public Button ContineButton;
  public Button QuitButton;
  public Button SettingsButton;
  void Start() {
    RegisterGameEventController();
    NewGameButton.onClick.AddListener(OnButtonNewGameClick);
    ContineButton.onClick.AddListener(OnButtonContineClick);
    QuitButton.onClick.AddListener(OnButtonQuitClick);
    SettingsButton.onClick.AddListener(OnButtonSettingsClick);
  }

  void Update() {

  }

  private void OnButtonSettingsClick() {

  }

  private void OnButtonQuitClick() {
    BroadcastEvent(new ButtonEventArgs(EventNames.QUIT));
  }

  private void OnButtonContineClick() {
    BroadcastEvent(new ButtonEventArgs(EventNames.CONTINUE));
  }

  private void OnButtonNewGameClick() {
    BroadcastEvent(new ButtonEventArgs(EventNames.NEWGAME));
  }
}
