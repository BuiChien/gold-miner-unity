using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenu : MonoBehaviour {
  [SerializeField]
  private LevelTargetPanel level_target_panel_;
  [SerializeField]
  private LevelCompletePanel level_complete_panel_;

  public void HideLevelTarget() {
    gameObject.SetActive(false);
    gameObject.GetComponent<Canvas>().sortingOrder = 0;
    level_target_panel_.Visible = false;
  }

  public void ShowLevelTarget(int level, int targetScore) {
    gameObject.SetActive(true);
    gameObject.GetComponent<Canvas>().sortingOrder = 3;
    level_target_panel_.Visible = true;
    level_target_panel_.Level = level;
    level_target_panel_.TargetScore = targetScore;
  }

  public void ShowLevelComplete(bool isWin, int score) {
    gameObject.SetActive(true);
    gameObject.GetComponent<Canvas>().sortingOrder = 3;
    level_complete_panel_.IsWin = isWin;
    level_complete_panel_.Score = score;
  }
}
