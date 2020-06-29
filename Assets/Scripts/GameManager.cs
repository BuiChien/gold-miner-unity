using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {
  public enum GameState {
    PREGAME,
    RUNNING,
    PAUSED
  }

  private GameEventController game_event_controller_;
  private List<string> scene_loaded_;
  private GameState current_state_ = GameState.PREGAME;
  AudioSource audio_background_player_;
  AudioSource audio_clip_player_;

  private void Awake() {
    audio_background_player_ = Instance.gameObject.AddComponent<AudioSource>();
    audio_clip_player_ = Instance.gameObject.AddComponent<AudioSource>();
  }

  void Start() {
    scene_loaded_ = new List<string>();
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

  private void OnGameEventHandler(GameEventArgs gameEvent) {
    switch(gameEvent.Name) {
      case EventNames.LOAD_SCENE:
        LoadScene(((LoadSceneEventArgs)gameEvent).SceneName);
        break;
    }
  }

  public GameState CurrentGameState {
    get { return current_state_; }
    private set { current_state_ = value; }
  }

  void OnLoadOperationComplete(AsyncOperation ao) {

  }

  void OnUnloadOperationComplete(AsyncOperation ao) {
    // Clean up level is necessary, go back to main menu
  }

  void UpdateState(GameState state) {
    GameState previousGameState = current_state_;
    current_state_ = state;
    switch (CurrentGameState) {
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
    OnGameEvent(new StateChangedEventArgs((int)previousGameState, (int)current_state_));
  }

  public void LoadScene(string name) {
    StartCoroutine(LoadSceneAsync(name));
  }

  public IEnumerator LoadSceneAsync(string levelName) {
    AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
    if (ao == null) {
      Debug.LogError("[GameManager] Unable to load level " + levelName);
      yield return null;
    }
    while (!ao.isDone) {
      yield return null;
    }
  }

  public void UnloadScene(string levelName) {
    AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
    ao.completed += OnUnloadOperationComplete;
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

  void OnGameEvent(GameEventArgs gameEvent) {
    if (gameEvent != null) {
      game_event_controller_.NotifyEvent(gameEvent);
    }
  }
}
