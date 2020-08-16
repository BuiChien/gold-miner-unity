using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : GameScript {
  [SerializeField]
  private InventoryItem[] item_pickup_list_;

  public bool CanUse {
    set {
      foreach(InventoryItem item in item_pickup_list_) {
        item.CanUse = value;
      }
    }
  }
  void Start() {
    RegisterGameEventController();
  }

  public void SetDisplayItems(List<BoughtItem> items) {
    for(int i = 0; i < item_pickup_list_.Length; i++) {
      if(i < items.Count) {
        if(items[i].Character.InventoryIcon != null) {
          item_pickup_list_[i].Item = items[i].Character;
          item_pickup_list_[i].Count = items[i].Count;
          item_pickup_list_[i].GameEvent.AddListener((e) => {
            BroadcastEvent(e);
          });
        }
      }
    }
  }

  public void AddDisplayItem(ItemPickupSo itemSo) {
    InventoryItem newItem = null;
    foreach (InventoryItem item in item_pickup_list_) {
      if(item.Item.Type == itemSo.Type) {
        newItem = item;
        break;
      }
    }
    if(newItem == null) {
      //newItem 
    }
    newItem.ItemFly();
  }
}
