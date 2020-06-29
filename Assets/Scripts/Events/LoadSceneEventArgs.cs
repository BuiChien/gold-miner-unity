using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneEventArgs : GameEventArgs {
  public string SceneName { get; private set; }
  public bool IsAdditive { get; private set; }
  public LoadSceneEventArgs(string name, bool isAdditive = false) 
    : base(EventNames.LOAD_SCENE) {
    SceneName = name;
    IsAdditive = isAdditive;
  }
}
