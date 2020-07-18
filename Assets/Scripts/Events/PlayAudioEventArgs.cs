using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioEventArgs : GameEventArgs {
  public AudioClip Clip { get; private set; }
  public bool IsRepeat { get; private set; }
  public bool IsDefaultMusic { get; private set; }
  public bool IsStop { get; private set; }
  public PlayAudioEventArgs(AudioClip clip, bool isRepeat = false,
    bool isStop = false, bool isDefault = false) 
    : base(EventNames.PLAY_AUDIO) {
    Clip = clip;
    IsRepeat = isRepeat;
    IsDefaultMusic = isDefault;
    IsStop = isStop;
  }

  public PlayAudioEventArgs()
    : this(null, true, false) {
    IsDefaultMusic = true;
  }

  public PlayAudioEventArgs(bool isDefault, bool isStop)
  : this(null, false, isStop) {
    IsDefaultMusic = isDefault;
  }
}
