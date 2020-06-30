using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : GameScript {
  public Button ResumeButton;
  public Button QuitButton;
  public Button RestartButton;

  protected override void Awake() {
    ResumeButton.onClick.AddListener(OnButtonResumeClick);
    QuitButton.onClick.AddListener(OnButtonQuitClick);
    RestartButton.onClick.AddListener(OnButtonRestartClick);
  }

  void OnButtonResumeClick() {
    NotifyEvent(new GameEventArgs(EventNames.RESUME));
  }

  void OnButtonQuitClick() {
    NotifyEvent(new GameEventArgs(EventNames.QUIT));
  }

  void OnButtonRestartClick() {
    NotifyEvent(new GameEventArgs(EventNames.RESTART));
  }
}
