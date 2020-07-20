using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyItemEventArgs : ButtonEventArgs {
  public ItemPickup BuyItem { get; private set; }
  public int Amount { get; private set; }
  public ShopBuyItemEventArgs(ItemPickup item, int amount) 
    : base("BuyItem") {
    BuyItem = item;
    Amount = amount;
  }
} 

public class ShopItemDisplay : GameScript {
  public ItemPickupSo Character {
    set {
      pickup_item_.Character = value;
      txt_name_.text = pickup_item_.Name;
      txt_price_.text = pickup_item_.Price.ToString();
      txt_usage_.text = pickup_item_.Usage;
    }
  }
  [SerializeField]
  private ItemPickup pickup_item_;
  [SerializeField]
  private TMP_Text txt_name_;
  [SerializeField]
  private TMP_Text txt_usage_;
  [SerializeField]
  private TMP_Text txt_price_;
  [SerializeField]
  private Button btn_buy_;
  [SerializeField]
  private Button btn_ads_;

  void Start() {
    btn_ads_.gameObject.SetActive(UnityEngine.Random.Range(0, 2) > 0 ? true : false);
    btn_buy_.onClick.AddListener(() => {
      btn_buy_.interactable = false;
      btn_ads_.interactable = !pickup_item_.Character.IsOnlyOneLevel;
      NotifyEvent(new ShopBuyItemEventArgs(pickup_item_, pickup_item_.Price));
    });
    btn_ads_.onClick.AddListener(() => {
      btn_ads_.interactable = false;
      //TODO: show ads
      NotifyEvent(new ShopBuyItemEventArgs(pickup_item_, 0));
    });
  }
}
