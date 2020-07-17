using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {
  [SerializeField]
  private ItemPickupSo character_;

  public ItemPickupType Type { get => character_.Type; }
  void Start() {

  }

  void Update() {

  }
}
