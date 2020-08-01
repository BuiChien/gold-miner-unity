﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameScript {
  [SerializeField]
  private PlayerSo character_;
  class PlayerState {
    public const int IDLE          = 1;
    public const int DROP_HOOK     = 2;
    public const int PULL          = 3;
    public const int PULL_HEAVY    = 4;
    public const int DROP_BOMB     = 5;
    public const int ANGRY         = 6;
    public const int HAPPY         = 7;
  }
  public float HookSpeed {
    set {
      rod_.Speed = value;
    }
  }

  public IVictim Victim => rod_.Victim;

  private Animator animimator_;
  [SerializeField]
  private Rod rod_;
  private StateMachine state_machine_;

  void Start() {
    animimator_ = GetComponent<Animator>();
    state_machine_ = new StateMachine();
    state_machine_.AddState(PlayerState.IDLE, () => {

    }, (e) => {
      //Do nothing
    });
    state_machine_.AddState(PlayerState.DROP_HOOK, () => {
      rod_.DropHook();
    }, (e) => {
      if(rod_.IsIdle) {
        OnChangeState(PlayerState.IDLE);
      }
    });
    state_machine_.AddState(PlayerState.PULL, () => {
      rod_.PullHook();
    }, (e) => {
      if(rod_.IsIdle) {
        NotifyEvent(new GameEventArgs(EventNames.PULL_SUCCESS));
      }
    });
    state_machine_.AddState(PlayerState.PULL_HEAVY, () => {
      rod_.PullHook();
    }, (e) => {
      if (rod_.IsIdle) {
        NotifyEvent(new GameEventArgs(EventNames.PULL_SUCCESS));
      }
    });
    state_machine_.AddState(PlayerState.DROP_BOMB, () => {
      rod_.CancelAttach();
    }, (e) => { 
      if(rod_.IsIdle) {
        OnChangeState(PlayerState.IDLE);
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
    OnChangeState(PlayerState.IDLE);
  }

  void Update() {
    state_machine_.ProcessEvent(null);
  }

  void FixedUpdate() {
    state_machine_.ProcessEvent(null);
  }

  public void DropHook() {
    if(state_machine_.StateId == PlayerState.IDLE) {
      OnChangeState(PlayerState.DROP_HOOK);
    }
  }

  public void UseWeapons(ItemPickupType type) {
    if(type.Equals(ItemPickupType.BOMB)) {
      OnChangeState(PlayerState.DROP_BOMB);
    }
  }

  public void Abort() {
    OnChangeState(PlayerState.IDLE);
    rod_.Abort();
  }

  public void Pull() {
    if (state_machine_.StateId == PlayerState.DROP_HOOK) {
      OnChangeState(PlayerState.PULL);
    }
  }

  public void PullHeavy() {
    if (state_machine_.StateId == PlayerState.DROP_HOOK) {
      OnChangeState(PlayerState.PULL_HEAVY);
    }
  }

  public void Angry() {
    OnChangeState(PlayerState.ANGRY);
  }

  public void Happy() {
    OnChangeState(PlayerState.HAPPY);
  }

  private IEnumerator OnAnimationHandler() {
    yield return new WaitForSeconds(0.5f);
    switch (state_machine_.StateId) {
      case PlayerState.HAPPY:
      case PlayerState.ANGRY:
        OnChangeState(PlayerState.IDLE);
        break;
      default:
        break;
    }
  }

  private void OnChangeState(int stateId) {
    animimator_.SetInteger("State", stateId);
    state_machine_.ChangeState(stateId);
  }
}
