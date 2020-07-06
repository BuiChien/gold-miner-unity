using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {
  #region PrivateField
  private GameEventController game_event_controller_;
  [SerializeField]
  private Player player_;
  [SerializeField]
  private HookDroppableArea hook_area_;
  #endregion
  #region UnityFuncs
  void Start() {
    game_event_controller_ = GameEventController.Instance;
    game_event_controller_.GameEvent.AddListener(OnGameEventHandler);
  }

  void Update() {

  }
  #endregion
  private void OnGameEventHandler(GameEventArgs arg0) {

  }
}
