using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    document_.Init();
#if DEBUG
    document_.NewGame();
#endif
    timer_.StartTime = document_.TotalTime;
    status_panel_.Level = document_.Level;
    status_panel_.TargetScore = document_.TagetScore;
    status_panel_.Score = 0;
    player_.HookSpeed = document_.HookSpeed;
    StartCoroutine(LoadLevel());
  }

  void Update() {
    status_panel_.TimeCounter = (int)timer_.Counter;
    if (timer_.IsFire || level_.IsFinish()) {
      StartCoroutine(FinishLevel());
    }
  }
#endregion

  private IEnumerator LoadLevel() {
    level_.LoadLevel(document_.Level, document_.LevelScore);
    yield return null;
    timer_.Restart();
  }

  private IEnumerator FinishLevel() {
    if (document_.IsVictory) {

    } else {

    }
    yield return new WaitForSeconds(1f);
    //NotifyEvent(new LoadSceneEventArgs("Shop"));
  }

  private void OnGameEventHandler(GameEventArgs gameEvent) {
    switch(gameEvent.Name) {
      case EventNames.HOOK_AREA_TOUCH:
        player_.DropHook();
        break;
      case EventNames.ATTACK: {
          AttachEventArgs attachEvent = (AttachEventArgs)gameEvent;
          document_.SetScoreAmount(attachEvent.Victim);
          NotifyEvent(new PlayAudioEventArgs(pulling_audio_, true));
          if (attachEvent.Victim.IsHeavy) {
            player_.PullHeavy();
          } else {
            player_.Pull();
          }
          break;
        }
      case EventNames.PULL_SUCCESS:
        OnPullSuccess();
        break;
      case EventNames.STATE_CHANGED:
        OnGameStateChanged((StateChangedEventArgs)gameEvent);
        break;
      case EventNames.DROP_BOMB:

        break;
      default:
        break;
    }
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
