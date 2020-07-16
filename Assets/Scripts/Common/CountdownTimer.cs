﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : GameScript {
  public float StartTime { get; set; }
  public float Counter { get; private set; }
  public bool IsFire { get => (Counter <= 0) && !pause_; }
  private bool pause_;
  void Start() {
    Counter = StartTime;
    pause_ = true;
  }

  void Update() {
    if(!pause_) {
      Counter -= UnityEngine.Time.deltaTime;
    }
  }

  public void Restart() {
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
