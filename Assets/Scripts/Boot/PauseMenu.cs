using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : GameScript {
  public Button ResumeButton;
  public Button QuitButton;
  public Button MenuButton;
  public Button MusicButton;
  public Button SoundButton;
  public Button LanguageButton;
  [SerializeField]
  private Sprite music_enable_image_;
  [SerializeField]
  private Sprite music_disable_image_;
  [SerializeField]
  private Sprite sound_enable_image_;
  [SerializeField]
  private Sprite sound_disable_image_;
  [SerializeField]
  private Sprite language_en_image_;
  [SerializeField]
  private Sprite language_vi_image_;
  private Document document_;
  private bool music_enable_ {
    get => document_.MusicEnable;
    set {
      document_.MusicEnable = value;
      MusicButton.image.sprite = value ? music_enable_image_ : music_disable_image_;
    }
  }

  private bool sound_enable_ {
    get => document_.SoundEnable;
    set {
      document_.SoundEnable = value;
      SoundButton.image.sprite = value ? sound_enable_image_ : sound_disable_image_;
    }
  }

  void Awake() {
    RegisterGameEventController();
    document_ = Document.Instance;
    music_enable_ = document_.MusicEnable;
    sound_enable_ = document_.SoundEnable;
    ResumeButton.onClick.AddListener(() => {
      BroadcastEvent(new ButtonEventArgs(EventNames.RESUME));
    });
    QuitButton.onClick.AddListener(() => {
      BroadcastEvent(new ButtonEventArgs(EventNames.QUIT));
    });
    MenuButton.onClick.AddListener(() => {
      BroadcastEvent(new ButtonEventArgs(EventNames.SHOW_MENU));
    });
    MusicButton.onClick.AddListener(() => {
      music_enable_ = !music_enable_;
      BroadcastEvent(new ButtonEventArgs(EventNames.MUSIC_SETTING_CHANGED));
      if (music_enable_) {
        SoundManager.Instance.ResumeBackground();
      } else {
        SoundManager.Instance.PauseBackground();
      }
    });
    SoundButton.onClick.AddListener(() => {
      BroadcastEvent(new ButtonEventArgs(EventNames.SOUND_SETTING_CHANGED));
      sound_enable_ = !sound_enable_;
    });
    //TODO: Support multilange
    LanguageButton.interactable = false;
    LanguageButton.onClick.AddListener(() => {
      BroadcastEvent(new ButtonEventArgs(EventNames.LANGUAGE_SETTING_CHANGED));
    });
  }
}
