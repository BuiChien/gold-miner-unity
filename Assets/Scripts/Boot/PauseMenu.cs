using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : GameScript {
  public Button ResumeButton;
  public Button QuitButton;
  public Button RestartButton;

  void Awake() {
    RegisterGameEventController();
    ResumeButton.onClick.AddListener(OnButtonResumeClick);
    QuitButton.onClick.AddListener(OnButtonQuitClick);
    RestartButton.onClick.AddListener(OnButtonRestartClick);
  }

  void OnButtonResumeClick() {
    NotifyEvent(new ButtonEventArgs(EventNames.RESUME));
  }

  void OnButtonQuitClick() {
    NotifyEvent(new ButtonEventArgs(EventNames.QUIT));
  }

  void OnButtonRestartClick() {
    NotifyEvent(new ButtonEventArgs(EventNames.RESTART));
  }
}
