using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSettings : ISetting {
  public bool MusicEnable { get; set; } = true;
  public bool SoundEnable { get; set; } = true;

  public void NullToDefault() {

  }
}
