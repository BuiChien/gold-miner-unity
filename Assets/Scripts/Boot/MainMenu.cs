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

  [SerializeField]
  private SettingPanel setting_panel_;

  void Start() {
    RegisterGameEventController();
    NewGameButton.onClick.AddListener(OnButtonNewGameClick);
    ContineButton.onClick.AddListener(OnButtonContineClick);
    QuitButton.onClick.AddListener(OnButtonQuitClick);
    SettingsButton.onClick.AddListener(OnButtonSettingsClick);
    setting_panel_.GameEvent.AddListener(OnGameEventHandler);
  }

  void Update() {
    if(gameObject.activeSelf) {
      ContineButton.gameObject.SetActive(!Document.Instance.IsFirstTime);
    }
  }

  private void OnGameEventHandler(GameEventArgs arg0) {
    if(arg0.Name == "CloseSetting") {
      NewGameButton.interactable = true;
      SettingsButton.interactable = true;
      QuitButton.interactable = true;
      ContineButton.interactable = true;
      setting_panel_.Visible = false;
    }
  }

  private void OnButtonSettingsClick() {
    setting_panel_.Visible = true;
    NewGameButton.interactable = false;
    SettingsButton.interactable = false;
    QuitButton.interactable = false;
    ContineButton.interactable = false;
    BroadcastEvent(new ButtonEventArgs(""));
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
