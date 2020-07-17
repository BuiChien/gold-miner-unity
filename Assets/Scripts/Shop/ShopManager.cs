using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {
  public ItemPickupSo[] PickupSos;

  private GameEventController game_event_controller_;
  void Awake() {
    game_event_controller_ = GameEventController.Instance;
    game_event_controller_.GameEvent.AddListener(OnGameEventHandler);
  }

  // Update is called once per frame
  void Update() {

  }

  private void OnGameEventHandler(GameEventArgs gameEvent) {
    switch (gameEvent.Name) {
      case EventNames.STATE_CHANGED:
        OnGameStateChanged((StateChangedEventArgs)gameEvent);
        break;
      default:
        break;
    }
  }

  private void OnGameStateChanged(StateChangedEventArgs gameEvent) {
    switch ((GameState)gameEvent.NextState) {
      case GameState.PAUSED:

        break;
      default:
        break;
    }
  }
}
