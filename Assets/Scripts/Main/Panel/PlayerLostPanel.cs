using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLostPanel : MonoBehaviour {
  [SerializeField]
  private TMP_Text score_;
  [SerializeField]
  private Button btn_goto_menu_;
  [SerializeField]
  private Button btn_retry_level_;
  [SerializeField]
  private Animator animator_;
  public int Score {
    set {
      score_.text = "$" + value.ToString();
    }
  }

  public bool Visible {
    set {
      gameObject.SetActive(value);
    }
  }

  void Awake() {
    btn_goto_menu_.onClick.AddListener(OnGotoMenu);
    btn_retry_level_.onClick.AddListener(OnRetryPlayLevel);
  }

  private void OnRetryPlayLevel() {
    GameEventController.Instance.NotifyEvent(new ButtonEventArgs(EventNames.RETRY_LEVEL));
  }

  private void OnGotoMenu() {
    GameEventController.Instance.NotifyEvent(new ButtonEventArgs(EventNames.SHOW_MENU));
  }
}
