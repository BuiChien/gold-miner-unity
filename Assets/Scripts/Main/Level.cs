using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
  private Spawner spawner_;

  void Awake() {
    spawner_ = gameObject.GetComponent<Spawner>();
  }

  void Update() {

  }

  public void LoadLevel(int level, int targetScore) {
    spawner_.Spawn(level, targetScore);
  }

  public bool IsFinish() {
    return spawner_.Count <= 0;
  }
}
