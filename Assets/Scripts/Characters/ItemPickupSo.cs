using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemPickupType {
  BOMB,
  BOOK_STONE,
  CLOCK,
  POWER
}

[CreateAssetMenu(fileName ="NewItemPickupStats", menuName = "Scriptable/ItemPickup", order = 1)]
public class ItemPickupSo : ScriptableObject {
  public string Name { get; set; }
  public ItemPickupType Type { get; set; }
  public Material Material { get; set; }
  public Sprite Icon { get; set; }
  public Rigidbody SpawnObject { get; set; }
  public int Price { get; set; }
}
