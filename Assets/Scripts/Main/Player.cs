using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameScript {
  [SerializeField]
  private PlayerSo character_;
  class PlayerState {
    public const string IDLE          = "Idle";
    public const string DROP_HOOK     = "DropHook";
    public const string PULL          = "Pull";
    public const string PULL_HEAVY    = "PullHeavy";
    public const string ANGRY         = "Angry";
    public const string HAPPY         = "Happy";
  }
  public float HookSpeed {
    set {
      rod_.Speed = value;
    }
  }
  [SerializeField]
  private Animator animimator_;
  [SerializeField]
  private Rod rod_;
  private StateMachine state_machine_;

  void Start() {
    state_machine_ = new StateMachine();
    state_machine_.AddState(PlayerState.IDLE, () => {
      rod_.CanAttach = true;
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
        if (rod_.CanAttach) {
          state_machine_.ChangeState(PlayerState.HAPPY);
          NotifyEvent(new GameEventArgs(EventNames.PULL_SUCCESS));
        } else {
          state_machine_.ChangeState(PlayerState.IDLE);
        }
      }
    });
    state_machine_.AddState(PlayerState.PULL_HEAVY, () => {
      rod_.PullHook();
    }, (e) => {
      if (rod_.IsIdle) {
        if(rod_.CanAttach) {
          state_machine_.ChangeState(PlayerState.ANGRY);
          NotifyEvent(new GameEventArgs(EventNames.PULL_SUCCESS));
        } else {
          state_machine_.ChangeState(PlayerState.IDLE);
        }
      }
    });
    state_machine_.AddState(PlayerState.ANGRY, () => {
      StartCoroutine(OnAnimationHandler());
    }, (e) => {
      //Do nothing
    });
    state_machine_.AddState(PlayerState.HAPPY, () => {
      StartCoroutine(OnAnimationHandler());
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

  public void UseWeapons() {
    rod_.CanAttach = false;
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

  private IEnumerator OnAnimationHandler() {
    yield return new WaitForSeconds(1f);
    switch (state_machine_.StateName) {
      case PlayerState.HAPPY:
        state_machine_.ChangeState(PlayerState.IDLE);
        break;
      case PlayerState.ANGRY:
        state_machine_.ChangeState(PlayerState.IDLE);
        break;
      default:
        break;
    }
  }
}
