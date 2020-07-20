using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {
  [SerializeField]
  private Text txt_level_;
  [SerializeField]
  private Text txt_score_;
  [SerializeField]
  private Button btn_next_level_;
  private Document document_;
  [SerializeField]
  private ShopItemDisplay[] items_display_;
  private List<int> spawn_indexes_;

  private GameEventController game_event_controller_;

  private int Level {
    set {
      txt_level_.text = "LEVEL " + value.ToString();
    }
  }

  private int Score {
    set {
      txt_score_.text = "$ " + value.ToString();
    }
  }

  void Awake() {
    game_event_controller_ = GameEventController.Instance;
    game_event_controller_.GameEvent.AddListener(OnGameEventHandler);
    btn_next_level_.onClick.AddListener(() => {
      document_.GoNextLevel();
      NotifyEvent(new ButtonEventArgs(""));
      NotifyEvent(new LoadSceneEventArgs(SceneNames.MAIN));
    });
    document_ = Document.Instance;
  }

  void Start() {
    spawn_indexes_ = new List<int>();
    int index;
    document_.Clear();
    Level = document_.Level;
    Score = document_.TotalScore;
    while(spawn_indexes_.Count <= items_display_.Length) {
      index = UnityEngine.Random.Range(0, document_.PickupSos.Count);
      if(!spawn_indexes_.Contains(index)) {
        spawn_indexes_.Add(index);
      }
    }
    for (int i = 0; i < items_display_.Length; i++) {
      items_display_[i].Character = document_.PickupSos[spawn_indexes_[i]];
      items_display_[i].GameEvent.AddListener((e) => {
        ShopBuyItemEventArgs args = e as ShopBuyItemEventArgs;
        NotifyEvent(e);
        document_.BuyPickupItem(args.BuyItem.Character, args.Amount);
        Score = document_.TotalScore;
      });
    }
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

  void NotifyEvent(GameEventArgs gameEvent) {
    game_event_controller_.NotifyEvent(gameEvent);
  }
}
