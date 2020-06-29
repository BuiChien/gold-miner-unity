using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChangedEventArgs : GameEventArgs {
  public StateChangedEventArgs() 
    : base(EventNames.STATE_CHANGED) {

  }
}
