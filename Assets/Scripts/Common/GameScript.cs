using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameScript : MonoBehaviour {
  private GameEventController game_event_controller_;
  public GameEventHandler GameEvent;
  protected void RegisterGameEventController() {
    game_event_controller_ = GameEventController.Instance;
    game_event_controller_.GameEvent.AddListener(OnAppEventHanlder);
  }

  protected void NotifyEvent(GameEventArgs gameEvent) {
    if (gameEvent != null) {
      GameEvent.Invoke(gameEvent);
    }
  }

  protected void BroadcastEvent(GameEventArgs gameEvent) {
    if (gameEvent != null) {
      game_event_controller_.NotifyEvent(gameEvent);
    }
  }

  protected void OnDestroy() {
    if(game_event_controller_ != null) {
      game_event_controller_.GameEvent.RemoveListener(OnAppEventHanlder);
    }
  }

  protected virtual void OnAppEventHanlder(GameEventArgs event_args) {

  }
}

