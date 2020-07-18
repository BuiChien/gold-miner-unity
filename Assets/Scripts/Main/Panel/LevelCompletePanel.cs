using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletePanel : MonoBehaviour {
  [SerializeField]
  private PlayerLostPanel lost_panel_;
  [SerializeField]
  private PlayerWinPanel win_panel_;

  public int Score {
    set {
      lost_panel_.Score = value;
      win_panel_.Score = value;
    }
  }

  public bool IsWin {
    set {
      gameObject.SetActive(true);
      if(value) {
        lost_panel_.Visible = false;
        win_panel_.Visible = true;
      } else {
        lost_panel_.Visible = true;
        win_panel_.Visible = false;
      }
    }
  }
}
