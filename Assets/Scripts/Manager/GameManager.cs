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
  [SerializeField]
  private AudioClip button_click_clip_ = null;
  private Document document_;
  private SoundManager sound_manager_;
  #endregion

  #region UnityFuncs
  private void Awake() {
    document_ = Document.Instance;
    sound_manager_ = SoundManager.Instance;
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

  void OnApplicationPause(bool pause) {
    document_.SaveData();
  }

  void OnApplicationQuit() {
    document_.SaveData();
  }

  #endregion

  #region ProcessExternalEvent
  private void OnGameEventHandler(GameEventArgs gameEvent) {
    if(gameEvent.IsButtonClick && document_.SoundEnable) {
      SoundManager.Instance.PlayClip(button_click_clip_);
    }
    switch (gameEvent.Name) {
      case EventNames.LOAD_SCENE:
        LoadScene((LoadSceneEventArgs)gameEvent);
        UpdateState(GameState.RUNNING);
        break;
      case EventNames.RESUME:
        if(current_state_ == GameState.PAUSED) {
          TogglePause();
        }
        break;
      case EventNames.SHOW_MENU:
        Time.timeScale = 1.0f;
        sound_manager_.StopAllRepeatClip();
        sound_manager_.StopClip();
        UnloadAll();
        break;
      case EventNames.QUIT:
        QuitGame();
        break;
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
  private void UpdateState(GameState state) {
    GameState previousGameState = current_state_;
    current_state_ = state;
    switch (CurrentState) {
      case GameState.PREGAME:
        break;
      case GameState.RUNNING:
        //TODO: Audio restart
        Time.timeScale = 1.0f;
        break;
      case GameState.PAUSED:
        Time.timeScale = 0.0f;
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
