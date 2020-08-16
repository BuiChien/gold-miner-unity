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
      GetComponent<Image>().sprite = value.InventoryIcon;
      gameObject.SetActive(true);
    }
  }

  void Awake() {
    gameObject.SetActive(false);
    button_ = GetComponent<Button>();
    button_.onClick.AddListener(() => {
      if(CanUse) {
        Count--;
        button_.interactable = false;
        StartCoroutine(ReEnable());
        NotifyEvent(new UseItemPickupEventArgs(item_));
      }
    });
  }

  IEnumerator ReEnable() {
    yield return new WaitForSeconds(0.8f);
    button_.interactable = !(Count <= 0);
    yield return null;
  }

  public void ItemFly() {

  }
}
