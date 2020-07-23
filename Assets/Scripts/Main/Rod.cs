using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod : MonoBehaviour {
  [SerializeField]
  private Hook hook_;
  public float RotationSpeed = 3;
  public float AngleRotationMax = 70;
  public bool IsIdle { get => hook_.IsIdle; }
  public bool CanAttach {
    get => hook_.CanAttach;
    set { hook_.CanAttach = value; } 
  }
  public float Speed { set {
      hook_.Speed = value;
    } 
  }
  public IVictim Victim => hook_.Victim;
  void Start() {

  }

  void Update() {
    gameObject.GetComponent<LineRenderer>().SetPosition(0, transform.position);
    gameObject.GetComponent<LineRenderer>().SetPosition(1, hook_.gameObject.transform.position);
  }

  void FixedUpdate() {
    if(hook_.IsIdle) {
      transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * RotationSpeed) * AngleRotationMax);
    } 
  }

  public void DropHook() {
    hook_.Drop();
  }

  public void Abort() {

  }

  public void PullHook() {
    hook_.Pull();
  }
}
