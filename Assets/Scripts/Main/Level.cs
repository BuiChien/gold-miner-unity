using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
  private Spawner good_spawner_;

  void Awake() {
    good_spawner_ = gameObject.GetComponent<Spawner>();
  }

  void Update() {

  }

  public void LoadLevel(int level, int targetScore) {
    good_spawner_.Spawn(level, targetScore);
  }
}
