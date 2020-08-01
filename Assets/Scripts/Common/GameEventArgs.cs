using System;
using UnityEngine.Events;

[System.Serializable]
public class GameEventArgs : EventArgs {
  public string Name { get; private set; }

  public object OptionData { get; set; }
  public bool IsButtonClick { get; set; }

  public GameEventArgs(string name, bool buttonClicked = false) {
    Name = name;
    IsButtonClick = buttonClicked;
  }
}

[System.Serializable]
public class GameEventHandler : UnityEvent<GameEventArgs> {

}