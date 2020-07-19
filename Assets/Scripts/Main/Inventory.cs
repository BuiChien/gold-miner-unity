using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : GameScript {
  [SerializeField]
  private InventoryItem[] item_pickup_list_;
  void Start() {
    RegisterGameEventController();
  }

  public void SetDisplayItems(List<ItemPickupSo> items) {
    for(int i = 0; i < item_pickup_list_.Length; i++) {
      if(i < items.Count) {
        if(items[i].InventoryIcon != null) {
          item_pickup_list_[i].Item = items[i];
          item_pickup_list_[i].GameEvent.AddListener((e) => {
            BroadcastEvent(e);
          });
        }
      }
    }
  }
}
