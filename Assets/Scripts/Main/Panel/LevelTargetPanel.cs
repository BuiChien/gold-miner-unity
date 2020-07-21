using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTargetPanel : MonoBehaviour {
  [SerializeField]
  private TMP_Text level_;
  [SerializeField]
  private TMP_Text target_score_;
  [SerializeField]
  private Animator animator_;
  public int Level {
    set {
      level_.text = "LEVEL" + value.ToString();
    }
  }

  public int TargetScore {
    set {
      target_score_.text = "$" + value.ToString();
    }
  }

  public bool Visible {
    set {
      gameObject.SetActive(value);
    }
  }
}
