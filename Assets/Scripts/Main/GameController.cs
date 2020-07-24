using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
  #region PrivateField
  private GameEventController game_event_controller_;
  [SerializeField]
  private Player player_;
  [SerializeField]
  private HookArea hook_area_;
  [SerializeField]
  private Level level_;
  [SerializeField]
  private AudioClip bomb_explosive_audio_;
  [SerializeField]
  private AudioClip catch_victim_low_score_audio_;
  [SerializeField]
  private AudioClip catch_victim_high_score_audio_;
  [SerializeField]
  private AudioClip catch_victim_normal_score_audio_;
  [SerializeField]
  private AudioClip timeup_count_audio_;
  [SerializeField]
  private AudioClip pulling_audio_;
  [SerializeField]
  private AudioClip user_lost_audio_;
  [SerializeField]
  private AudioClip user_win_audio_;
  [SerializeField]
  private StatusPanel status_panel_;
  [SerializeField]
  private PopupMenu popup_menu_;
  [SerializeField]
  private Inventory inventory_;
  [SerializeField]
  private Waypoint[] bomb_waypoints_;
  [SerializeField]
  private AudioClip background_audio_clip_ = null; 
  [SerializeField]
  private GameObject pickup_item_prefab_ = null;
  private Document document_;
  private bool level_finished_;
  #endregion
  #region UnityFuncs
  void Awake() {
    game_event_controller_ = GameEventController.Instance;
    game_event_controller_.GameEvent.AddListener(OnGameEventHandler);
    hook_area_.GameEvent.AddListener(OnGameEventHandler);
    player_.GameEvent.AddListener(OnGameEventHandler);
    document_ = Document.Instance;
  }

  void Start() {
    level_finished_ = false;
    status_panel_.Level = document_.Level;
    status_panel_.TargetScore = document_.TagetScore;
    status_panel_.Score = document_.TotalScore;
    player_.HookSpeed = document_.HookSpeed;
    inventory_.SetDisplayItems(document_.BuyItems);
    StartCoroutine(LoadLevel());
  }

  void Update() {
    if(level_finished_) {
      return;
    }
    if (document_.IsFinished || level_.IsFinish()) {
      player_.Abort();
      StartCoroutine(FinishLevel());
      level_finished_ = true;
      StopAllCoroutines();
    } else {
      status_panel_.TimeCounter = document_.Counter;
      if(document_.Counter <= 10) {
        NotifyEvent(new PlayAudioEventArgs(timeup_count_audio_, true));
      }
    }
  }
#endregion

  private IEnumerator LoadLevel() {
    popup_menu_.ShowLevelTarget(document_.Level, document_.TagetScore);
    level_.LoadLevel(document_.Level, document_.LevelScore);
    yield return new WaitForSeconds(1f);
    popup_menu_.HideLevelTarget();
    document_.StartTimer();
    NotifyEvent(new PlayAudioEventArgs(background_audio_clip_, true, false, true));
    yield return null;
  }

  private IEnumerator FinishLevel() {
    document_.FinishLevel();
    popup_menu_.ShowLevelComplete(document_.IsVictory, document_.TotalScore);
    NotifyEvent(new PlayAudioEventArgs(false, true));
    yield return null;
  }

  private void OnGameEventHandler(GameEventArgs gameEvent) {
    switch(gameEvent.Name) {
      case EventNames.HOOK_AREA_TOUCH:
        player_.DropHook();
        break;
      case EventNames.ATTACK: {
          AttachEventArgs attachEvent = (AttachEventArgs)gameEvent;
          inventory_.CanUse = true;
          document_.SetScoreAmount(attachEvent.Victim);
          NotifyEvent(new PlayAudioEventArgs(pulling_audio_, true));
          if (attachEvent.Victim.IsHeavy) {
            player_.HookSpeed = document_.HookHeavySpeed;
            player_.PullHeavy();
          } else {
            player_.Pull();
          }
          break;
        }
      case EventNames.PULL_SUCCESS:
        inventory_.CanUse = false;
        OnPullSuccess();
        player_.HookSpeed = document_.HookSpeed;
        break;
      case EventNames.STATE_CHANGED:
        OnGameStateChanged((StateChangedEventArgs)gameEvent);
        break;
      case EventNames.BUTTON_CLICK:
        OnButtonEventHandler((ButtonEventArgs)gameEvent);
        break;
      default:
        break;
    }
  }

  private void OnButtonEventHandler(ButtonEventArgs buttonEvent) {
    switch (buttonEvent.ButtonName) {
      case EventNames.USE_ITEM_PICKUP:
        OnUseItemPickupHandler((UseItemPickupEventArgs)buttonEvent);
        break;
    }
  }

  private void OnUseItemPickupHandler(UseItemPickupEventArgs gameEvent) {
    GameObject obj;
    switch (gameEvent.UseItem.Type) {
      case ItemPickupType.BOMB:
        obj = Instantiate(pickup_item_prefab_, player_.transform);
        obj.GetComponent<SpriteRenderer>().sprite = gameEvent.UseItem.InventoryIcon;
        obj.GetComponent<SpriteRenderer>().sortingOrder = 1;
        Bomb bObj = obj.AddComponent<Bomb>();
        obj.GetComponent<BoxCollider2D>().size = obj.GetComponent<SpriteRenderer>().sprite.rect.size;
        bObj.Waypoints = bomb_waypoints_;
        bObj.Target = player_.Victim;
        break;
      default:
        break;
    }
    document_.UseItemPickup(gameEvent.UseItem);
    player_.UseWeapons();
  }

  private void OnPullSuccess() {
    document_.UpdateScore();
    status_panel_.Score = document_.TotalScore;
    AudioClip clip;
    switch (document_.AmountType) {
      case ScoreAmountType.LOW:
        clip = catch_victim_low_score_audio_;
        break;
      case ScoreAmountType.NORMAL:
        clip = catch_victim_normal_score_audio_;
        break;
      default:
        clip = catch_victim_high_score_audio_;
        break;
    }
    NotifyEvent(new PlayAudioEventArgs(clip));
  }

  private void OnGameStateChanged(StateChangedEventArgs gameEvent) {
    switch ((GameState)gameEvent.NextState) {
      case GameState.RUNNING:
        document_.ResumeLevel();
        break;
      case GameState.PAUSED:
        NotifyEvent(new PlayAudioEventArgs(false, true));
        document_.PauseLevel();
        break;
      default:
        break;
    }
  }

  void NotifyEvent(GameEventArgs gameEvent) {
    game_event_controller_.NotifyEvent(gameEvent);
  }
}
