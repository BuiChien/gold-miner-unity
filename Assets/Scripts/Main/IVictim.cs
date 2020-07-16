using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVictim {
  int ScoreAmount { get; }
  bool IsHeavy { get; }
  void DragAway(Transform target);
  void Death();
}
