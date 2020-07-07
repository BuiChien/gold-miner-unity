using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameScript {
  public enum PlayerState {
    IDLE,
    DROP_HOOK,
    PULL,
    PULL_HEAVY,
    ANGRY,
    HAPPY
  }
  public PlayerState State { get; set; }
  [SerializeField]
  private Animator animimator_;
  [SerializeField]
  private Rod rod_;
  void Start() {
    State = PlayerState.IDLE;
  }

  void Update() {
    switch (State) {
      case PlayerState.IDLE:
        PlayerIdle();
        break;
      case PlayerState.DROP_HOOK:
        PlayerDropHook();
        break;
      case PlayerState.PULL:
        PlayerPull();
        break;
      case PlayerState.PULL_HEAVY:
        PlayerPullHeavy();
        break;
      case PlayerState.ANGRY:
        PlayerAngry();
        break;
      case PlayerState.HAPPY:
        PlayerHappy();
        break;
      default:
        break;
    }
  }

  private void PlayerIdle() {

  }

  private void PlayerDropHook() {
    rod_.DropHook();
  }

  private void PlayerPull() {
    rod_.PullHook();
  }

  private void PlayerPullHeavy() {
    rod_.PullHook();
  }

  private void PlayerAngry() {

  }

  private void PlayerHappy() {

  }
}
