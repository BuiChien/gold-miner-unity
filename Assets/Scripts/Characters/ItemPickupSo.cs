using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemPickupType {
  BOMB,
  ROCK_BOOK,
  CLOCK,
  POWER,
  CLOVER,
  DIAMOND
}

[CreateAssetMenu(fileName ="NewItemPickupStats", menuName = "Scriptable/ItemPickup", order = 1)]
public class ItemPickupSo : ScriptableObject {
  public string Name;
  public ItemPickupType Type;
  public Sprite Icon;
  public Sprite InventoryIcon;
  public int MinPrice;
  public int MaxPrice;
  public string Usage;
  public bool IsOnlyOneLevel;
}
