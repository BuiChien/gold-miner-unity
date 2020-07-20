using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItemPickupEventArgs : ButtonEventArgs {
  public ItemPickupSo UseItem { get; private set; }
  public UseItemPickupEventArgs(ItemPickupSo item)
    : base(EventNames.USE_ITEM_PICKUP) {
    UseItem = item;
  }
}

public class InventoryItem : GameScript {
  public bool CanUse { get; set; }
  private ItemPickupSo item_;
  private Button button_;
  public int Count { get; set; }
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
      if(CanUse) {
        if(Count <= 0) {
          button_.interactable = false;
        }
        NotifyEvent(new UseItemPickupEventArgs(item_));
      }
    });
  }
}
