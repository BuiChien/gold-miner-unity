using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
  [SerializeField]
  private GoodsSpawner good_spawner_;
  private GridLayout grid_layout_;

  void Start() {
    gameObject.AddComponent<GridLayout>();
    grid_layout_ = gameObject.GetComponent<GridLayout>();
  }

  void Update() {

  }

  public void LoadLevel(int level) {
    good_spawner_.Spawn(level);
  }
}
