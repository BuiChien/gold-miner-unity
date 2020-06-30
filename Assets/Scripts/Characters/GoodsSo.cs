using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoodsType {

}

[CreateAssetMenu(fileName = "NewGoodsStats", menuName = "Scriptable/Goods", order = 2)]
public class GoodsSo : ScriptableObject {
  public string Name { get; set; }
  public GoodsType Type { get; set; }
  public Material Material { get; set; }
  public Sprite Icon { get; set; }
  public Rigidbody SpawnObject { get; set; }
  public int Score { get; set; }
}
