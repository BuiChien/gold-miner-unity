using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameScript {
  class PlayerState {
    public const string IDLE          = "Idle";
    public const string DROP_HOOK     = "DropHook";
    public const string PULL          = "Pull";
    public const string PULL_HEAVY    = "PullHeavy";
    public const string ANGRY         = "Angry";
    public const string HAPPY         = "Happy";
  }
  [SerializeField]
  private Animator animimator_;
  [SerializeField]
  private Rod rod_;
  private StateMachine state_machine_;

  void Start() {
    state_machine_ = new StateMachine();
    state_machine_.AddState(PlayerState.IDLE, () => {

    }, (e) => {
      //Do nothing
    });
    state_machine_.AddState(PlayerState.DROP_HOOK, () => {
      rod_.DropHook();
    }, (e) => {
      if(rod_.IsIdle) {
        state_machine_.ChangeState(PlayerState.IDLE);
      }
    });
    state_machine_.AddState(PlayerState.PULL, () => {
      rod_.PullHook();
    }, (e) => {
      if(rod_.IsIdle) {
        state_machine_.ChangeState(PlayerState.IDLE);
      }
    });
    state_machine_.AddState(PlayerState.PULL_HEAVY, () => {
      rod_.PullHook();
    }, (e) => {
      //Do nothing
    });
    state_machine_.AddState(PlayerState.ANGRY, () => {

    }, (e) => {
      //Do nothing
    });
    state_machine_.AddState(PlayerState.HAPPY, () => {

    }, (e) => {
      //Do nothing
    });
    state_machine_.ChangeState(PlayerState.IDLE);
  }

  void Update() {
    state_machine_.ProcessEvent(null);
  }

  void FixedUpdate() {
    state_machine_.ProcessEvent(null);
  }

  public void DropHook() {
    if(state_machine_.StateName == PlayerState.IDLE) {
      state_machine_.ChangeState(PlayerState.DROP_HOOK);
    }
  }

  public void Pull() {
    if (state_machine_.StateName == PlayerState.DROP_HOOK) {
      state_machine_.ChangeState(PlayerState.PULL);
    }
  }

  public void PullHeavy() {
    if (state_machine_.StateName == PlayerState.DROP_HOOK) {
      state_machine_.ChangeState(PlayerState.PULL);
    }
  }

  public void Angry() {
    if (state_machine_.StateName == PlayerState.PULL_HEAVY) {
      state_machine_.ChangeState(PlayerState.ANGRY);
    }
  }

  public void Happy() {
    if (state_machine_.StateName == PlayerState.PULL) {
      state_machine_.ChangeState(PlayerState.HAPPY);
    }
  }
}
