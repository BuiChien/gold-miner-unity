using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : Singleton<UiManager> {
  #region PrivateField
  [SerializeField]
  private Startup startup_;
  [SerializeField] 
  private MainMenu main_menu_;
  [SerializeField] 
  private PauseMenu pause_menu_;
  [SerializeField] 
  private Camera dummy_camera_;
  private GameEventController game_event_controller_;
  #endregion

  #region UnityFuncs
  void Start() {
    game_event_controller_ = GameEventController.Instance;
    game_event_controller_.GameEvent.AddListener(OnGameEventHandler);
    startup_.GameEvent.AddListener(OnGameEventHandler);
  }

  void Update() {

  }
  #endregion
  private void OnGameEventHandler(GameEventArgs gameEvent) {
    switch (gameEvent.Name) {
      case EventNames.BUTTON_CLICK: {
          OnButtonEventHandler((ButtonEventArgs)gameEvent);
        }
        break;
      case EventNames.STARTUP_SUCCESS:
        main_menu_.gameObject.SetActive(true);
        break;
    }
  }

  private void OnButtonEventHandler(ButtonEventArgs buttonEvent) {
    switch (buttonEvent.ButtonName) {
      case EventNames.RESTART: 
        break;
      case EventNames.NEWGAME: {
          main_menu_.gameObject.SetActive(false);
        }
        break;
      case EventNames.CONTINUE: {
          main_menu_.gameObject.SetActive(false);
        }
        break;
    }
  }
}
