using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : GameScript {
  private Animator animator_;
  [SerializeField]
  private Button btn_music_;
  [SerializeField]
  private Button btn_sound_;
  [SerializeField]
  private Button btn_close_;
  [SerializeField]
  private Sprite music_enable_image_;
  [SerializeField]
  private Sprite music_disable_image_;
  [SerializeField]
  private Sprite sound_enable_image_;
  [SerializeField]
  private Sprite sound_disable_image_;
  Document document_;
  private bool visible_;

  public bool Visible {
    get => visible_;
    set {
      visible_ = value;
      animator_.SetBool("In", value);
    }
  }

  private bool music_enable_ {
    get => document_.UserSettingsInfo.MusicEnable;
    set {
      document_.UserSettingsInfo.MusicEnable = value;
      btn_music_.image.sprite = value ? music_enable_image_ : music_disable_image_;
    }
  }

  private bool sound_enable_ {
    get => document_.UserSettingsInfo.SoundEnable;
    set {
      document_.UserSettingsInfo.SoundEnable = value;
      btn_sound_.image.sprite = value ? sound_enable_image_ : sound_disable_image_;
    }
  }

  void Awake() {
    visible_ = false;
    document_ = Document.Instance;
    RegisterGameEventController();
    animator_ = GetComponent<Animator>();
    music_enable_ = document_.UserSettingsInfo.MusicEnable;
    sound_enable_ = document_.UserSettingsInfo.SoundEnable;
    btn_music_.onClick.AddListener(() => {
      BroadcastEvent(new ButtonEventArgs(""));
      music_enable_ = !music_enable_;
      if(music_enable_) {
        SoundManager.Instance.ResumeBackground();
      } else {
        SoundManager.Instance.PauseBackground();
      }
    });
    btn_sound_.onClick.AddListener(() => {
      BroadcastEvent(new ButtonEventArgs(""));
      sound_enable_ = !sound_enable_;
    });
    btn_close_.onClick.AddListener(() => {
      Visible = false;
      NotifyEvent(new ButtonEventArgs("CloseSetting"));
    });
  }
}
