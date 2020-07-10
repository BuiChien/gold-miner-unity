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
  BONE
}

[CreateAssetMenu(fileName = "NewGoodsStats", menuName = "Scriptable/Goods", order = 2)]
public class GoodsSo : ScriptableObject {
  public GoodsType Type;
  public Sprite Icon;
  public Animator GoodsAnimator;
  public float Scale;
  public bool IsHeavy;
  public int ScoreAmount;
}
