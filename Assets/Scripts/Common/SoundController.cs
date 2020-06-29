using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : Singleton<SoundController> {
  AudioSource BackgroundPlayer;
  AudioSource ClipPlayer;
  public bool BackgroundMusic {
    get => BackgroundPlayer.enabled;
    set {
      BackgroundPlayer.enabled = value;
    }
  }

  public AudioClip BackgroundClip {
    get => BackgroundPlayer.clip;
    set { 
      BackgroundPlayer.clip = value;
    }
  }

  public bool ClipMusic {
    get => ClipPlayer.enabled;
    set {
      ClipPlayer.enabled = value;
    }
  }

  void Awake() {
    BackgroundPlayer = Instance.gameObject.AddComponent<AudioSource>();
    ClipPlayer = Instance.gameObject.AddComponent<AudioSource>();
  }

  public void PlayClip(AudioClip clip) {
    if(clip != null) {
      ClipPlayer.PlayOneShot(clip);
    }
  }

  public void PlayBackgroundMusic() {
    if(BackgroundPlayer.clip != null) {
      BackgroundPlayer.Play();
    }
  }
}
