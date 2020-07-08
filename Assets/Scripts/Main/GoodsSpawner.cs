using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsSpawner : MonoBehaviour, ISpawner {
  [SerializeField]
  private GoodsSo[] list_goods_;
  public List<GoodsSo> GoodsSpawned;
  public void Spawn(int level) {

  }

  public void Clear() {

  }
}
