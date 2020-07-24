using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : GameScript {
  private float start_time_;
  public float StartTime {
    get => start_time_; 
    set {
      Counter = value;
      start_time_ = value;
    }
  }
  public float Counter { get; set; }
  public bool IsFire { get => (Counter <= 0) && !pause_; }
  private bool pause_;
  void Start() {
    pause_ = true;
  }

  void Update() {
    if(!pause_) {
      Counter -= Time.deltaTime;
    }
  }

  public void Restart() {
    Counter = StartTime;
    pause_ = false;
  }

  public void Resume() {
    pause_ = false;
  }

  public void Pause() {
    pause_ = true;
  }

  public void Stop() {
    pause_ = true;
    Counter = 0f;
  }
}
