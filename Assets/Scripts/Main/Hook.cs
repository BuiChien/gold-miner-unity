using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour, IAttacker {
  public enum HookState {
    IDLE,
    DROP,
    PULL,
  }

  public string Name { get ; set; }
  public IGoodsBehavior GoodAttacked { get; set; }
  public HookState State { get; set; }
  [SerializeField]
  private GameObject hook_;
  [SerializeField]
  private GameObject half_hook_;
  private Vector3 original_position_;
  private float speed_;
  private Vector2 velocity_;
  [SerializeField]
  public Transform original_coordinates_;

  private bool IsVisible {
    get => gameObject.GetComponent<Renderer>().isVisible;
  }

  void Start() {
    original_position_ = transform.localPosition;
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
  
  public void DestroyGoodAttached() {
    GoodAttacked.Destroy();
  }

  public void CalculateVelocity() {
    velocity_ = new Vector2(transform.position.x - original_coordinates_.position.x,
                       transform.position.y - original_coordinates_.position.y);
    velocity_.Normalize();
    speed_ = 3;
  }

  private void HookPull() {
    if(IsVisible) {
      hook_.SetActive(false);
      half_hook_.SetActive(true);
      GetComponent<Rigidbody2D>().velocity = velocity_ * speed_;
    } else {
      // hook is out of cammera view, quick pull
      GetComponent<Rigidbody2D>().velocity = velocity_ * speed_ * 3;
    }
  }

  private void HookDrop() {
    // hook is out of cammera view, move to pull state
    if(IsVisible == false) {
      State = HookState.PULL;
    }
    GetComponent<Rigidbody2D>().velocity = velocity_ * speed_;
  }

  private void HookIdle() {
    hook_.SetActive(true);
    half_hook_.SetActive(false);
  }
}
