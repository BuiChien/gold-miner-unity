using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod : MonoBehaviour {
  [SerializeField]
  private Hook hook_;
  public float RotationSpeed = 3;
  public float AngleRotationMax = 70;
  void Start() {
    hook_.State = Hook.HookState.IDLE;
  }

  void Update() {
    gameObject.GetComponent<LineRenderer>().SetPosition(0, transform.position);
    gameObject.GetComponent<LineRenderer>().SetPosition(1, hook_.gameObject.transform.position);
  }

  void FixedUpdate() {
    if(hook_.State == Hook.HookState.IDLE) {
      transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * RotationSpeed) * AngleRotationMax);
    } 
  }

  public void DropHook() {
    if(hook_.State == Hook.HookState.IDLE) {
      hook_.CalculateVelocity();
      hook_.State = Hook.HookState.DROP;
    }
  }

  public void PullHook() {
    if (hook_.State == Hook.HookState.DROP) {
      hook_.CalculateVelocity();
      hook_.State = Hook.HookState.DROP;
    }
  }
}
