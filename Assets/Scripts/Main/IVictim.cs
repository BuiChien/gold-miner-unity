using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVictim {
  int Id { get; }
  string Tag { get; }
  Vector3 Position { get; }
  int ScoreAmount { get; }
  bool IsHeavy { get; }
  void DragAway(Transform target);
  void Death(IAttacker attacker);
}
