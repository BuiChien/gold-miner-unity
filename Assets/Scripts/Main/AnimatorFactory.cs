using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFactory : MonoBehaviour {
  public List<RuntimeAnimatorController> Animators;
  public RuntimeAnimatorController Create(string name) {
    RuntimeAnimatorController animatorController = Animators.Find(x => x.name == name);
    return animatorController;
  }
}
