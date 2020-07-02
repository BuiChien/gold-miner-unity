﻿using System;
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
  private bool active_camera_ {
    get => dummy_camera_.gameObject.activeSelf;
    set {
      dummy_camera_.gameObject.SetActive(value);
    }
  }

  private bool active_main_menu_ {
    get => main_menu_.gameObject.activeSelf;
    set {
      main_menu_.gameObject.SetActive(value);
      ActiveCamera();
    }
  }
  private bool active_pause_menu_ {
    get => pause_menu_.gameObject.activeSelf;
    set {
      pause_menu_.gameObject.SetActive(value);
      ActiveCamera();
    }
  }
  private bool active_startup_ {
    get => startup_.gameObject.activeSelf;
    set {
      startup_.gameObject.SetActive(value);
      ActiveCamera();
    }
  }
  #endregion

  #region UnityFuncs
  void Start() {
    game_event_controller_ = GameEventController.Instance;
    game_event_controller_.GameEvent.AddListener(OnGameEventHandler);
    startup_.gameObject.SetActive(true);
    main_menu_.gameObject.SetActive(false);
    pause_menu_.gameObject.SetActive(false);
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
        active_startup_ = false;
        active_main_menu_ = true;
        active_pause_menu_ = false;
        break;
      case EventNames.STATE_CHANGED:
        OnGameStateChanged((StateChangedEventArgs)gameEvent);
        break;
    }
  }

  private void OnGameStateChanged(StateChangedEventArgs args) {
    switch ((GameState)args.NextState) {
      case GameState.PAUSED:
        active_pause_menu_ = true;
        break;
      default:
        active_main_menu_ = false;
        active_pause_menu_ = false;
        break;
    }
  }

  private void OnButtonEventHandler(ButtonEventArgs buttonEvent) {
    switch (buttonEvent.ButtonName) {
      case EventNames.RESTART:
        active_pause_menu_ = false;
        break;
      case EventNames.NEWGAME: {
          active_main_menu_ = false;
          NotifyEvent(new LoadSceneEventArgs("Main"));
        }
        break;
      case EventNames.CONTINUE: {
          active_main_menu_ = false;
          NotifyEvent(new LoadSceneEventArgs("Main"));
        }
        break;
    }
  }

  private void ActiveCamera() {
    if(active_main_menu_ == false && 
      active_pause_menu_ == false  && 
      active_startup_ == false) {
      active_camera_ = false;
    } else {
      active_camera_ = true;
    }
  }

  void NotifyEvent(GameEventArgs gameEvent) {
    if (gameEvent != null) {
      game_event_controller_.NotifyEvent(gameEvent);
    }
  }
}
