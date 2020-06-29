using System;
using UnityEngine.Events;

[System.Serializable]
public class GameEventArgs : EventArgs {
  public string Name { get; private set; }

  public object OptionData;

  public GameEventArgs(string name) {
    Name = name;
  }
}

[System.Serializable]
public class GameEventHandler : UnityEvent<GameEventArgs> {

}