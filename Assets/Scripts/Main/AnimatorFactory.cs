using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFactory : MonoBehaviour {
  public List<RuntimeAnimatorController> Animators;
  public RuntimeAnimatorController Create(string name) {
    RuntimeAnimatorController animatorController = null;
    if(name.Equals("MousePlus")) {
      List<RuntimeAnimatorController> mouses = Animators.FindAll(x => x.name.Contains(name));
      int index = Random.Range(0, mouses.Count);
      animatorController = mouses[index];
    } else {
      animatorController = Animators.Find(x => x.name == name);
    }    
    return animatorController;
  }
}
