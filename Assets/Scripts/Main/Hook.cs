using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour, IAttacker {
  internal class HookState {
    public const string IDLE = "Idle";
    public const string DROP = "Drop";
    public const string PULL = "Pull";
  }
  public float Speed { get; set; }
  private IVictim victim_ { get; set; }
  [SerializeField]
  private GameObject hook_;
  [SerializeField]
  private GameObject half_hook_;
  private Vector3 original_position_;
  private Vector2 velocity_;
  [SerializeField]
  public Transform original_coordinates_;

  public bool IsIdle { get => state_machine_.StateName.Equals(HookState.IDLE); }

  private StateMachine state_machine_;

  private bool IsVisible {
    get => gameObject.GetComponent<Renderer>().isVisible;
  }

  void Start() {
    state_machine_ = new StateMachine();
    state_machine_.AddState(HookState.IDLE, () => {
      hook_.SetActive(true);
      half_hook_.SetActive(false);
      original_position_ = transform.localPosition;
    }, (e) => {
      //Do nothing
    });

    state_machine_.AddState(HookState.DROP, () => {
      CalculateVelocity();
      GetComponent<Rigidbody2D>().velocity = velocity_ * Speed;
    }, (e) => {
      // hook is out of cammera view, move to pull state
      if (IsVisible == false) {
        state_machine_.ChangeState(HookState.PULL);
      }
    });

    state_machine_.AddState(HookState.PULL, () => {
      velocity_ = -velocity_;
      if (IsVisible) {
        hook_.SetActive(false);
        half_hook_.SetActive(true);
        GetComponent<Rigidbody2D>().velocity = velocity_ * Speed;
      } else {
        GetComponent<Rigidbody2D>().velocity = velocity_ * Speed * 3;
      }
    }, (e) => {
      if(victim_ != null) {
        victim_.DragAway(transform);
      }
      if (transform.localPosition.y > original_position_.y) {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.localPosition = original_position_;
        if(victim_ != null) {
          victim_.Death();
          victim_ = null;
        }
        state_machine_.ChangeState(HookState.IDLE);
      }
    });
    state_machine_.ChangeState(HookState.IDLE);
  }

  void Update() {
    state_machine_.ProcessEvent(null);
  }

  void FixedUpdate() {
    state_machine_.ProcessEvent(null);
  }

  public void Drop() {
    if(state_machine_.StateName == HookState.IDLE) {
      state_machine_.ChangeState(HookState.DROP);
    }
  }

  public void Pull() {
    if (state_machine_.StateName == HookState.DROP) {
      state_machine_.ChangeState(HookState.PULL);
    }
  }
  
  private void CalculateVelocity() {
    velocity_ = new Vector2(transform.position.x - original_coordinates_.position.x,
                       transform.position.y - original_coordinates_.position.y);
    velocity_.Normalize();
  }

  public void OnAttach(IVictim victim) {
    victim_ = victim;
  }
}
