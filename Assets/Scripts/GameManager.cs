using System;
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

  public GameObject[] SystemPrefabs;
  public GameEventHandler GameEvent;

  private List<AsyncOperation> load_operations_;
  private List<GameObject> instanced_system_prefabs_;
  private GameState current_state_ = GameState.PREGAME;
  private string _level_name;

  public GameState CurrentGameState {
    get { return current_state_; }
    private set { current_state_ = value; }
  }

  void Start() {
    instanced_system_prefabs_ = new List<GameObject>();
    load_operations_ = new List<AsyncOperation>();
    InstantiateSystemPrefabs();
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

  void OnLoadOperationComplete(AsyncOperation ao) {
    if (load_operations_.Contains(ao)) {
      load_operations_.Remove(ao);
      if (load_operations_.Count == 0) {
        UpdateState(GameState.RUNNING);
      }
    }
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
    OnGameEvent(new StateChangedEventArgs() { });
  }

  void InstantiateSystemPrefabs() {
    GameObject prefabInstance;
    for (int i = 0; i < SystemPrefabs.Length; ++i) {
      prefabInstance = Instantiate(SystemPrefabs[i]);
      instanced_system_prefabs_.Add(prefabInstance);
    }
  }

  public void LoadLevel(string levelName) {
    AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
    if (ao == null) {
      Debug.LogError("[GameManager] Unable to load level " + levelName);
      return;
    }
    ao.completed += OnLoadOperationComplete;
    load_operations_.Add(ao);
    _level_name = levelName;
  }

  public void UnloadLevel(string levelName) {
    AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
    ao.completed += OnUnloadOperationComplete;
  }

  public void TogglePause() {
    UpdateState(current_state_ == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
  }

  public void RestartGame() {
    UpdateState(GameState.PREGAME);
  }

  public void StartGame() {
    LoadLevel("Main");
  }

  public void QuitGame() {
    // Clean up application as necessary
    // Maybe save the players game
    Debug.Log("[GameManager] Quit Game.");
    Application.Quit();
  }

  void OnGameEvent(GameEventArgs gameEvent) {
    if (gameEvent != null) {
      GameEvent.Invoke(gameEvent);
    }
  }
}
