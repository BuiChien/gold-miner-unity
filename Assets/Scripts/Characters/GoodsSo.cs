using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoodsType {
  GOLD_SMALL,
  GOLD_MEDIUM,
  GOLD_HUGE,
  ROCK_SMALL,
  ROCK_MEDIUM,
  ROCK_HUGE,
  DIAMOND_VIOLET,
  DIAMOND_PINK,
  DIAMOND_GREEN,
  DIAMOND_YELLOW,
  SKULLCAP,
  BONE,
  GIFT_BAG,
  DOLAR_BAG,
  MOUSE,
  MOUSE_PLUS
}

[CreateAssetMenu(fileName = "NewGoodsStats", menuName = "Scriptable/Goods", order = 2)]
public class GoodsSo : ScriptableObject {
  public GoodsType Type;
  public Sprite Icon;
  public float Scale;
  public bool IsHeavy;
  public int ScoreAmount;
  public int MaxAmount;
  public string Tag;
}
