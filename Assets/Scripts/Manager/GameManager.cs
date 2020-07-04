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
  private AudioClip background_audio_clip_ = null;
  [SerializeField]
  private AudioClip button_click_clip_ = null;
  private Document document_;
  #endregion

  #region UnityFuncs
  private void Awake() {
    audio_background_player_ = Instance.gameObject.AddComponent<AudioSource>();
    audio_background_player_.clip = background_audio_clip_;
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
      case EventNames.RESTART:
        UpdateState(GameState.PREGAME);
        break;
      case EventNames.RESUME:
        TogglePause();
        break;
      case EventNames.QUIT:
        QuitGame();
        break;
    }
  }

  private void PlayAudio(PlayAudioEventArgs eventArgs) {
    if(eventArgs.IsDefaultMusic) {
      if (document_.UserSettingsInfo.MusicEnable) {
        audio_background_player_.Play();
      }
    } else {
      if (document_.UserSettingsInfo.SoundEnable) {
        if(eventArgs.Clip != null) {
          if(eventArgs.IsRepeat) {
            audio_clip_player_.clip = eventArgs.Clip;
            audio_clip_player_.Play();
          } else {
            audio_clip_player_.PlayOneShot(eventArgs.Clip);
          }
        }
      }
    }
  }

  private void LoadScene(LoadSceneEventArgs eventArgs) {
    if (scenes_loaded_.Contains(eventArgs.SceneName)) {
      return;
    }
    if (!eventArgs.IsAdditive) {
      scenes_loaded_.ForEach(x => {
        StartCoroutine(UnloadSceneAsync(x));
      });
    }
    StartCoroutine(LoadSceneAsync(eventArgs.SceneName));
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
  private void UpdateState(GameState state) {
    GameState previousGameState = current_state_;
    current_state_ = state;
    switch (CurrentState) {
      case GameState.PREGAME:
        // Initialize any systems that need to be reset
        Time.timeScale = 1.0f;
        break;
      case GameState.RUNNING:
        //  Unlock player, enemies and input in other systems, update tick if you are managing time
        Time.timeScale = 1.0f;
        break;
      case GameState.PAUSED:
        // Pause player, enemies etc, Lock other input in other systems
        Time.timeScale = 0.0f;
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
    // Clean up application as necessary
    // Maybe save the players game
    Debug.Log("[GameManager] Quit Game.");
    Application.Quit();
  }
  #endregion
  void NotifyEvent(GameEventArgs gameEvent) {
    if (gameEvent != null) {
      game_event_controller_.NotifyEvent(gameEvent);
    }
  }
}
