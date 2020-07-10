using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVictim {
  void DragAway(Transform target);
  void Death();
  bool IsHeavy();
}
