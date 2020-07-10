using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
  private GoodsSpawner good_spawner_;

  void Awake() {
    good_spawner_ = gameObject.GetComponent<GoodsSpawner>();
  }

  void Update() {

  }

  public void LoadLevel(int level) {
    good_spawner_.Spawn(level);
  }
}
