using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {
  #region PublicField
  public GameState CurrentState {
    get { return current_state_; }
    private set { current_state_ = value; }
  }
  #endregion

  #region PrivateField
  private GameEventController game_event_controller_;
  private List<string> scenes_loaded_;
  private GameState current_state_ = GameState.PREGAME;
  AudioSource audio_background_player_;
  AudioSource audio_clip_player_;
  [SerializeField]
  private AudioClip button_click_clip_ = null;
  private AudioClip oneshot_repeat_clip_ = null;
  private Document document_;
  #endregion

  #region UnityFuncs
  private void Awake() {
    audio_background_player_ = Instance.gameObject.AddComponent<AudioSource>();
    audio_clip_player_ = Instance.gameObject.AddComponent<AudioSource>();
    document_ = Document.Instance;
  }

  void Start() {
    scenes_loaded_ = new List<string>();
    game_event_controller_ = GameEventController.Instance;
    game_event_controller_.GameEvent.AddListener(OnGameEventHandler);
    UpdateState(GameState.PREGAME);
  }

  void Update() {
    if (current_state_ == GameState.PREGAME) {
      return;
    }
    if (Input.GetKeyUp(KeyCode.Escape)) {
      TogglePause();
    }
    AudioRepeat();
  }

  void FixedUpdate() {
    AudioRepeat();
  }

  void OnApplicationPause(bool pause) {
    document_.SaveData();
  }

  void OnApplicationQuit() {
    document_.SaveData();
  }

  #endregion

  #region ProcessExternalEvent
  private void OnGameEventHandler(GameEventArgs gameEvent) {
    switch(gameEvent.Name) {
      case EventNames.LOAD_SCENE:
        LoadScene((LoadSceneEventArgs)gameEvent);
        UpdateState(GameState.RUNNING);
        break;
      case EventNames.BUTTON_CLICK: {
        if(document_.UserSettingsInfo.SoundEnable) {
            audio_clip_player_.PlayOneShot(button_click_clip_);
          }
        }
        break;
      case EventNames.PLAY_AUDIO: {
          PlayAudio((PlayAudioEventArgs)gameEvent);
        }
        break;
      case EventNames.RESUME:
        TogglePause();
        break;
      case EventNames.SHOW_MENU:
        UnloadAll();
        oneshot_repeat_clip_ = null;
        audio_clip_player_.Stop();
        break;
      case EventNames.QUIT:
        QuitGame();
        break;
    }
  }

  private void PlayAudio(PlayAudioEventArgs eventArgs) {
    if(eventArgs.IsDefaultMusic) {
      if (document_.UserSettingsInfo.MusicEnable) {
        if(eventArgs.Clip != null && !eventArgs.IsStop) {
          audio_background_player_.clip = eventArgs.Clip;
          audio_background_player_.PlayOneShot(eventArgs.Clip);
        } else if(eventArgs.IsStop) {
          audio_background_player_.Stop();
        } else {
          if(audio_background_player_.clip != null) {
            audio_background_player_.Play();
          }
        }
      } else if(eventArgs.IsStop) {
        audio_background_player_.Stop();
      } else if(eventArgs.Clip != null) {
        audio_background_player_.clip = eventArgs.Clip;
      }
    } else {
      if (document_.UserSettingsInfo.SoundEnable) {
        if(eventArgs.Clip != null) {
          oneshot_repeat_clip_ = eventArgs.IsRepeat ? eventArgs.Clip : null;
          audio_clip_player_.PlayOneShot(eventArgs.Clip);
        }
      }
    }
  }

  private void LoadScene(LoadSceneEventArgs eventArgs) {
    if (scenes_loaded_.Contains(eventArgs.SceneName)) {
      return;
    }
    if (!eventArgs.IsAdditive) {
      UnloadAll();
    }
    StartCoroutine(LoadSceneAsync(eventArgs.SceneName));
  }

  private void UnloadAll() {
    for (int i = 0; i < scenes_loaded_.Count; i++) {
      StartCoroutine(UnloadSceneAsync(scenes_loaded_[i]));
    }
  }

  private IEnumerator LoadSceneAsync(string sceneName) {
    AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    if (ao == null) {
      Debug.LogError("[GameManager] Unable to load level " + sceneName);
      yield return null;
    }
    scenes_loaded_.Add(sceneName);
    while (!ao.isDone) {
      yield return null;
    }
  }

  private IEnumerator UnloadSceneAsync(string sceneName) {
    AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);
    if (ao == null) {
      Debug.LogError("[GameManager] Unable to unload level " + sceneName);
      yield return null;
    }
    scenes_loaded_.Remove(sceneName);
    while (!ao.isDone) {
      yield return null;
    }
  }
  #endregion

  #region ProcessInternalEvent
  private void AudioRepeat() {
    if (oneshot_repeat_clip_ != null && !
      audio_clip_player_.isPlaying &&
      document_.UserSettingsInfo.SoundEnable) {
      audio_clip_player_.PlayOneShot(oneshot_repeat_clip_);
    }
    if(audio_background_player_.clip != null && 
      !audio_background_player_.isPlaying &&
      document_.UserSettingsInfo.MusicEnable) {
      audio_background_player_.Play();
    }
  }

  private void UpdateState(GameState state) {
    GameState previousGameState = current_state_;
    current_state_ = state;
    switch (CurrentState) {
      case GameState.PREGAME:
        break;
      case GameState.RUNNING:
        //TODO: Audio restart
        break;
      case GameState.PAUSED:
        //TODO: Audio stop
        break;
      default:
        break;
    }
    NotifyEvent(new StateChangedEventArgs((int)previousGameState, (int)current_state_));
  }

  public void TogglePause() {
    UpdateState(current_state_ == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
  }

  public void QuitGame() {
    Application.Quit();
  }
  #endregion
  void NotifyEvent(GameEventArgs gameEvent) {
    if (gameEvent != null) {
      game_event_controller_.NotifyEvent(gameEvent);
    }
  }
}
