using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusPanel : MonoBehaviour {
  public int TimeCounter {
    set {
      txt_time_count_.text = value.ToString();
    }
  }

  public int Level {
    set {
      txt_level_.text = "LEVEL " + value.ToString();
    }
  }

  public int Score {
    set {
      txt_score_.text = "$" + value.ToString();
    }
  }

  public int TargetScore {
    set {
      txt_target_score_.text = "$" + value.ToString();
    }
  }
  [SerializeField]
  private TMP_Text txt_time_count_;
  [SerializeField]
  private TMP_Text txt_score_;
  [SerializeField]
  private TMP_Text txt_target_score_;
  [SerializeField]
  private TMP_Text txt_level_;
}
