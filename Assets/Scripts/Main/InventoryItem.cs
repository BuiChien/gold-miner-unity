using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItemPickupEventArgs : ButtonEventArgs {
  public ItemPickupSo UseItem { get; private set; }
  public UseItemPickupEventArgs(ItemPickupSo item)
    : base("UseItemPickup") {
    UseItem = item;
  }
}

public class InventoryItem : GameScript {
  private ItemPickupSo item_;
  private Button button_;
  public ItemPickupSo Item {
    get => item_;
    set {
      item_ = value;
      GetComponent<Image>().color = Color.white;
      GetComponent<Image>().sprite = value.InventoryIcon;
    }
  }

  void Start() {
    GetComponent<Image>().color = Color.clear;
    button_ = GetComponent<Button>();
    button_.onClick.AddListener(() => {
      button_.interactable = false;
      NotifyEvent(new UseItemPickupEventArgs(item_));
    });
  }
}
