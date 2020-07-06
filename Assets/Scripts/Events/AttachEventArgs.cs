using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachEventArgs : GameEventArgs {
  public enum AttachState {
    START,
    END
  }
  public IAttacker Attacker { get; private set; }
  public AttachState State { get; private set; }
  public AttachEventArgs(IAttacker attacker)
    : base(EventNames.PLAYER_ATTACH) {
    Attacker = attacker;
    State = AttachState.START;
  }

  public AttachEventArgs()
  : base(EventNames.PLAYER_ATTACH) {
    Attacker = null;
    State = AttachState.END;
  }
}
