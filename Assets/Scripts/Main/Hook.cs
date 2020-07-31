using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour, IAttacker {
  public const string MyName = "Hook";
  internal class HookState {
    public const int IDLE = 1;
    public const int DROP = 2;
    public const int PULL = 3;
    public const int CANCEL = 4;
  }
  public float Speed { get; set; }
  public IVictim Target { get; set; }
  [SerializeField]
  private GameObject hook_;
  [SerializeField]
  private GameObject half_hook_;
  private Vector3 original_position_;
  private Vector2 velocity_;
  [SerializeField]
  public Transform original_coordinates_;
  private bool pull_without_victim_;

  public bool IsIdle { get => state_machine_.StateId.Equals(HookState.IDLE); }

  private StateMachine state_machine_;
  private bool IsVisible {
    get => gameObject.GetComponent<Renderer>().isVisible;
  }

  public string Name { get => MyName; }

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
      GetComponent<Rigidbody2D>().velocity = velocity_ * 6;
      pull_without_victim_ = false;
    }, (e) => {
      // hook is out of cammera view, move to pull state
      if (IsVisible == false) {
        pull_without_victim_ = true;
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
        GetComponent<Rigidbody2D>().velocity = velocity_ * Speed * 5;
      }
    }, (e) => {
      if(Target != null) {
        Target.DragAway(transform);
      }
      if (transform.localPosition.y > original_position_.y) {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.localPosition = original_position_;
        if(Target != null) {
          Target.Death(this);
        }
        Target = null;
        state_machine_.ChangeState(HookState.IDLE);
      }
    });

    state_machine_.AddState(HookState.CANCEL, () => {
      CalculateVelocity();
      velocity_ = -velocity_;
      GetComponent<Rigidbody2D>().velocity = velocity_ * Speed * 6;
    }, (e) => {
      if (transform.localPosition.y > original_position_.y) {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.localPosition = original_position_;
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
    if(state_machine_.StateId == HookState.IDLE) {
      state_machine_.ChangeState(HookState.DROP);
    }
  }

  public void Pull() {
    if (state_machine_.StateId == HookState.DROP) {
      state_machine_.ChangeState(HookState.PULL);
    }
  }
  
  private void CalculateVelocity() {
    velocity_ = new Vector2(transform.position.x - original_coordinates_.position.x,
                       transform.position.y - original_coordinates_.position.y);
    velocity_.Normalize();
  }

  public bool OnAttach(IVictim victim) {
    bool isOk = !pull_without_victim_ && Target == null;
    if(isOk) {
      Target = victim;
    }
    return isOk;
  }

  public void CancelAttach() {
    if(state_machine_.StateId == HookState.PULL) {
      Target = null;
      state_machine_.ChangeState(HookState.CANCEL);
    }
  }

  public void Abort() {
    if (state_machine_.StateId != HookState.IDLE) {
      state_machine_.ChangeState(HookState.IDLE);
    }
    if(Target != null) {
      Target.Death(this);
    }
  }
}
