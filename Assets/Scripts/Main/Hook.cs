using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour, IAttacker {
  public enum HookState {
    IDLE,
    DROP,
    PULL,
  }

  public string Name { get ; set; }
  public IGoodsBehavior GoodAttacked { get; set; }
  public HookState State { get; private set; }
  [SerializeField]
  private GameObject hook_;
  [SerializeField]
  private GameObject half_hook_;

  public void DestroyGoodAttached() {
    GoodAttacked.Destroy();
  }

  void Start() {
    State = HookState.IDLE;
  }

  void Update() {
    switch(State) {
      case HookState.IDLE:
        HookIdle();
        break;
      case HookState.DROP:
        HookDrop();
        break;
      case HookState.PULL:
        HookPull();
        break;
    }
  }

  private void HookPull() {

  }

  private void HookDrop() {

  }

  private void HookIdle() {

  }
}
