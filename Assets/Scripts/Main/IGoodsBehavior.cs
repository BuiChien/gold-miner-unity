using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoodsBehavior {
  void SetFollowTarget(GameObject target);
  void Destroy();
}
