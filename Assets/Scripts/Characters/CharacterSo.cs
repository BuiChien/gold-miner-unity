using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Scriptable/Stats", order = 1)]
public class CharacterSo : ScriptableObject {
  [System.Serializable]
  public class CharLevelUps {
    public int maxHealth;
    public int maxMana;
    public int maxWealth;
    public int baseDamage;
    public float baseResistance;
    public float maxEncumbrance;
  }
  #region PublicFields
  public int MaxHealth = 0;
  public int CurrentHealth = 0;

  public int MaxWealth = 0;
  public int CurrentWealth = 0;

  public int MaxMana = 0;
  public int CurrentMana = 0;

  public int BaseDamage = 0;
  public int CurrentDamage = 0;

  public float BaseResistance = 0;
  public float CurrentResistance = 0f;

  public float MaxEncumbrance = 0f;
  public float CurrentEncumbrance = 0f;

  public int CharExperience = 0;
  public int CharLevel = 0;

  public CharLevelUps[] charLevelUps;
  #endregion

}