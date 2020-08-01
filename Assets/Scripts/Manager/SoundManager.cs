﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {
  AudioSource background_player_;
  AudioSource clip_player_;
  Dictionary<string, AudioSource> clip_repeat_players_;
  private Document document_;
  private List<string> repeat_pause_list_;
  void Awake() {
    background_player_ = gameObject.AddComponent<AudioSource>();
    clip_player_ = gameObject.AddComponent<AudioSource>();
    clip_repeat_players_ = new Dictionary<string, AudioSource>();
    repeat_pause_list_ = new List<string>();
    document_ = Document.Instance;
  }

  public void PlayBackground(AudioClip clip) {
    if(background_player_.isPlaying) {
      background_player_.Stop();
    }
    background_player_.clip = clip;
    background_player_.loop = true;
    background_player_.volume = 0.5f;
    if (document_.MusicEnable) {
      background_player_.Play();
    }
  }

  public void PauseBackground() {
    background_player_.Pause();
  }

  public void ResumeBackground() {
    if(!background_player_.isPlaying) {
      background_player_.UnPause();
    }
  }

  public void StopBackground() {
    background_player_.Stop();
    background_player_.clip = null;
  }

  public void PlayClip(AudioClip clip, bool isOverride = false) {
    if(isOverride) {
      clip_player_.Stop();
    }
    clip_player_.PlayOneShot(clip, 1.0f);
  }

  public void PauseClip() {
    if(clip_player_.isPlaying) {
      clip_player_.Pause();
    }
  }

  public void ResumeClip() {
    if (!clip_player_.isPlaying) {
      clip_player_.UnPause();
    }
  }

  public void PlayRepeatClip(AudioClip clip) {
    if(!clip_repeat_players_.ContainsKey(clip.name)) {
      AudioSource audio = gameObject.AddComponent<AudioSource>();
      audio.clip = clip;
      audio.loop = true;
      audio.volume = 1.0f;
      audio.Play();
      clip_repeat_players_.Add(clip.name, audio);
    } else {
      if(!clip_repeat_players_[clip.name].isPlaying) {
        if(repeat_pause_list_.Contains(clip.name)) {
          repeat_pause_list_.Remove(clip.name);
        }
        clip_repeat_players_[clip.name].Play();
      }
    }
  }

  public void StopRepeatClip(AudioClip clip) {
    if (clip_repeat_players_.ContainsKey(clip.name)) {
      clip_repeat_players_[clip.name].Pause();
    }
  }

  public void PauseAllRepeatClip() {
    foreach (var audio in clip_repeat_players_) {
      if(audio.Value.isPlaying) {
        audio.Value.Pause();
        repeat_pause_list_.Add(audio.Key);
      }
    }
  }

  public void ResumeAllRepeatClip() {
    foreach (var audio in clip_repeat_players_) {
      if(repeat_pause_list_.Contains(audio.Key)) {
        audio.Value.UnPause();
        repeat_pause_list_.Remove(audio.Key);
      }
    }
  }

  public void StopAllRepeatClip() {
    foreach(var audio in clip_repeat_players_) {
      audio.Value.Stop();
      Destroy(audio.Value);
    }
    clip_repeat_players_.Clear();
    repeat_pause_list_.Clear();
  }

  public void StopClip() {
    clip_player_.Stop();
  }
}
