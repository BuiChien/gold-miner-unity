using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewOperationData", menuName = "Scriptable/OperationData", order = 3)]
public class OperationData : ScriptableObject {
  public int Level;
  public int TotalScore;
  public int TargetScore;
  public int BombCount;
}
