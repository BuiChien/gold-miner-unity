using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewOperationData", menuName = "Scriptable/OperationData", order = 3)]
public class OperationData : ScriptableObject {
  [System.Serializable]
  public class LevelDataInfo {
    public int TargetScore;
  }
  public int Level;
  public LevelDataInfo[] LevelData;
}
