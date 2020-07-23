using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IAttacker {
  public const string MyName = "Bomb";
  public bool CanAttach { get; set; }
  public string Name { get => MyName; }
  public Waypoint[] Waypoints;
  private int current_pos_;
  public IVictim Target { 
    get => target_;
    set {
      StartCoroutine(FollowTarget());
    }
  }
  public IVictim target_ { get; set; }

  void Start() {
    current_pos_ = 0;
    transform.position = Waypoints[current_pos_].transform.position;
    CanAttach = true;
  }

  public void OnAttach(IVictim victim) {
    if(Target == victim) {
      target_ = null;
      victim.Death(this);
      Destroy(gameObject, 0.5f);
    }
  }

  IEnumerator FollowTarget() {
    int next_pos = current_pos_ + 1;
    Vector3 startPoint = Waypoints[current_pos_].transform.position;
    Vector3 endPoint = Waypoints[next_pos].transform.position;
    while (target_ != null) {
      if(transform.position != endPoint) {
        transform.position = Vector3.Lerp(startPoint, endPoint, Time.deltaTime);
      } else {
        if(next_pos < Waypoints.Length - 1) {
          current_pos_++;
          next_pos++;
          startPoint = Waypoints[current_pos_].transform.position;
          endPoint = Waypoints[next_pos].transform.position;
        } else { // Finish way points
          current_pos_ = Waypoints.Length - 1;
          startPoint = Waypoints[current_pos_].transform.position;
          endPoint = target_.Position;
        }
      }
      yield return null;
    }
    yield return null;
  }
}
