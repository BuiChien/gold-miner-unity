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
  [SerializeField]
  private Level level_;
  [SerializeField]
  private AudioClip explosive_audio_clip_;
  [SerializeField]
  private AudioClip low_value_audio_clip_;
  [SerializeField]
  private AudioClip normal_value_audio_clip_;
  [SerializeField]
  private AudioClip high_value_audio_clip_;
  [SerializeField]
  private AudioClip last_ten_seconds_audio_clip_;
  [SerializeField]
  private AudioClip pull_audio_clip_;
  [SerializeField]
  private AudioClip lose_audio_clip_;
  [SerializeField]
  private AudioClip win_audio_clip_;

  private Document document_;
  private CountdownTimer timer_;
  #endregion
  #region UnityFuncs
  void Awake() {
    game_event_controller_ = GameEventController.Instance;
    game_event_controller_.GameEvent.AddListener(OnGameEventHandler);
    hook_area_.GameEvent.AddListener(OnGameEventHandler);
    player_.GameEvent.AddListener(OnGameEventHandler);
    document_ = Document.Instance;
    timer_ = GetComponent<CountdownTimer>();
  }

  void Start() {
    StartCoroutine(LoadLevel());
  }

  void Update() {
    if(timer_.IsFire) {
      StartCoroutine(FinishLevel());
    }
  }
  #endregion

  private IEnumerator LoadLevel() {
    level_.LoadLevel(1, 3600);
    yield return null;
  }

  private IEnumerator FinishLevel() {
    if (document_.IsVictory) {

    } else {

    }
    yield return new WaitForSeconds(1f);
    NotifyEvent(new LoadSceneEventArgs("Shop"));
  }

  private void OnGameEventHandler(GameEventArgs gameEvent) {
    switch(gameEvent.Name) {
      case EventNames.HOOK_AREA_TOUCH:
        player_.DropHook();
        break;
      case EventNames.ATTACK: {
          AttachEventArgs attachEvent = (AttachEventArgs)gameEvent;
          document_.ScoreAmount = attachEvent.Victim.ScoreAmount;
          NotifyEvent(new PlayAudioEventArgs(pull_audio_clip_));
          if (attachEvent.Victim.IsHeavy) {
            player_.PullHeavy();
          } else {
            player_.Pull();
          }
          break;
        }
      case EventNames.PULL_SUCCESS:
        document_.UpdateScore();
        break;
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
        timer_.Pause();
        document_.SaveData();
        break;
      default:
        break;
    }
  }

  void NotifyEvent(GameEventArgs gameEvent) {
    game_event_controller_.NotifyEvent(gameEvent);
  }
}
