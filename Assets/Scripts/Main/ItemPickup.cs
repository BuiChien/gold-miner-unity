using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour {
  private ItemPickupSo character_;
  public ItemPickupSo Character {
    get => character_;
    set {
      character_ = value;
      GetComponent<Image>().sprite = value.Icon;
      Price = UnityEngine.Random.Range(character_.MinPrice, character_.MaxPrice);
    }
  }

  public int Price { get; private set; } = 0;
  public string Usage => character_.Usage;
  public string Name => character_.Name;

  public Sprite Icon => character_.Icon;

  public ItemPickupType Type { get => character_.Type; }
  void Start() {

  }

  void Update() {

  }
}
