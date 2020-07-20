using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IAttacker {
  public const string MyName = "Bomb";
  public bool CanAttach { get; set; }
  public string Name { get => MyName; }

  void Start() {
    CanAttach = true;
  }

  public void OnAttach(IVictim victim) {
    victim.Death(this);
    Destroy(gameObject);
  }

  void Update() {

  }
}
