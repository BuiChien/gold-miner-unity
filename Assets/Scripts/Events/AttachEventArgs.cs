using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachEventArgs : GameEventArgs {

  public IAttacker Attacker { get; private set; }
  public IVictim Victim { get; private set; }
  public AttachEventArgs(IAttacker attacker, IVictim victim)
    : base(EventNames.ATTACK) {
    Attacker = attacker;
    Victim = victim;
  }
}
