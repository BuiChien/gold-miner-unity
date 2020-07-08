using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
  // Start is called before the first frame update
  GridLayout grid_layout_;

  void Start() {
    gameObject.AddComponent<GridLayout>();
    grid_layout_ = gameObject.GetComponent<GridLayout>();
  }

  // Update is called once per frame
  void Update() {

  }
}
