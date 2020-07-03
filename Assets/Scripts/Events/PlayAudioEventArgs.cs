using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioEventArgs : GameEventArgs {
  public AudioClip Clip { get; private set; }
  public bool IsRepeat { get; private set; }
  public bool IsDefaultMusic { get; private set; }
  public PlayAudioEventArgs(AudioClip clip, bool isRepeat) 
    : base(EventNames.PLAY_AUDIO) {
    Clip = clip;
    IsRepeat = isRepeat;
    IsDefaultMusic = false;
  }

  public PlayAudioEventArgs()
    : this(null, true) {
    IsDefaultMusic = true;
  }
}
