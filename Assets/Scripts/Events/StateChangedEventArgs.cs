using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChangedEventArgs : GameEventArgs {
  public int CurrentState { get; private set; }
  public int NextState { get; private set; }
  public StateChangedEventArgs(int current, int next) 
    : base(EventNames.STATE_CHANGED) {
    CurrentState = current;
    NextState = next;
  }
}
