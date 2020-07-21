using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWinPanel : MonoBehaviour {
  [SerializeField]
  private TMP_Text score_;
  [SerializeField]
  private Button btn_goto_menu_;
  [SerializeField]
  private Button btn_next_level_;
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
    btn_next_level_.onClick.AddListener(OnGoNextLevel);
  }

  private void OnGoNextLevel() {
    GameEventController.Instance.NotifyEvent(new LoadSceneEventArgs(SceneNames.SHOP));
  }

  private void OnGotoMenu() {
    GameEventController.Instance.NotifyEvent(new GameEventArgs(EventNames.SHOW_MENU));
  }
}
