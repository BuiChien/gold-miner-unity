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
  private HookArea hook_area_;
  #endregion
  #region UnityFuncs
  void Start() {
    game_event_controller_ = GameEventController.Instance;
    game_event_controller_.GameEvent.AddListener(OnGameEventHandler);
    hook_area_.GameEvent.AddListener(OnGameEventHandler);
    player_.GameEvent.AddListener(OnGameEventHandler);
  }

  void Update() {

  }
  #endregion
  private void OnGameEventHandler(GameEventArgs gameEvent) {
    switch(gameEvent.Name) {
      case EventNames.HOOK_AREA_TOUCH: {
          if (player_.State == Player.PlayerState.IDLE) {
            player_.State = Player.PlayerState.DROP_HOOK;
          }
        }
        break;
    }
  }
}
