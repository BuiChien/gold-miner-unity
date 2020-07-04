using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUserSettings", menuName = "Scriptable/UserSettings", order = 3)]
public class UserSettings : ScriptableObject {
  public bool MusicEnable;
  public bool SoundEnable;
}
