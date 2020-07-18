using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {
  public ItemPickupSo[] PickupSos;
  [SerializeField]
  private Text txt_level_;
  [SerializeField]
  private Text txt_score_;
  [SerializeField]
  private Button btn_next_level_;
  private Document document_;

  private GameEventController game_event_controller_;
  void Awake() {
    game_event_controller_ = GameEventController.Instance;
    game_event_controller_.GameEvent.AddListener(OnGameEventHandler);
    btn_next_level_.onClick.AddListener(OnNextLevel);
    document_ = Document.Instance;
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

  private void OnNextLevel() {
    document_.GoNextLevel();
    NotifyEvent(new LoadSceneEventArgs(SceneNames.MAIN));
  }

  void NotifyEvent(GameEventArgs gameEvent) {
    game_event_controller_.NotifyEvent(gameEvent);
  }
}
